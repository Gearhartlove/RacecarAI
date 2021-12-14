using System;

namespace RacecarAI {
    public class Policy {
        private UtilityFunction uf;
        private Racetrack rt;
        private Random rand;
        
        public Policy(UtilityFunction uf, Racetrack rt) {
            this.uf = uf;
            this.rt = rt;
            rand = new Random();
        }

        public Tuple<int, int> ChooseStartSpot() {
            int rand_spot = rand.Next(rt.GetStartSpots.Count);
            return new Tuple<int, int>(rt.GetStartSpots[rand_spot][0], rt.GetStartSpots[rand_spot][1]);
        }

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