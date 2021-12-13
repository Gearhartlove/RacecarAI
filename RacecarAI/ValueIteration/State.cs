using System;
using System.Collections.Generic;

namespace RacecarAI {
    /// <summary>
    /// State is the building block from which the utility function is created. Each state has an x and y position, along with a
    /// specific x and y velocity, and a utility. Each racetrack will have many states for each cell, which compensates for all possible
    /// values of the before mentioned variables the car can have. A utility with a score of -1 is very undesirable, whereas a utility
    /// score of 1 is very desirable.
    /// </summary>
    public class State {

        public State(int x, int y, int xVel, int yVel) {
            x_pos = x;
            y_pos = y;
            x_vel = xVel;
            y_vel = yVel;
        }

        public State(int x, int y, double util) {
            x_pos = x;
            y_pos = y;
            utility = util;
            // assigns velocity to an impossible value because the velocity is not considered for the wall 
            // and finish states
            x_vel = int.MaxValue;
            y_vel = int.MaxValue;
        }
        
        private int x_pos;
        public int GetXPos => x_pos;
        private int y_pos;
        public int GetYPos => y_pos;
        private int x_vel;
        public int GetXVel => x_vel;
        private int y_vel;
        public int GetYVel => y_vel;

        // utility initialized to zero
        private double utility = 0;
        public double GetUtility => utility;

        // called when the utility needs to be updated
        // aka every sweep 
        public void UpdateUtility(double new_util) {
            utility = new_util;
        }
        
        
        // public static State[] ToStateArray(Racetrack racetrack) {
        //     int range = racetrack.GetRacecar.GetMAX_SPEED;
        //     List<int[]> permutations = GetPermutations(racetrack, range);
        //     // loop through the board state and assign values for the track 
        //     
        //
        //     return new State[]{new State()};
        // }

        
        
    }
}