using System;
using System.Collections.Generic;
using System.Linq;
using GATravelingSalesman.Utils;
using GeneticLib.Genome;
using GeneticLib.GenomeFactory.Mutation;
using GeneticLib.Randomness;

namespace GATravelingSalesman.GA
{
	/// <summary>
    /// Swap 2 genes within the same genome.
    /// </summary>
	public class TSSwapMutation : MutationBase
    {
		public static HashSet<int> indexes = new HashSet<int>();

        /// <summary>
		/// Guarantees that 2 different elements are chosen.
		/// A little optimization is involved here.
        /// </summary>
		protected override void DoMutation(IGenome genome)
		{
			var count = genome.Genes.Count();
			int index1 = GARandomManager.Random.Next(0, count);
			int index2;

			if (count > 10)
			{
				do
				{
					index2 = GARandomManager.Random.Next(0, count);
				} while (index1 == index2);
			}
			else
			{
				var elements = Enumerable.Range(0, count)
				                         .ToList();
				elements.Remove(index1);
				index2 = elements.RandomChoice();
			}

			indexes.Add(index1);
			indexes.Add(index2);

			// At this point, both indexes are chosen.

			var aux = genome.Genes[index1];
			genome.Genes[index1] = new Gene(genome.Genes[index2]);
			genome.Genes[index2] = new Gene(aux);
		}
	}
}
