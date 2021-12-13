using System;

namespace RacecarAI {
	
	public class Simulator {

		private int steps = 0;
		
		public Tuple<Racecar, SimulationResult> simulateStep(Racecar car, Racetrack track, int accelDeltaX, int accelDeltaY) {
			steps++;
			
			var newAccelX = car.GetXAccel() + accelDeltaX;
			var newAccelY = car.GetYAccel() + accelDeltaY;
			
			var ignoreAccel = Util.rollRange(20);

			var newSpeedX = car.GetXVel() + newAccelX * ignoreAccel;
			var newSpeedY = car.GetYVel() + newAccelY * ignoreAccel;

			var newPosX = car.GetXPos() + newSpeedX;
			var newPosY = car.GetYPos() + newSpeedY;
			
			var nextLocation = track[newPosX, newPosY];

			var newRacecar = new Racecar(newPosX, newPosY, newSpeedX, newSpeedY, newAccelX, newAccelY);
			
			switch (nextLocation) {
				case TrackComponent.Finish:
					return new Tuple<Racecar, SimulationResult>(newRacecar, SimulationResult.FINISH);
				case TrackComponent.Wall:
					return new Tuple<Racecar, SimulationResult>(newRacecar, SimulationResult.CRASH);
				case TrackComponent.Start:
				case TrackComponent.Track:
					return new Tuple<Racecar, SimulationResult>(newRacecar, SimulationResult.CONTINUE);
			}

			throw new Exception("Racetrack value should not be set to initial");
		}

		public int getStepsTaken() {
			return steps;
		}

		public void resetSteps() {
			steps = 0;
		}
		
	}

	public enum SimulationResult {
		CRASH,
		CONTINUE,
		FINISH
	}
}