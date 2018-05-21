using System;
using System.Linq;
using GeneticLib.Genome;

namespace GATravelingSalesman.GA
{
	/// <summary>
	/// Vector2 Genome
	/// </summary>
	public class V2Genome : GenomeBase
	{
		public override object Clone()
		{
			var result = new V2Genome
			{
				Fitness = this.Fitness,
				Genes = this.Genes
							.Select(g => new Gene(g))
							.ToArray()
			};
			return result;
		}

		public override IGenome CreateNew(Gene[] genes)
		{
			var result = new V2Genome
            {
                Genes = genes.Select(g => new Gene(g))
                             .ToArray()
            };
			return result;
		}
	}
}
