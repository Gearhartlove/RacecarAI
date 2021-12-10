using System;
using System.Runtime.InteropServices;

namespace RacecarAI {
    public class ProgramDriver {

        public void DebugRunProgram() {
            RacetrackParcer parcer = new RacetrackParcer();
            ValueIteration vi = new ValueIteration();
            Sarsa sarsa = new Sarsa();
            
            // track instantiation
            Racetrack debug_track = parcer.Parse("Debug-Track");
            Console.WriteLine(debug_track);
            
            // run algorithms
            vi.Run(debug_track);
            sarsa.Run(debug_track);
        }
        
        public void RunProgram() {
            RacetrackParcer parcer = new RacetrackParcer();
            ValueIteration vi = new ValueIteration();
            Sarsa sarsa = new Sarsa();
            
            // track instantiation
            Racetrack l_track = parcer.Parse("L-Track");
            Racetrack r_track = parcer.Parse("R-Track");
            Racetrack o_track = parcer.Parse("O-Track"); 
            
            // run algorithms
            vi.Run(l_track);
            vi.Run(r_track);
            vi.Run(o_track);
            sarsa.Run(l_track);
            sarsa.Run(r_track);
            sarsa.Run(o_track);
            
            // printint out track with car test
            Console.WriteLine(l_track);
        }
    }
}
