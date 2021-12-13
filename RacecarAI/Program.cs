using System;

namespace RacecarAI {
    class Program {
        static void Main(string[] args) {
            QLearningAgent agent = new QLearningAgent();
            Racetrack racetrack = new RacetrackParcer().Parse("Debug-Track");
            
            agent.run(150, racetrack);
        }
    }
}