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

        public State(int x, int y, TrackComponent component, int number, int sub_number, int xVel = Int32.MinValue,
            int yVel = Int32.MaxValue, double util=0) {
            x_pos = x;
            y_pos = y;
            utility = util;
            x_vel = xVel;
            y_vel = yVel;
            this.component = component;

            Position = new Tuple<int, int>(x_pos, y_pos);
            Velocity = new Tuple<int, int>(x_vel, y_vel);
        }
        
        private int x_pos;
        public int GetXPos => x_pos;
        private int y_pos;
        public int GetYPos => y_pos;
        private int x_vel;
        public int GetXVel => x_vel;
        private int y_vel;
        public int GetYVel => y_vel;
        
        // better debugging
        public readonly Tuple<int, int> Position;
        public readonly Tuple<int, int> Velocity; 
        
        //what type of state is this (connected to the racetrack itself)
        private TrackComponent component;
        public TrackComponent GetComponent => component;
        
        // number to refer back to the state (make the api easier)
        private int number;
        public int GetNumber => number;
        private int sub_number;
        public int GetSubNumber => sub_number;

        // utility initialized to zero
        private double utility;
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