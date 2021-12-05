using System;

namespace RacecarAI {
    public class ProgramDriver {

        public void RunProgram() {
            RacetrackParcer parcer = new RacetrackParcer();
            Racetrack l_track = parcer.Parse("L-Track");
            
            // printint out track with car test
            l_track.GetRacecar.xtLocation = 1;
            l_track.GetRacecar.ytLocation = 8;
            Console.WriteLine(l_track);
        }
    }
}
