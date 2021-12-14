using System;
using System.Collections.Generic;

namespace RacecarAI {
	public class Util {

		public static Random rand = new Random();
		
		/// <summary>
		/// This give back a 1 or 0 depending on the probability given.
		/// </summary>
		/// <param name="percentFailure"></param>
		/// <returns></returns>
		public static int rollRange(int percentFailure) {
			float roll = rand.Next(0, 100);
			if (roll >= percentFailure) return 1;
			return 0;
		}
		
		/// <summary>
		/// This limits a number between the min and max values.
		/// </summary>
		/// <param name="value"></param>
		/// <param name="min"></param>
		/// <param name="max"></param>
		/// <returns></returns>
		public static int Clamp(int value, int min, int max) {
			if (value < min) return min;
			if (value > max) return max;
			return value;
		}
		
	}

	
	/// <summary>
	/// This is a class that can take in values with an associated weight and then later roll to select on of the items
	/// at random.
	/// </summary>
	/// <typeparam name="T"></typeparam>
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

	/// <summary>
	/// This is a class that will keep track of the largest value passed to it.
	/// </summary>
	/// <typeparam name="T"></typeparam>
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