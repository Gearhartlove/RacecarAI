using System;

namespace RacecarAI {
    public class ProgramDriver {

        public void RunProgram() {
            RacetrackParcer parcer = new RacetrackParcer();
            Racetrack l_track = parcer.Parse("L-Track");
            
            // printint out track with car test
            Console.WriteLine(l_track);
        }
    }
}
