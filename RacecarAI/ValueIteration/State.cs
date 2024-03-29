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

        /// <summary>
        /// Empty constructor used for default state instantiation.
        /// </summary>
        public State() {
            
        }
        
        // Constructor for State, used when creating a state object
        public State(int x, int y, TrackComponent component, int number, int sub_number, int xVel = Int32.MinValue,
            int yVel = Int32.MaxValue, double util=0) {
            x_pos = x;
            y_pos = y;
            utility = util;
            x_vel = xVel;
            y_vel = yVel;
            this.component = component;

            this.number = number;
            this.sub_number = sub_number;

            Position = new Tuple<int, int>(x_pos, y_pos);
            Velocity = new Tuple<int, int>(x_vel, y_vel);
        }
        
        // local variables used in the state object class
        private int x_pos;
        public int GetXPos => x_pos;
        private int y_pos;
        public int GetYPos => y_pos;
        private int x_vel;
        public int GetXVel => x_vel;
        private int y_vel;
        public int GetYVel => y_vel;
        
        // better debugging tools (no technical usage other than reading in the debug mode)
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
        public double GetUtility {
            get => utility;
            set => utility = value;
        }
    }
}