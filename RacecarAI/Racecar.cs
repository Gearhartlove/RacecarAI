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

        public void Spawn(Tuple<int, int> coords) {
            x_pos = coords.Item1;
            y_pos = coords.Item2;
        }
    }
}