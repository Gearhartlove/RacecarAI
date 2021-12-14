using System;
using System.Collections.Generic;
using System.Xml;

namespace RacecarAI {
    
    public class Racetrack {
        TrackComponent[,] racetrack;
        private Racecar racecar = new Racecar(-1, -1, 0, 0, 0, 0);
        public Racecar GetRacecar => racecar;

        public List<int[]> start_spots = new List<int[]>();
        public List<int[]> GetStartSpots => start_spots;
        
        private ProbabilityTable<Tuple<int, int>> startPositions = new ProbabilityTable<Tuple<int, int>>();
        
        private int x = 0; // initialize track to size 0
        private int y = 0; // initialize track to size 0
        private int timestep = 0;
        public int GetTimestep => timestep;
        
        // indexer to make Racetrack API more accessible
        //ValueIterationBranch
        //public TrackComponent this[int indexX, int indexY] {
            //get => racetrack[indexY, indexX];

  
        //TODO: Fis this issue :)
        public TrackComponent this[int indexY, int indexX, debug] {
            get => racetrack[Util.Clamp(indexY, 0, y-1), Util.Clamp(indexX, 0, x-1)];
          
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

        public void addStartPos(int x, int y) {
            startPositions.add(new Tuple<int, int>(x, y), 1);
        }

        public Tuple<int, int>[] getStartPosistions() {
            return startPositions.Data;
        }

        public Racecar rollRandomStartCar() {
            var start = startPositions.roll();
            return new Racecar(start.Item1, start.Item2, 0, 0, 0, 0);
        }

        public string getDisplay(Racecar racecar) {
            string o = "";
            o += "Timestep: " + GetTimestep + "\n";
            for (int row = 0; row < y; row++) {
                for (int column = 0; column < x; column++) {
                    if (racecar.GetXPos() == column && racecar.GetYPos() == row) {
                        o += "\u2588";
                    } else {
                        switch (this[column, row]) {
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
            o += racecar.ToString();
            o += "--------------------------------------------------";
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