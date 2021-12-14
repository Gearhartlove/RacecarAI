using System;

namespace RacecarAI {
	
	public class Simulator {

		private int steps = 0;
		
		/// <summary>
		/// This will simulate one step on the race track. It takes a state, an action, and the track. Then it will
		/// return what happened. With this simulator, there is a %20 chance that the action wont be taken, and the
		/// acceleration wont be changed.
		/// </summary>
		/// <param name="car"></param>
		/// <param name="track"></param>
		/// <param name="accelDeltaX"></param>
		/// <param name="accelDeltaY"></param>
		/// <returns></returns>
		/// <exception cref="Exception"></exception>
		public Tuple<Racecar, SimulationResult> simulateStep(Racecar car, Racetrack track, int accelDeltaX, int accelDeltaY) {
			steps++;
			
			var newAccelX = accelDeltaX;
			var newAccelY = accelDeltaY;
			
			var ignoreAccel = Util.rollRange(20);

			var newSpeedX = car.GetXVel() + newAccelX * ignoreAccel;
			var newSpeedY = car.GetYVel() + newAccelY * ignoreAccel;

			var newPosX = Util.Clamp(car.GetXPos() + newSpeedX, 0, track.XTracksize-1);
			var newPosY = Util.Clamp(car.GetYPos() + newSpeedY, 0 , track.YTracksize-1);
			
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

		
		/// <summary>
		/// Overload of the function above.
		/// </summary>
		/// <param name="car"></param>
		/// <param name="track"></param>
		/// <param name="action"></param>
		/// <returns></returns>
		/// <exception cref="Exception"></exception>
		public Tuple<Racecar, SimulationResult> simulateStep(Racecar car, Racetrack track, TrialAction action) {
			switch (action) {
				case TrialAction.ACCEL_NORTH:
					return simulateStep(car, track, 0, -1);
				case TrialAction.ACCEL_NORTHEAST:
					return simulateStep(car, track, 1, -1);
				case TrialAction.ACCEL_EAST:
					return simulateStep(car, track, 1, 0);
				case TrialAction.ACCEL_SOUTHEAST:
					return simulateStep(car, track, 1, 1);
				case TrialAction.ACCEL_SOUTH:
					return simulateStep(car, track, 0, 1);
				case TrialAction.ACCEL_SOUTHWEST:
					return simulateStep(car, track, -1, 1);
				case TrialAction.ACCEL_WEST:
					return simulateStep(car, track, -1, 0);
				case TrialAction.ACCEL_NORTHWEST:
					return simulateStep(car, track, -1, -1);
				case TrialAction.NO_ACCEL:
					return simulateStep(car, track, 0, 0);
			}
			
			throw new Exception("Not a recognized Action.");
		}

		/// <summary>
		/// This returns how many steps have been taken since that last reset or start of the simulators life.
		/// </summary>
		/// <returns></returns>
		public int getStepsTaken() {
			return steps;
		}

		/// <summary>
		/// Resets the number of states taken so the simulator can start a new trial
		/// </summary>
		public void resetSteps() {
			steps = 0;
		}
		
	}

	/// <summary>
	/// These are the outcomes of the simulation.
	/// </summary>
	public enum SimulationResult {
		CRASH,
		CONTINUE,
		FINISH
	}
}