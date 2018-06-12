using System.Collections.Generic;
using System.Linq;
using GeneticLib.Genome;
using GeneticLib.Genome.Genes;
using GeneticLib.GenomeFactory.GenomeProducer.Breeding.Crossover;
using GeneticLib.Randomness;

namespace GATravelingSalesman.GA
{
	/// <summary>
	/// The crossover is from here:
	/// http://www.theprojectspot.com/tutorial-post/applying-a-genetic-algorithm-to-the-travelling-salesman-problem/5
	/// </summary>
	public class TSCrossover : CrossoverBase
	{
		public static HashSet<int> indexes = new HashSet<int>();

		public override int NbOfChildren => 1;

		public TSCrossover() : base(2)
		{
		}      

		protected override IList<IGenome> PerformCross(IList<IGenome> parents)
		{
			var genesLen = parents[0].Genes.Length;

			var rnd = GARandomManager.Random;
			var index1 = rnd.Next(0, genesLen);
			var len = rnd.Next(1, genesLen - index1);

			var targetParent = rnd.NextDouble() > 0.5d ? parents[0] : parents[1];
			var otherParent = parents[0] == targetParent ? parents[1] : parents[0];

			var selectedGenes = targetParent.Genes
			                                .Skip(index1)
			                                .Take(len)
			                                .ToArray();

			foreach (var gene in selectedGenes)
				indexes.Add((gene.Value as ClonableIndex).index);

			var offspringGenes = new Gene[genesLen];
			var j = 0;
			for (var i = 0; i < genesLen; i++)
			{
				Gene resultingGene;

				if (i >= index1 && i <= index1 + len - 1)
					resultingGene = targetParent.Genes[i];
				else
				{
					while (ContainsGene(selectedGenes, otherParent.Genes[j]))
						j++;
					resultingGene = otherParent.Genes[j];
					j++;
				}

				offspringGenes[i] = new Gene(resultingGene);
			}

			return new IGenome[]
			{
				new GenomeBase { Genes = offspringGenes }
			};
		}

		private bool GenesMatch(Gene gene1, Gene gene2)
		{
			return (gene1.Value as ClonableIndex).index ==
                   (gene2.Value as ClonableIndex).index;
		}

		private bool ContainsGene(IList<Gene> genes, Gene target)
		{
			var element = genes.FirstOrDefault(g => GenesMatch(g, target));
			return element != null && element.Value != null;
		}
	}
}
