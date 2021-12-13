using System;

namespace RacecarAI {
    class Program {
        static void Main(string[] args) {
            var argMax = new ArgMax<int>();
            
            argMax.insert(10);
            argMax.insert(12);
            argMax.insert(13);
            argMax.insert(11);
            argMax.insert(9);
            
            Console.WriteLine(argMax.Greatest);
        }
    }
}