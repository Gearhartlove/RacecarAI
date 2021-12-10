using System;

namespace RacecarAI {
    public class ProgramDriver {

        public void RunProgram() {
            RacetrackParcer parcer = new RacetrackParcer();
            ValueIteration VI = new ValueIteration();
            Sarsa sarsa = new Sarsa();
            
            // track instantiation
            Racetrack l_track = parcer.Parse("L-Track");
            Racetrack r_track = parcer.Parse("R-Track");
            Racetrack o_track = parcer.Parse("O-Track"); 
            
            // run programs
            VI.Run(l_track);
            VI.Run(r_track);
            VI.Run(o_track);
            sarsa.Run(l_track);
            sarsa.Run(r_track);
            sarsa.Run(o_track);
            
            // printint out track with car test
            Console.WriteLine(l_track);
        }
    }
}
