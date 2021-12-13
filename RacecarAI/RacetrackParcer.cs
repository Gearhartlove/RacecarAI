using System;
using System.IO;
using System.Linq;

namespace RacecarAI {
    public class RacetrackParcer {
        
        private Racetrack racetrack;
        
        public Racetrack Parse(string fileName) {
            racetrack = new Racetrack();
            // instantiate a new racetrack
            // instantiate correct file path
            string[] file = File.ReadAllLines("../../Racetracks/" + fileName + ".txt");
            // get race track length
            string[] racetrackSize = file[0].Split(',');
            racetrack.XTracksize = Int32.Parse(racetrackSize[1]);
            racetrack.YTracksize = Int32.Parse(racetrackSize[0]);
            file = file.Skip(1).ToArray(); // skip the first element of the track
            // instantiate the race track size
            // instantiate the race track size
            racetrack.SetSize();

            // Go line by line through the .txt file and create an enum for each character, assign the component
            // to the TrackComponent array in Racetrack
            for (int line = 0; line < file.Length; line++ ) {
                for (int character = 0; character < file[line].Length; character++) {
                    switch (file[line][character]) {
                        case '#':
                            AssignComponent(line, character, TrackComponent.Wall);
                            break;
                        case '.':
                            AssignComponent(line, character, TrackComponent.Track); 
                            break;
                        case 'F':
                            AssignComponent(line, character, TrackComponent.Finish); 
                            break;
                        case 'S':
                            AssignComponent(line, character, TrackComponent.Start); 
                            racetrack.addStartPos(character, line);
                            break;
                    }
                }
            }
            return racetrack;
        }

        private void AssignComponent(int y, int x, TrackComponent comp) {
            racetrack[y, x] = comp;
        }
    }
}