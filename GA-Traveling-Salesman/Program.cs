using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using GATravelingSalesman.GA;
using GeneticLib.Generations;
using GeneticLib.GenomeFactory.GenomeProducer.Breeding.Selection;
using GeneticLib.Randomness;

namespace GA_Traveling_Salesman
{
    class Program
    {
		public int numberOfLocations = 10;
		public Vector2 minPos = new Vector2(0, 0);
		public Vector2 maxPos = new Vector2(100, 100);

		public Vector2[] Locations { get; }


        static void Main(string[] args)
        {

        }

		public Program()
		{
			GARandomManager.Random = new RandomClassic(0);

			Locations = GenerateRandomLocations(
				numberOfLocations,
				minPos,
			    maxPos);

			var initialGenerationGenerator = new TSInitGenerationGenerator(Locations);

			var selection = new EliteSelection();
            //var crossover = new 

            var generationManager = new GenerationManagerKeepLast();

		}

		private static Vector2[] GenerateRandomLocations(
			int n,
			Vector2 minPos,
			Vector2 maxPos)
		{
			var rnd = GARandomManager.Random;
			return Enumerable.Range(0, n)
					  .Select(i => new Vector2(
				          (float)rnd.NextDouble(minPos.X, maxPos.X),
				          (float)rnd.NextDouble(minPos.Y, maxPos.Y)))
			          .ToArray();
		}
    }
}
