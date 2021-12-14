using System;
using System.Net;

namespace RacecarAI {
    public class Racecar {

        private const int MAX_SPEED = 5;
        public int GetMAX_SPEED => MAX_SPEED;
        private const int MAX_ACCEL = 1;
        
        // Read-Only Stats
        private int x_vel = 0;
        private int y_vel = 0;
        private int x_accel = 0;
        private int y_accel = 0;
        private int x_pos = 0;
        private int y_pos = 0;

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
        
        // actions for the Racecar to make
        public enum Actions {
            Nothing,
            Accelerate,
            Decelerate,
        }

        /// <summary>
        /// ToString() used to print out the racecar stats in conjunction with the board.
        /// </summary>
        /// <returns></returns>
        public override string ToString() {
            string o = "";
            o += "Racecar Stats (X,Y): \n";
            o += "Position:       " + x_pos + " " + y_pos + "\n";
            o += "Velocity:       " + x_vel + " " + y_vel + "\n";
            o += "Acceleration:   " + x_accel + " " + y_accel + "\n";
            return o;
        }

        public static bool operator ==(Racecar racecar1, Racecar racecar2) {
            return racecar1.Equals(racecar2);
        }
        
        public static bool operator !=(Racecar racecar1, Racecar racecar2) {
            return !racecar1.Equals(racecar2);
        }


        /// <summary>
        /// Prevents the velocity of the car from being more or less than its bounds (-5 to 5 in this problem's case)
        /// </summary>
        /// <param name="value"></param>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <returns></returns>
        public int Clamp(int value, int min, int max) {
            if (value < min) return min;
            if (value > max) return max;
            return value;
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

        public override int GetHashCode() {
            return x_pos.GetHashCode() ^
                   y_pos.GetHashCode() ^
                   x_vel.GetHashCode() ^
                   y_vel.GetHashCode() ^
                   x_accel.GetHashCode() ^
                   y_accel.GetHashCode();
        }

        /// <summary>
        /// Spawns the car on the racecar track, according to an x and y value
        /// </summary>
        /// <param name="coords"></param>
        public void Spawn(Tuple<int, int> coords) {
            x_pos = coords.Item1;
            y_pos = coords.Item2;
        }

        /// <summary>
        /// Move the racecar to the desired x,y coordinate
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        public void Drive(int x, int y) {
            if (x_vel + x_pos < x) {
                x_vel += 1;
            } else if (x_vel + x_pos > x) {
                x_vel -= 1;
            }

            if (y_vel + y_pos < y) {
                y_vel += 1;
            } else if (y_vel + y_pos > y) {
                y_vel -= 1;
            }
            
            x_pos = x;
            y_pos = y;
        }
    }
}