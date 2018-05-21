using System;
using System.Collections.Generic;
using System.Linq;
using GeneticLib.Randomness;

namespace GATravelingSalesman.Utils
{
	public static class Extensions
    {
		public static IEnumerable<T> Shuffle<T>(this IEnumerable<T> source)
        {
			var rnd = GARandomManager.Random;
            return source.OrderBy<T, int>((item) => rnd.Next());
        }

		public static T RandomChoice<T>(this IEnumerable<T> source)
        {
            var rnd = GARandomManager.Random;
			return source.ElementAt(rnd.Next(0, source.Count()));
        }

		//public static double Range(this Random, )
    }
}
