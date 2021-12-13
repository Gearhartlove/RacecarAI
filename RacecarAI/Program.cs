using System;
using System.Collections.Generic;

namespace RacecarAI {
    class Program {
        static void Main(string[] args) {
            //TODO Make a Logger class for better Debuging
            //
            
            QLearningAgent agent = new QLearningAgent();
            Racetrack racetrack = new RacetrackParcer().Parse("Debug-Track");
            
            agent.run(100000, racetrack);

            

//            Racecar racecar1 = new Racecar(0, 0, 0, 0, 0, 0);
//            Racecar racecar2 = new Racecar(0,0,0,0,0,0);
//            
//            StateActionPair pair1 = new StateActionPair(racecar1, TrialAction.NO_ACCEL);
//            StateActionPair pair2 = new StateActionPair(racecar2, TrialAction.NO_ACCEL);
//            
//            var test = new Dictionary<StateActionPair, string>();
//            test[pair1] = "test";
//            
//            Console.WriteLine(pair1 == pair2);
//            Console.WriteLine(racecar1.GetHashCode() == racecar2.GetHashCode());
//            Console.WriteLine(test[pair2]);
        }
    }
}