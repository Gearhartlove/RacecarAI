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
            get {
                if (i + j * row_count < 0 || i + j * row_count > function.Length) {
                    return function[0]; // always return a wall
                }
                return function[i + j * row_count];
            }
        }

        public State this[State s] {
            get { return function[s.GetNumber][s.GetSubNumber]; }
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
            // assign the states velocity values 
        }

        // Called by the Track and Start position states
        private void SetUpState(int x, int y, State[] states, TrackComponent component, int num, List<int[]> perms,
            double u) {
            for (int i = 0; i < states.Length; i++) {
                states[i] = new State(x, y, component, num, sub_number: i, xVel: perms[i][0], yVel: perms[i][1],
                    util: u);
            }
        }

        // Get future state utility
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

            return function[s.GetNumber + (newXVel) + (7 * newYVel)][0].GetUtility;
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