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

		/// <summary>
		/// This method runs the Qlearning algorithm over a number of trials.
		/// </summary>
		/// <param name="numberOfTrials"></param>
		/// <param name="racetrack"></param>
		public void run(int numberOfTrials, Racetrack racetrack) {
			int numberOfFailures = 0;
			int numberOfSuccesses = 0;

			var state = racetrack.rollRandomStartCar();
			
			for (int i = 0; i < numberOfTrials; i++) {
				//Console.WriteLine("Iteration " + i + ":");
				var result = runOneTrial(racetrack, state);

				switch (result) {
					case SimulationResult.CRASH:
						numberOfFailures++;
						break;
					case SimulationResult.FINISH:
						numberOfSuccesses++;
						break;
				}

				//Console.WriteLine("Iteration " + i + " ended with result " + result);
				//Console.WriteLine();
			}

			Console.WriteLine("Number of Successes: " + numberOfSuccesses +" | Number of Failures: " + numberOfFailures);
		}
		
		/// <summary>
		/// This method contains the QLearning algorithm itself
		/// </summary>
		/// <param name="racetrack"></param>
		/// <param name="start"></param>
		/// <returns></returns>
		public SimulationResult runOneTrial(Racetrack racetrack, Racecar start) {
			
			//First thing that I do is initialize the state and make sure my simulator is reset.
			simulator.resetSteps();
			var lastState = start;

			Tuple<Racecar, SimulationResult> result;
			do {
				//Then I calculated the action im going to take given the current state.
				var action = calculatePolicy(lastState);
				
				//Then, I simulate what happens when that action is taken.
				result = simulator.simulateStep(lastState, racetrack, action);
				
				//Based on the return of the simulator, I make a reward.
				var reward = calculateReward(result.Item2);
				
				//Then I calculate the new Qvalue for the state action pair and store it.
				var newQValue = qTable.getValue(lastState, action) + learningRate *
				                (reward + (discountRate * maxQValueForState(result.Item1)) - qTable.getValue(lastState, action));
				qTable.setValue(lastState, action, newQValue);
				
				//and set the new state to the old one.
				lastState = result.Item1;
				
				//And I do this until I finsih or crash
			} while (result.Item2 == SimulationResult.CONTINUE);

			//After everything, I return the result of this trial.
			return result.Item2;
		}
		
		/// <summary>
		/// This function chooses the action to take next. It does this by determining the likely hood that the
		/// specific state action pair will increase the QValue. Sometimes, however, It will pick a non-greedy
		/// direction. It does this by putting all of the action possible into a probability table. The actions
		/// that are considered greedy have a higher probability of being selected, however other actions can be
		/// selected if they are lucky enough. This allows for the Qlearner to explore enough but still be effective.
		/// </summary>
		/// <param name="racecar"></param>
		/// <returns></returns>
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
			
			foreach (TrialAction action in Enum.GetValues(typeof(TrialAction))) {
				var actionProbability = Math.Pow(Math.E, qTable.getValue(racecar, action)/T) / denominator;
				roller.add(action, actionProbability);
			}

			return roller.roll();
		}

		/// <summary>
		/// This function calculated the reward based on the return value of the simulator.
		/// </summary>
		/// <param name="result"></param>
		/// <returns></returns>
		/// <exception cref="Exception"></exception>
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

		/// <summary>
		/// This just finds the largest QValue for a given state by iterating over all of the possible actions and
		/// looking up their QValues.
		/// </summary>
		/// <param name="racecar"></param>
		/// <returns></returns>
		private double maxQValueForState(Racecar racecar) {
			var max = new ArgMax<double>();
			
			foreach (TrialAction action in Enum.GetValues(typeof(TrialAction))) {
				max.insert(qTable.getValue(racecar, action));
			}

			return max.Greatest;
		}
	}

	/// <summary>
	/// This is a QTable is a action pair lookup that defaults a return value of 0 if there is no value set.
	/// </summary>
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
	
	/// <summary>
	/// This enum defines what actions can be taken.
	/// </summary>
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

	/// <summary>
	/// This class defines a State Pair action. It stores the state value pair, it also implements method for
	/// equality and hashing, so it can be used as a key in a hash table.
	/// </summary>
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