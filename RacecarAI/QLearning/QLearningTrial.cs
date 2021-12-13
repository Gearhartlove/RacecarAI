using System;
using System.Collections.Generic;

namespace RacecarAI {
	public class QLearningTrial {
		Simulator simulator = new Simulator();
		
		private QTable qTable = new QTable();
		private Dictionary<StateActionPair, int> frequency = new Dictionary<StateActionPair, int>();
		private const float learningRate = 0.1f;
		
		public void run() {
			simulator.resetSteps();
			
			Tuple<Racecar, SimulationResult> result;
//			do {
//				result = simulator.simulateStep()
//			} while (result.Item2 != SimulationResult.CRASH);
		}
		
		private TrialAction calculatePolicy(Racecar racecar) {
			double accelNorthDen = Math.Pow(Math.E, qTable.getValue(racecar, TrialAction.ACCEL_NORTH));
			double accelNorthWestDen = Math.Pow(Math.E, qTable.getValue(racecar, TrialAction.ACCEL_NORTHWEST));
			double accelWestDen = Math.Pow(Math.E, qTable.getValue(racecar, TrialAction.ACCEL_WEST));
			double accelSouthWestDen = Math.Pow(Math.E, qTable.getValue(racecar, TrialAction.ACCEL_SOUTHWEST));
			double accelSouthDen = Math.Pow(Math.E, qTable.getValue(racecar, TrialAction.ACCEL_SOUTH));
			double accelSouthEastDen = Math.Pow(Math.E, qTable.getValue(racecar, TrialAction.ACCEL_SOUTHEAST));
			double accelEastDen = Math.Pow(Math.E, qTable.getValue(racecar, TrialAction.ACCEL_EAST));
			double accelNorthEastDen = Math.Pow(Math.E, qTable.getValue(racecar, TrialAction.ACCEL_NORTHEAST));
			double noAccelDen = Math.Pow(Math.E, qTable.getValue(racecar, TrialAction.NO_ACCEL));

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
			foreach (TrialAction action in Enum.GetValues(typeof(TrialAction))) {
				roller.add(action, calculateActionProbability(racecar, action, denominator));
			}

			return roller.roll();
		}

		private double calculateActionProbability(Racecar racecar, TrialAction action, double denominator) {
			return Math.Pow(Math.E, qTable.getValue(racecar, action)) / denominator;
		}
		
		//private Tuple<int, int> getValueFromAction()
	}

	public class QTable {
		private Dictionary<StateActionPair, float> table = new Dictionary<StateActionPair, float>();

		public float getValue(StateActionPair pair) {
			if (table.ContainsKey(pair)) return table[pair];
			return 0;
		}

		public float getValue(Racecar racecar, TrialAction action) {
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

		public override bool Equals(object obj) {
			if (obj is StateActionPair) {
				var other = (StateActionPair) obj;
				return other.State == state && other.Action == action;
			}
			
			return base.Equals(obj);
		}

		public override string ToString() {
			return "(" + state + ", " + Action + ")";
		}
	}
}