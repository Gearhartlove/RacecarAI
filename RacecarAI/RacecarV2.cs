using System;
using System.Net;

namespace RacecarAI {
    public class RacecarV2 {
         
        
        // // velocity
         private int vel_x = 0;

         public int Velocity_X {
             get {
                 return vel_x;
             }
             set {
                 // NOTE: value == acceleration
                 // prevent the car from accelerating more or less than + or - 1 unity per time step
                 if (value > 1 || value < -1) {
                     throw new Exception("Can't accelerate the car at speed " + value);
                 }
                 // can't have a velocity greater than + 5 or less than - 5
                 if (vel_x + value > 5 || vel_x - value < -5) {
                     vel_x += 0; // don't do anything > is there another way to do this? 
                 } else {
                     vel_x += value;
                 }
             }
         }
        // public int GetXVelocity => vel_x;
         private int vel_y = 0;
        // public int GetYVelocity => vel_y;
        // // acceleration
        // private int accel_x = 0;
        // public int GetXAccel => accel_x;
        // private int accel_y = 0;
        // public int GetYAccel => accel_y;
        // // position
        // private int x_pos = -1;
        // public int GetXPos => x_pos; 
        // private int y_pos = -1;
        // public int GetYPos => y_pos; 
        
    }
}