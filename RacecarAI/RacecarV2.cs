using System;
using System.Net;

namespace RacecarAI {
    public class RacecarV2 {

        private RacecarRules rules;
        // Read-Only Stats
        private int x_vel = 0;
        private int y_vel = 0;
        private int x_accel = 0;
        private int y_accel = 0;
        private int x_pos = -1;
        private int y_pos = -1;

        public int GetXVel() => x_vel;
        public int GetYVel() => y_vel;
        public int GetXAccel() => x_accel;
        public int GetYAccel() => y_accel;
        public int GetXPos() => x_pos;
        public int GetYPos() => y_pos;

        public RacecarV2() {
            rules = new RacecarRules(this);
        }

        public override string ToString() {
            string o = "";
            o += "Racecar Stats (X,Y): \n";
            o += "Position:       " + x_pos + " " + y_pos + "\n";
            o += "Velocity:       " + x_vel + " " + y_vel + "\n";
            o += "Acceleration:   " + x_accel + " " + y_accel + "\n";
            return o;
        }

        // private int vel_x = 0;
         //
         // public void SetXVelocity(int value) {
         //     if (IsValidVelocity(value)) {
         //        vel_x = value;
         //     }
         //    
         //     throw new Exception("Cannot change velocity to: " + value);
         // }
         //
         // public void SetYVelocity(int value) {
         //     if (IsValidVelocity(value)) {
         //        vel_y = value;
         //     }
         //
         //     throw new Exception("Cannot change velocity to: " + value);
         // }

         // private bool IsValidVelocity(int value) {
         //     if (value > max_velocity || value < min_velocity) {
         //         return false;
         //     }
         //     return true;
         // }
         //
         // public int Velocity_X {
         //     get {
         //         return vel_x;
         //     }
         //     set {
         //         int accel = value;
         //         // prevent the car from accelerating more or less than + or - 1 unity per time step
         //         if (accel > max_acceleration || accel < min_acceleration) {
         //             throw new Exception("Can't accelerate the car at speed " +accel);
         //         }
         //         // can't have a velocity greater than + 5 or less than - 5
         //         if (vel_x + accel > max_velocity || vel_x - accel < min_velocity) {
         //             vel_x += 0; // don't do anything > Q: is there another way to do this? 
         //         } else {
         //             vel_x += accel;
         //         }
         //     }
         // }
        // public int GetXVelocity => vel_x;
         //private int vel_y = 0;
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