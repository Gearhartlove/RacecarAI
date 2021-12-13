using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace RacecarAI {
    public class UtilityFunction {
        
        // utility function
        // array of state arrays 
        public int row_count = 0;
        private State[][] function;
        public State[][] GetFunction => function;

        public UtilityFunction() {
            
        }

        // indexer to make API for accesding states simpler
        public State[] this[int i, int j] {
            get { return function[i + j * row_count]; }
        }

        public State this[int j, int i, int x_vel, int y_vel] {
            get {
                // TODO : i or j ?? order ??
                if (function[i + j * row_count][0].GetUtility == 1) return function[j][i];
                if (function[i + j * row_count][0].GetUtility == -1) return function[j][i];
                foreach (State s in function[i + j * row_count]) { 
                    //Console.WriteLine(s.GetXVel + " " + s.GetYVel);
                    if (s.GetXVel == x_vel && s.GetYVel == y_vel) {
                        return s;
                    }
                }

                throw new Exception("No matching velocity found");
            }
        }
        
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
                    switch (racetrack[col, row]) {
                        case TrackComponent.Start:
                        case TrackComponent.Track:
                            // create a state for every position of size = number of possible velocity permutations
                            // NOTE: utility initialized to zero
                            function[col + rowf] = new State[vel_permutations.Count];
                            // assigns the desired velocity and correct position to each state
                            SetUpState(function[col + rowf], col, row, vel_permutations);
                            break;
                        case TrackComponent.Wall:
                            function[col + rowf] = new State[1];
                            SetUpState(function[col + rowf], col, row, vel_permutations, -1);
                            break;
                        case TrackComponent.Finish:
                            function[col + rowf] = new State[1];
                            SetUpState(function[col + rowf], col, row, vel_permutations, 1);
                            break;
                        
                    }
                }
            }
            // assign the states velocity values 
        }

        // Called by the Track and Start position states
        private void SetUpState(State[] states, int x, int y, List<int[]> perms) {
            for (int i = 0; i < states.Length; i++) {
                states[i] = new State(x, y, perms[i][0], perms[i][1]);
            }
        }

        // Called by the Wall and Finish line states
        private void SetUpState(State[] states, int x, int y, List<int[]> perms, double util) { 
            states[0] = new State(x, y, util);
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
                    // Console.WriteLine(i + " " + j);
                    permutations.Add(combination);
                }
            }

            return permutations;
        }

        /// <summary>
        /// Create a copy (not reference) of the utility function and return it.
        /// </summary>
        /// <returns></returns>
        public UtilityFunction DeepCopy() {
            if (function == null) {
                throw new Exception("Can't deep copy a null utility function!");
            }
            return new UtilityFunction() {function = this.function };
        }
    }
}