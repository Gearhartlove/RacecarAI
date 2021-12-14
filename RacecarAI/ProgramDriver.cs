using System;
using System.Runtime.InteropServices;

namespace RacecarAI {
    public class ProgramDriver {

        public void DebugRunProgram() {
            RacetrackParcer parcer = new RacetrackParcer();
            ValueIteration vi = new ValueIteration();
            
            // track instantiation
            Racetrack debug_track = parcer.Parse("Debug-Track");
            //Console.WriteLine(debug_track);
            
            // run algorithms
            UtilityFunction uf = vi.RunValueIteration(debug_track);
            Policy optimal_policy = new Policy(uf, debug_track);
            optimal_policy.Run();

        }
        
        public void RunProgram() {
            RacetrackParcer parcer = new RacetrackParcer();
            ValueIteration vi = new ValueIteration();

            // track instantiation
            Racetrack l_track = parcer.Parse("L-Track");
            Racetrack r_track = parcer.Parse("R-Track");
            Racetrack o_track = parcer.Parse("O-Track"); 
            
            // run algorithms

            vi.RunValueIteration(l_track);
            vi.RunValueIteration(r_track);
            vi.RunValueIteration(o_track);
            
            // printint out track with car test
            Console.WriteLine(l_track);
        }
    }
}
