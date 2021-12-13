using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;

namespace RacecarAI {
    
    public class ValueIteration {
        private double success = 0.8;
        private double failiure = 0.2;
        private double r = -0.04;
        
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
            double max_change = 0; //??
            double gamma = 0.05; // low = lower preference for future
            double max_error = 0.1; //??
            // data structures for utilities of racetrack, initially zero
            UtilityFunction oldU = new UtilityFunction(racetrack);
            UtilityFunction newU = new UtilityFunction(racetrack);
            // maximum relative change 
            while (max_change <= (max_error*((1-gamma)/gamma))) {
                oldU = newU.DeepCopy(); // TODO test implementation
                max_change = 0; // reset
                // loop through every state
                // update the utility of track and starting area, start from the end going forward
                // Go through each one of the cells
                for (int i = 0; i < oldU.GetFunction.Length; i++) {
                    // If not a wall or a finish line, continue
                    if (oldU.GetFunction[i].Length > 1) {
                        // go through each of the states for each cell and perform a Bellman equation on them
                        // TODO: Fix problem where the utility of the walls and the finish line are trying to be updated
                        for (int j = 0; j < oldU.GetFunction[i].Length; j++) {
                            // Don't update the walls and the finish line utility
                            // only update the start and the track cells on the Racetrack
                            // considered state
                            State cs = newU.GetFunction[i][j];
                            // Q value is returning the same value always, not updating properly
                            if (racetrack[cs.GetYPos, cs.GetXPos] != TrackComponent.Wall || racetrack[cs.GetYPos, cs.GetXPos] != TrackComponent.Finish) {
                                newU.GetFunction[i][j].UpdateUtility(MaxUtility(
                                    QValue(racetrack, oldU,j,i,gamma)));
                                if (newU.GetFunction[i][j].GetUtility - oldU.GetFunction[i][j].GetUtility >
                                    max_change) {
                                    max_change = newU.GetFunction[i][j].GetUtility - oldU.GetFunction[i][j].GetUtility;
                                }
                            }
                        }
                    }
                }
            }
            return oldU;
        }

        // Returns the utility value for calculating the bellman equation
        public List<double> QValue(Racetrack racetrack, UtilityFunction U, int x, int y, double gamma) {
            List<double> utilities = new List<double>();
            // reference to the states below
            // look at each different action and make a calculation,
            // add each calculation to the utilities and return the utilities
            // TODO: kick it out to the utility function to avoid multiple memory instantiations
            List<int[]> accel_perms = new List<int[]>();
            int[] accel = new int[] {-1, 0, 1};
            for (int x_a = 0; x_a < accel.Length; x_a++) {
                for (int y_a = 0; y_a < accel.Length; y_a++) {
                    accel_perms.Add(new int[] {accel[x_a], accel[y_a]});
                }
            }

            // go through each action or "accel_perms" 
            foreach (int[] action in accel_perms) {
                int next_y_vel = ClampVelocity(racetrack.GetRacecar.GetYVel() + action[1], racetrack.GetRacecar);
                int next_x_vel = ClampVelocity(racetrack.GetRacecar.GetXVel() + action[0], racetrack.GetRacecar);
                if (action[0] == 0 && action[1] == 0) {
                    //100% chance to do what want :)
                    double new_util = (1 * (r + gamma * U[x, y, next_x_vel, next_y_vel].GetUtility));
                    utilities.Add(new_util);
                }
                else {
                    // 80% chance to do what I want and a 20% chance to ignore the desired action
                    double Dcurrent_util = U[x, y, next_x_vel, next_y_vel].GetUtility;
                    double DFuturecurrent_util = U[x, y, next_x_vel+action[0], next_y_vel].GetUtility+action[1];
                    //Console.WriteLine("succes: " + success * (r + gamma * U[x, y, next_x_vel, next_y_vel].GetUtility));
                    Console.WriteLine("failiure: " + failiure *
                        (r + gamma * U[x+next_x_vel, y+next_y_vel, next_x_vel, next_y_vel].GetUtility));
                    //double new_util = (success * (r + gamma * U[x, y, next_x_vel, next_y_vel].GetUtility)) +
                    //                 (failiure * (r + gamma * U[x, y, next_x_vel-action[0], next_y_vel-action[1]].GetUtility));
                    //Console.WriteLine("New util: " + new_util);
                    utilities.Add(0.0);
                }
            }
            
            return utilities;
        }

        private int ClampVelocity(int value, Racecar racecar) {
            if (value < 0) {
                if (value > racecar.GetMAX_SPEED) {
                    return -racecar.GetMAX_SPEED;
                }
                return value;
            }
            else {
                if (value > racecar.GetMAX_SPEED) {
                    return racecar.GetMAX_SPEED;
                }
                return value;
            }
        }

        // TODO: implement
        public double MaxUtility(List<double> utilities) {
            double max = 0;
            for (int i = 0; i < utilities.Count; i++) {
                if (utilities[i] >= max) {
                    max = utilities[i];
                }
            }
            return max;
        }
    }
}