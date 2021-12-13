using System;
using System.Collections.Generic;

namespace RacecarAI {
	public class Util {

		public static Random rand = new Random();
		
		public static int rollRange(int percentFailure) {
			float roll = rand.Next(0, 100);
			if (roll <= percentFailure) return 1;
			return 0;
		}
		
		public static int Clamp(int value, int min, int max) {
			if (value < min) return min;
			if (value > max) return max;
			return value;
		}
		
	}

	public class ProbabilityTable<T> {
		private List<double> probabilities = new List<double>();
		private List<T> data = new List<T>();
		private double sum = 0;

		public T[] Data => data.ToArray();

		public void add(T value, double probability) {
			probabilities.Add(probability);
			data.Add(value);
			sum += probability;
		}

		public T roll() {
			var value = sum * Util.rand.NextDouble();
			for (int i = 0; i < data.Count; i++) {
				if (probabilities[i] < value) {
					value -= probabilities[i];
					continue;
				}

				return data[i];
			}
			
			throw new Exception();
		}
	}

	public class ArgMax<T> where T : IComparable {
		private T greatest;

		public T Greatest => greatest;

		public void insert(T value) {
			if (greatest == null || value.CompareTo(greatest) > 0) {
				greatest = value;
				return;
			}
		}

		public void reset() {
			greatest = default(T);
		}
	}
}