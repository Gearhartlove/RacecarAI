using System;
using System.Xml;

namespace RacecarAI {
    public class Racetrack {
        TrackComponent[,] racetrack = null;
        private Racecar racecar = new Racecar();
        public Racecar GetRacecar => racecar;
        private int x = 0; // initialize track to size 0
        private int y = 0; // initialize track to size 0
        
        // indexer to make Racetrack API more accessible
        public TrackComponent this[int indexY, int indexX] {
            get => racetrack[indexY, indexX];
            // if the track index has not been assigned yet, then assign it to a value.
            // otherwise do not assign a value and print out a message
            set {
                if (racetrack[indexY, indexX] == TrackComponent.Initial) racetrack[indexY, indexX] = value;
                else Console.WriteLine("Cannot assign a track value twice");
            } 
        }
        
        // Get the number of columns for the track
        public int XTracksize {
            get => x;
            set {
                if (x == 0) {
                    x = value;
                }
                else {
                    Console.WriteLine("X Tracksize already assigned, cannot assign another value");
                }
            }
        }

        // Get number of rows for the track
        public int YTracksize {
            get => y;
            set {
                if (y == 0) {
                    y = value;
                }
                else {
                    Console.WriteLine("Y Tracksize already assigned, cannot assign another value");
                }
            }
        }

        // Set the size of the track, used in the RacetrackParcer class
        public void SetSize() {
            if (racetrack == null) {
                racetrack = new TrackComponent[y, x];
            }
            else Console.WriteLine("Size is already assigned, cannot reassign");
        }
        
        // Print out the track
        public override string ToString() {
            string o = "";
            for (int row = 0; row < y; row++) {
                for (int column = 0; column < x; column++) {
                    if (racecar.GetPosition().Item1 == column && racecar.GetPosition().Item2 == row) {
                        o += "\u2588";
                    } else {
                        switch (this[row, column]) {
                            case TrackComponent.Track:
                                o += ".";
                                break;
                            case TrackComponent.Finish:
                                o += "F";
                                break;
                            case TrackComponent.Start:
                                o += "S";
                                break;
                            case TrackComponent.Wall:
                                o += "#";
                                break;
                            case TrackComponent.Initial:
                                Console.WriteLine("Racetrack is either not instantiated or incorrectly constructed");
                                break;
                        }
                    }
                }

                o += "\n";
            }
            return o;
        }
    }
    
    // Components from which the track can be composed of
    public enum TrackComponent {
        Initial,
        Track,
        Start,
        Finish,
        Wall
    }
}