using System;

namespace RacecarAI {
    class Program {
        static void Main(string[] args) {
            var roller = new ProbabilityTable<string>();
            roller.add("test1", 0.50);
            roller.add("test2", 0.25);
            roller.add("test3", 0.25);

            for (int i = 0; i < 10; i++) {
                Console.WriteLine(roller.roll());
            }
            
            Console.WriteLine(5.CompareTo(4));
        }
    }
}