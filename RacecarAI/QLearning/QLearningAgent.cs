using System;
using System.Collections.Generic;

namespace RacecarAI {
	public class QLearningAgent {
		Simulator simulator = new Simulator();
		
		private QTable qTable = new QTable();
		private Dictionary<StateActionPair, int> frequency = new Dictionary<StateActionPair, int>();
		private const double learningRate = 0.1;
		private const double discountRate = 0.1;
		private const double T = 1.2;

		public void run(int numberOfTrials, Racetrack racetrack) {
			int numberOfFailures = 0;
			int numberOfSuccesses = 0;

			var state = racetrack.rollRandomStartCar();
			
			for (int i = 0; i < numberOfTrials; i++) {
				Console.WriteLine("Iteration " + i + " Start!");
				var result = runOneTrial(racetrack, state);

				switch (result) {
					case SimulationResult.CRASH:
						numberOfFailures++;
						break;
					case SimulationResult.FINISH:
						numberOfSuccesses++;
						break;
				}
				
				Console.WriteLine("Iteration " + i + " ended with result " + result);
				Console.WriteLine();
			}
			
			Console.WriteLine("Number of Successes: " + numberOfSuccesses +" | Number of Failures: " + numberOfFailures);
		}
		
		public SimulationResult runOneTrial(Racetrack racetrack, Racecar start) {
			simulator.resetSteps();
			var lastState = start;

			Tuple<Racecar, SimulationResult> result;
			do {
				Console.WriteLine("  Step " + simulator.getStepsTaken());
				Console.WriteLine("    Last State: " + "{Position: (" + lastState.GetXPos() + ", " + lastState.GetYPos() + "); Velocity:(" + lastState.GetXVel() +", " + lastState.GetYVel() + ")}");
				
				var action = calculatePolicy(lastState);
				Console.WriteLine("    Action Chosen: " + action);
				result = simulator.simulateStep(lastState, racetrack, action);
				var reward = calculateReward(result.Item2);
				var newQValue = qTable.getValue(lastState, action) + learningRate *
				                (reward + (discountRate * maxQValueForState(result.Item1)) - qTable.getValue(lastState, action));
				Console.WriteLine("    New Q value for {Position: (" + lastState.GetXPos() + ", " + lastState.GetYPos() + "); Velocity:(" + lastState.GetXVel() +", " + lastState.GetYVel() + "); " + action + "}");
				Console.WriteLine("    New Value: " + newQValue + " | Old Value: " + qTable.getValue(lastState, action));
				qTable.setValue(lastState, action, newQValue);
				lastState = result.Item1;
			} while (result.Item2 == SimulationResult.CONTINUE);

			//Console.WriteLine("Steps Taken: " + simulator.getStepsTaken());
			//Console.WriteLine(racetrack.getDisplay(lastState));
			//Console.WriteLine();
			
			return result.Item2;
		}
		
		private TrialAction calculatePolicy(Racecar racecar) {
			double accelNorthDen = Math.Pow(Math.E, qTable.getValue(racecar, TrialAction.ACCEL_NORTH)/T);
			double accelNorthWestDen = Math.Pow(Math.E, qTable.getValue(racecar, TrialAction.ACCEL_NORTHWEST)/T);
			double accelWestDen = Math.Pow(Math.E, qTable.getValue(racecar, TrialAction.ACCEL_WEST)/T);
			double accelSouthWestDen = Math.Pow(Math.E, qTable.getValue(racecar, TrialAction.ACCEL_SOUTHWEST)/T);
			double accelSouthDen = Math.Pow(Math.E, qTable.getValue(racecar, TrialAction.ACCEL_SOUTH)/T);
			double accelSouthEastDen = Math.Pow(Math.E, qTable.getValue(racecar, TrialAction.ACCEL_SOUTHEAST)/T);
			double accelEastDen = Math.Pow(Math.E, qTable.getValue(racecar, TrialAction.ACCEL_EAST)/T);
			double accelNorthEastDen = Math.Pow(Math.E, qTable.getValue(racecar, TrialAction.ACCEL_NORTHEAST)/T);
			double noAccelDen = Math.Pow(Math.E, qTable.getValue(racecar, TrialAction.NO_ACCEL)/T);

			double denominator = accelNorthDen +
			                     accelNorthWestDen +
			                     accelWestDen +
			                     accelSouthWestDen +
			                     accelSouthDen +
			                     accelSouthEastDen +
			                     accelEastDen +
			                     accelNorthEastDen +
			                     noAccelDen;

			var roller = new ProbabilityTable<TrialAction>();
			
			Console.WriteLine("    Action Probabilities:");
			foreach (TrialAction action in Enum.GetValues(typeof(TrialAction))) {
				var actionProbability = Math.Pow(Math.E, qTable.getValue(racecar, action)/T) / denominator;
				Console.WriteLine("      (" + action + ", " + actionProbability + ")");
				roller.add(action, actionProbability);
			}

			return roller.roll();
		}

		private double calculateReward(SimulationResult result) {
			switch (result) {
				case SimulationResult.CRASH:
					return -1;
				case SimulationResult.CONTINUE:
					return -0.04;
				case SimulationResult.FINISH:
					return 1;
			}

			throw new Exception("Unexpected result.");
		}

		private double maxQValueForState(Racecar racecar) {
			var max = new ArgMax<double>();
			
			foreach (TrialAction action in Enum.GetValues(typeof(TrialAction))) {
				max.insert(qTable.getValue(racecar, action));
			}

			return max.Greatest;
		}
		
		//private Tuple<int, int> getValueFromAction()
	}

	public class QTable {
		private Dictionary<StateActionPair, double> table = new Dictionary<StateActionPair, double>();

		public void setValue(StateActionPair pair, double value) {
			table[pair] = value;
		}

		public void setValue(Racecar racecar, TrialAction action, double value) {
			setValue(new StateActionPair(racecar, action), value);
		}
		
		public double getValue(StateActionPair pair) {
			if (table.ContainsKey(pair)) return table[pair];
			return 0;
		}

		public double getValue(Racecar racecar, TrialAction action) {
			return getValue(new StateActionPair(racecar, action));
		}
	}
	
	public enum TrialAction {
		ACCEL_NORTH,
		ACCEL_NORTHEAST,
		ACCEL_EAST,
		ACCEL_SOUTHEAST,
		ACCEL_SOUTH,
		ACCEL_SOUTHWEST,
		ACCEL_WEST,
		ACCEL_NORTHWEST,
		NO_ACCEL
	}

	public class StateActionPair {
		private Racecar state;
		private TrialAction action;

		public Racecar State => state;
		public TrialAction Action => action;


		public StateActionPair(Racecar state, TrialAction action) {
			this.state = state;
			this.action = action;
		}

		public static bool operator ==(StateActionPair sap1, StateActionPair sap2) {
			return sap1.Equals(sap2);
		}
        
		public static bool operator !=(StateActionPair sap1, StateActionPair sap2) {
			return !sap1.Equals(sap2);
		}
		
		public override bool Equals(object obj) {
			if (obj is StateActionPair) {
				var other = (StateActionPair) obj;
				return other.State == state && other.Action == action;
			}
			
			return base.Equals(obj);
		}

		public override int GetHashCode() {
			return state.GetHashCode() ^ action.GetHashCode();
		}

		public override string ToString() {
			return "(" + state + ", " + Action + ")";
		}
	}
}