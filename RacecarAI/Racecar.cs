using System;
using System.Net;

namespace RacecarAI {
    public class Racecar {

        private const int MAX_SPEED = 5;
        private const int MAX_ACCEL = 1;
        
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

        public Racecar(int posX, int posY, int velX, int velY, int accelX, int accelY) {
            x_pos = posX;
            y_pos = posY;
            x_vel = Util.Clamp(velX, -MAX_SPEED, MAX_SPEED);
            y_vel = Util.Clamp(velY, -MAX_SPEED, MAX_SPEED);
            x_accel = Util.Clamp(accelX, -MAX_ACCEL, MAX_ACCEL);
            y_accel = Util.Clamp(accelY, -MAX_ACCEL, MAX_ACCEL);
        }

        public override string ToString() {
            string o = "";
            o += "Racecar Stats (X,Y): \n";
            o += "Position:       " + x_pos + " " + y_pos + "\n";
            o += "Velocity:       " + x_vel + " " + y_vel + "\n";
            o += "Acceleration:   " + x_accel + " " + y_accel + "\n";
            return o;
        }

        public override bool Equals(object obj) {
            if (obj is Racecar) {
                var other = (Racecar) obj;
                return x_pos == other.GetXPos() &&
                       y_pos == other.GetYPos() &&
                       x_vel == other.GetXVel() &&
                       y_vel == other.GetYVel() &&
                       x_accel == other.GetXAccel() &&
                       y_accel == other.GetYAccel();
            }
            
            return base.Equals(obj);
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