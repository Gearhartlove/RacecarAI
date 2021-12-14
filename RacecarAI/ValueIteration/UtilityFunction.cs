using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;

namespace RacecarAI {
    public class UtilityFunction {

        // UtilityFunction local variables
        public int row_count = 0;
        private State[][] function;
        public State[][] GetFunction => function;

        // Intentional empty constructor used when making a deep copy of UtilityFunction in the Q function for 
        // Value iteration
        public UtilityFunction() {
            
        }

        // indexer to make API for making accessing states simpler
        public State[] this[int i, int j] {
            get {
                if (i + j * row_count < 0 || i + j * row_count > function.Length) {
                    return function[0]; // always return a wall
                }
                return function[i + j * row_count];
            }
        }

        // indexer used for referencing another state using a state to begin with 
        public State this[State s] {
            get { return function[s.GetNumber][s.GetSubNumber]; }
        }

        /// <summary>
        /// Constructor used to create a UtilityFunction from a race track; input: racetrack, output: UtilityFunction.
        /// Goes cell by cell and creates the desired states for the cells, which the car can travel over.
        /// </summary>
        /// <param name="racetrack"></param>
        public UtilityFunction(Racetrack racetrack) {
            row_count = racetrack.YTracksize;
            // Get permutations for the racetrack
            List<int[]> vel_permutations = GetPermutations(racetrack);

            function = new State[racetrack.YTracksize * racetrack.XTracksize][];
            for (int col = 0; col < racetrack.XTracksize; col++) {
                for (int row = 0; row < racetrack.YTracksize; row++) {
                    // for indexing the function specifically ( I don't like this but . . .) 
                    int rowf = row;
                    rowf *= racetrack.XTracksize;
                    int number = col + (racetrack.YTracksize * row);
                    switch (racetrack[col, row]) {
                        case TrackComponent.Start:
                        case TrackComponent.Track:
                            // create a state for every position of size = number of possible velocity permutations
                            // NOTE: utility initialized to zero
                            function[col + rowf] = new State[vel_permutations.Count];
                            // assigns the desired velocity and correct position to each state
                            SetUpState(col, row, function[col + rowf], TrackComponent.Track,
                                num: number, vel_permutations, 0);
                            break;
                        case TrackComponent.Wall:
                            function[col + rowf] = new State[1];
                            SetUpState(col, row, function[col + rowf], TrackComponent.Wall,
                                number, vel_permutations, -1);
                            break;
                        case TrackComponent.Finish:
                            function[col + rowf] = new State[1];
                            SetUpState(col, row, function[col + rowf], TrackComponent.Finish,
                                number, vel_permutations, 1);
                            break;

                    }
                }
            }
        }
        
        /// <summary>
        /// Private method used to help set up the states created above (remove what would be repeated code from the
        /// switch statement above).
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="states"></param>
        /// <param name="component"></param>
        /// <param name="num"></param>
        /// <param name="perms"></param>
        /// <param name="u"></param>
        private void SetUpState(int x, int y, State[] states, TrackComponent component, int num, List<int[]> perms,
            double u) {
            for (int i = 0; i < states.Length; i++) {
                states[i] = new State(x, y, component, num, sub_number: i, xVel: perms[i][0], yVel: perms[i][1],
                    util: u);
            }
        }

        /// <summary>
        /// Gets the future state or s' utility, used in value iteration class while conducting the Q function
        /// </summary>
        /// <param name="s"></param>
        /// <param name="xVel"></param>
        /// <param name="yVel"></param>
        /// <returns></returns>
        public double FutureStateUtility(State s, int xVel, int yVel) {
            int newXVel = 0;
            int newYVel = 0;
            // check if X velocity OOB
            if (xVel + s.GetXVel <= 5 && xVel + s.GetXVel >= -5) {
                newXVel = xVel + s.GetXVel;
            }

            // check if Y velocity OOB
            if (yVel + s.GetYVel <= 5 && yVel + s.GetYVel >= -5) {
                newYVel = yVel + s.GetYVel;
            }

            if (s.GetNumber + newXVel + row_count * newYVel >= 0 && 
                s.GetNumber + newXVel + row_count * newYVel < function.Length) {
                int debug_num = s.GetNumber + (newXVel) + (row_count * newYVel);
                return function[s.GetNumber + (newXVel) + (row_count * newYVel)][0].GetUtility;
            }

            // if OOB consider it to be a wall
            else return -1;
        }

        /// <summary>
        /// Get every possible combination of speed values for the Racecar.
        /// The number of permutations depends on the Max speed of the racecar.
        /// </summary>
        /// <param name="racetrack"></param>
        /// <param name="range"></param>
        /// <returns></returns>
        public static List<int[]> GetPermutations(Racetrack racetrack) {
            int range = racetrack.GetRacecar.GetMAX_SPEED;
            List<int[]> permutations = new List<int[]>();
            for (int i = -range; i < range + 1; i++) {
                for (int j = -range; j < range + 1; j++) {
                    int[] combination = new[] {i, j};
                    permutations.Add(combination);
                }
            }

            return permutations;
        }

        /// <summary>
        /// Creates a deep copy of the UtilityFunction class.
        /// </summary>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public UtilityFunction DeepCopy() {
            if (function == null) {
                throw new Exception("Can't deep copy a null utility function!");
            }

            UtilityFunction other = new UtilityFunction();

            other.function = new State[function.Length][];
            
            for (int i = 0; i < other.function.Length; i++) {
                other.function[i] = new State[this.function[i].Length];
                for (int j = 0; j < other.function[i].Length; j++) {
                    
                    other.function[i][j] = new State(function[i][j].GetXPos, function[i][j].GetYPos,
                        function[i][j].GetComponent, function[i][j].GetNumber,
                        function[i][j].GetSubNumber, function[i][j].GetXVel, function[i][j].GetYVel,
                        function[i][j].GetUtility);
                }
            }
            other.row_count = row_count;
            return other;
        }
    }
}