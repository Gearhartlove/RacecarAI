using System;
using System.Collections.Generic;
using System.Net;

namespace RacecarAI {
    
    /// <summary>
    /// Class which runs the UtilityFunction created from the Value Iteration Algorithm.
    /// </summary>
    public class Policy {
        private UtilityFunction uf;
        private Racetrack rt;
        private Random rand;
        private int rowsize;
        private List<Tuple<int,int>> traveled = new List<Tuple<int,int>>();
        
        public Policy(UtilityFunction uf, Racetrack rt) {
            this.uf = uf;
            this.rt = rt;
            rand = new Random();
            rowsize = rt.YTracksize;
        }

        /// <summary>
        /// Randomly selects the start spot for the car to take on the track
        /// </summary>
        /// <returns></returns>
        public Tuple<int, int> ChooseStartSpot() {
            int rand_spot = rand.Next(rt.GetStartSpots.Count);
            return new Tuple<int, int>(rt.GetStartSpots[rand_spot][0], rt.GetStartSpots[rand_spot][1]);
        }

        /// <summary>
        /// Runs the desired policy created from the UtilityFunction in the ValueIteration algorithm.
        /// </summary>
        public void Run() {
            Racecar rc = rt.GetRacecar;
            Console.WriteLine(rt.getDisplay(rt.GetRacecar));
            rt.GetRacecar.Spawn(ChooseStartSpot());
            // set race car position to start
            while (rt[rt.GetRacecar.GetXPos(),rt.GetRacecar.GetYPos()] != TrackComponent.Finish || 
                   rt[rt.GetRacecar.GetXPos(),rt.GetRacecar.GetYPos()] != TrackComponent.Wall) { 
                // print out the board 
                Console.WriteLine(rt.getDisplay(rt.GetRacecar));
                // move the car by: 1) consider all the states the car can take and try move to the best one; account
                // for a 80% chance of success and 20% of failure
                // Q: what is the car's range?
                // Considered squares
                var considered_states = GenerateConsiderations(rc);
                double best_util = Double.MinValue;
                State best_state = new State();
                foreach (var coord in considered_states) {
                    foreach (var state in uf[coord.Item1, coord.Item2]) {
                        if (state.GetUtility > best_util && state.GetXPos != rc.GetXPos() && 
                            state.GetYPos != rc.GetYPos() && !traveled.Contains(new Tuple<int, int>(state.GetXVel, state.GetYPos))) {
                            best_state = state;
                            best_util = state.GetUtility;
                        }
                    }
                }
                
                // travel to the best state around
                rc.Drive(best_state.GetXPos, best_state.GetYPos);
                traveled.Add(new Tuple<int, int> (best_state.GetXVel, best_state.GetYPos));
            }

            
        }

        private List<Tuple<int, int>> GenerateConsiderations(Racecar rc) {
            List<int> considered_x_states = new List<int>();
            List<int> considered_y_states = new List<int>();

            // X use case
            // same direction
            considered_x_states.Add(rc.GetXPos() + rc.GetXVel());
            // accelerate 
            if (rc.GetXVel() != 5)
                considered_x_states.Add(rc.GetXPos() + rc.GetXVel() + 1);
            // decelerate
            if (rc.GetXVel() != -5)
                considered_x_states.Add(rc.GetXPos() + rc.GetXVel() - 1);

            // Y use case
            // same direction
            considered_y_states.Add(rc.GetYPos() + rc.GetYVel());
            // accelerate 
            if (rc.GetYVel() != 5)
                considered_y_states.Add(rc.GetYPos() + rc.GetYVel() + 1);
            // decelerate
            if (rc.GetYVel() != -5)
                considered_y_states.Add(rc.GetYPos() + rc.GetYVel() - 1);

            List<Tuple<int, int>> considerations = new List<Tuple<int, int>>();
            for (int i = 0; i < considered_x_states.Count; i++) {
                for (int j = 0; j < considered_y_states.Count; j++) {
                    considerations.Add(new Tuple<int,int>(considered_x_states[i], considered_y_states[j]));
                }
            }

            return considerations;
        }
    }
}