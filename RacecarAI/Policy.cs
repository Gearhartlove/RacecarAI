using System;

namespace RacecarAI {
    
    /// <summary>
    /// Class which runs the UtilityFunction created from the Value Iteration Algorithm.
    /// </summary>
    public class Policy {
        private UtilityFunction uf;
        private Racetrack rt;
        private Random rand;
        
        public Policy(UtilityFunction uf, Racetrack rt) {
            this.uf = uf;
            this.rt = rt;
            rand = new Random();
        }

        /// <summary>
        /// Randomly selects the start spot for the car to take on the track
        /// </summary>
        /// <returns></returns>
        public Tuple<int, int> ChooseStartSpot() {
            int rand_spot = rand.Next(rt.GetStartSpots.Count);
            return new Tuple<int, int>(rt.GetStartSpots[rand_spot][0], rt.GetStartSpots[rand_spot][1]);
        }

        /// <summary>
        /// Runs the desired policy created from the UtilityFunction in the ValueIteration algorithm.
        /// </summary>
        public void Run() {
            Console.WriteLine(rt.getDisplay(rt.GetRacecar));
            rt.GetRacecar.Spawn(ChooseStartSpot());
            // set race car position to start
            while (rt[rt.GetRacecar.GetXPos(),rt.GetRacecar.GetYPos()] != TrackComponent.Finish || 
                   rt[rt.GetRacecar.GetXPos(),rt.GetRacecar.GetYPos()] != TrackComponent.Wall) { 
                
                Console.WriteLine(rt.getDisplay(rt.GetRacecar));
                break;
            }
            
        }
        
        
    }
}