using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;

namespace RacecarAI {
    
    public class ValueIteration {
        private double success = 0.8;
        private double failiure = 0.2;
        private double r = -0.04;
        double gamma = 0.001; // low = lower preference for future
        
        // maximum relative change int he utility of any state
        public ValueIteration() {
            
        }

        // Whole goal of Value-Iteration is to make a utility function for each given state of the race track.
        // What does the agent need to do and when? 
        /// <summary>
        /// The MDP (Markov Decision Process) in this problem is represented by the racetrack data structure.
        /// It contains the States "S" in the form of positions on the board (x,y) and possible velocities for the car
        /// at those positions (x',y'). The Actions "A" are the change in acceleration of the Racecar (x'',y'').
        /// And the Transition Model is there is 80% for intended action, 20% for non-intended action. The rewards
        /// function follows these rules : TRACK=0, WALL=-1, FINISH_LINE=1, START_LINE=0;
        /// ??? maximum error allowed in the utility of any state
        /// 
        /// </summary>
        /// <param name="racetrack"></param>
        public UtilityFunction RunValueIteration(Racetrack racetrack) {
            // maximum relative change in the utility of any state
            double max_change = 0; // helps determine when the progranm needs to stop running 
            double max_error = 0.001; //??
            // data structures for utilities of racetrack, initially zero
            UtilityFunction oldU = new UtilityFunction(racetrack);
            UtilityFunction newU = new UtilityFunction(racetrack);
            // maximum relative change 
            int count = 0;
            while (max_change <= max_error*((1-gamma)/gamma)) {
                Console.WriteLine(++count);
                if (count == 5000) {
                    return oldU;
                }
                oldU = newU.DeepCopy(); // TODO test implementation
                max_change = 0; // reset
                // loop through every state
                // update the utility of track and starting area, start from the end going forward
                // Go through each one of the cells
                foreach (State[] cell_state in oldU.GetFunction) {
                    // ignore the walls and finish line

                    foreach (State sub_state in cell_state) {
                        // Update the track and the start state of the racetrack
                        if (sub_state.GetComponent != TrackComponent.Wall &&
                            sub_state.GetComponent != TrackComponent.Finish) {
                            // debugging Q function now
                            newU[sub_state].GetUtility = (MaxUtility(QValue(racetrack, oldU, sub_state)));
                            //Console.WriteLine(Math.Abs(newU[sub_state].GetUtility - oldU[sub_state].GetUtility));
                            if (Math.Abs(newU[sub_state].GetUtility - oldU[sub_state].GetUtility) > max_change) {
                                max_change = Math.Abs(newU[sub_state].GetUtility - oldU[sub_state].GetUtility);
                                Console.WriteLine(max_change);
                            }
                        }
                    }
                }
            }
            Console.WriteLine("sauce");
            return oldU;
        }

        // Returns the utility value for calculating the bellman equation
        public List<double> QValue(Racetrack racetrack, UtilityFunction U, State s) {
            List<double> utilities = new List<double>();
            // reference to the states below
            // look at each different action and make a calculation,
            // add each calculation to the utilities and return the utilities
            // TODO: kick it out to the utility function to avoid multiple memory instantiations
            List<int[]> actions = GetRacecarActions();
        
            // go through each action or "accel_perms" 
            foreach (int[] action in actions) {
                if (action[0] == 0 && action[1] == 0) {
                    //100% chance to do what want :)
                    double new_util = 1 * (r + gamma * U.FutureStateUtility(s, xVel: action[0], yVel: action[1]));
                    utilities.Add(new_util);
                }
                else {
                    // 80% chance to do what I want and a 20% chance to ignore the desired action
                    double new_util =
                        success * (r + gamma * U.FutureStateUtility(s, xVel: action[0], yVel: action[1])) +
                        failiure * (r + gamma * U.FutureStateUtility(s, xVel: 0, yVel: 0));
                    //Console.WriteLine("New util: " + new_util);
                    utilities.Add(new_util);
                }
            }
            return utilities;
        }

        private List<int[]> GetRacecarActions() {
            int[] accel = new int[] {-1, 0, 1};
            List<int[]> actions = new List<int[]>();
            for (int x_a = 0; x_a < accel.Length; x_a++) {
                for (int y_a = 0; y_a < accel.Length; y_a++) {
                     actions.Add(new int[] { accel[x_a], accel[y_a]});
                }
            }
            return actions;
        } 
        
        // TODO: implement
        public double MaxUtility(List<double> utilities) {
            double max = -1000;
            for (int i = 0; i < utilities.Count; i++) {
                if (utilities[i] >= max) {
                    max = utilities[i];
                }
            }
            return max;
        }
    }
}