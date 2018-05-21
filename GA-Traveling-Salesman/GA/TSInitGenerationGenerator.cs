using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using GATravelingSalesman.Utils;
using GeneticLib.Generations.InitialGeneration;
using GeneticLib.Genome;

namespace GATravelingSalesman.GA
{
	public class TSInitGenerationGenerator : InitialGenerationCreatorBase
    {
		private IList<Vector2> Locations;

		public TSInitGenerationGenerator(IList<Vector2> locations)
        {
			Locations = locations.ToArray();
        }

		protected override IGenome NewRandomGenome()
		{
			Locations.Shuffle();
			var genes = Locations.Select(l => new Gene(new ClonableV2(l)))
								 .ToArray();
			
			var genome = new V2Genome
			{
				Genes = genes
		    };
            
			return genome;
		}
	}
}
