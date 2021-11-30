using System;

namespace RacecarAI {
    class Program {
        static void Main(string[] args) {
            Console.WriteLine("Hello World!");
            Racecar testState = new Racecar();
            testState.TimeStep(1, 1);
            testState.TimeStep(1, -1);
        }
    }
}