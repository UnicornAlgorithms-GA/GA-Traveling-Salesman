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
		private IList<Vector2> Locations { get; set; }

		public TSInitGenerationGenerator(IList<Vector2> locations)
        {
			Locations = locations;
        }

		protected override IGenome NewRandomGenome()
		{
			var genes = Locations.Shuffle()
			                     .Select(l => new Gene(new ClonableIndex(Locations.IndexOf(l))))
								 .ToArray();
			
			var genome = new V2Genome
			{
				Genes = genes
		    };
            
			return genome;
		}
	}
}
