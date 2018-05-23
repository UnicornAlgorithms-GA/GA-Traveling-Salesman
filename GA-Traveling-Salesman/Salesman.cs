using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Numerics;
using GATravelingSalesman;
using GATravelingSalesman.GA;
using GATravelingSalesman.Utils;
using GATravelingSalesman.Utils.TSGraph;
using GeneticLib.Generations;
using GeneticLib.GeneticManager;
using GeneticLib.Genome;
using GeneticLib.GenomeFactory;
using GeneticLib.GenomeFactory.GenomeProducer;
using GeneticLib.GenomeFactory.GenomeProducer.Breeding;
using GeneticLib.GenomeFactory.GenomeProducer.Breeding.Selection;
using GeneticLib.GenomeFactory.GenomeProducer.Reinsertion;
using GeneticLib.GenomeFactory.Mutation;
using GeneticLib.Randomness;
using GeneticLib.Utils.Graph;

namespace GA_Traveling_Salesman
{
	class Salesman
	{
		// The slowmotion is necessary, because at the begninning, there are 
		// more changes.
		public static bool drawGif = true;
		public static bool drawResult = true;

		public static int numberOfLocations = 20;
		public static Vector2 maxPos = new Vector2(9, 9);
        
		public Vector2[] Locations { get; }

		int genomesCount = 50;
		float geneMutationChance = 0.4f;

		float crossoverPart = 0.80f;
		float reinsertionPart = 0.2f;

		GeneticManagerClassic geneticManager;
		public static int maxIterations = 3000;

		static void Main(string[] args)
		{
			GARandomManager.Random = new RandomClassic((int)DateTime.Now.Ticks);
			var salesman = new Salesman(GetLocations());

			var gif = new GifDrawer(
				salesman.Locations,
				() => salesman.GetBestPath(),
				gifSlowMotionGradient: 1.02f,
				gifSlowMotionCount: 0.8f);

			var fitnessGrph = new GraphDataCollector();

			for (var i = 1; i < maxIterations; i++)
			{
				salesman.Evolve();

				// Debugging stuff...
				var bestDist = salesman.CurrentBestDist();
				Console.WriteLine(String.Format(
						"{0}) dist = {1}", i, bestDist));
				fitnessGrph.Tick(i, bestDist);

				if (drawGif)
					gif.Tick(i);
			}

			if (drawGif)
				gif.ShowGif(maxPos);

			if (drawResult)
			{
				TSGraph.DrawGrapghResult(
					maxPos.X, maxPos.Y,
					salesman.Locations, salesman.GetBestPath());

				fitnessGrph.Draw("Distance", "b-");
			}
		}

		#region Static Utils
        // Here are choosen the locations.
		private static Vector2[] GetLocations()
		{
			var locGenerator = new LocationGenerator(numberOfLocations, maxPos);

            //var locations = locGenerator.CircularLocations();
			//var locations = locGenerator.TwoCirclesLocations(1f / 15);
			var locations = locGenerator.RandomLocations();

			return locations;
		}
		#endregion

		public Salesman(Vector2[] locations)
		{
			Locations = locations;

			var initialGenerationGenerator = new TSInitGenerationGenerator(Locations);

			var selection = new EliteSelection();
			var crossover = new TSCrossover();
			var breeding = new BreedingClassic(
				crossoverPart,
				0,
				selection,
				crossover,
				InitMutations()
			);

			var reinsertion = new EliteReinsertion(reinsertionPart, 0);
			var producers = new IGenomeProducer[]
			{
				reinsertion,
				breeding
			};

			var genomeForge = new GenomeForge(producers);
			var generationManager = new GenerationManagerKeepLast();

			geneticManager = new GeneticManagerClassic(
				generationManager,
				initialGenerationGenerator,
				genomeForge,
				genomesCount
			);
			geneticManager.Init();
		}

        #region Genetics
		public void Evolve()
		{
			var genomes = geneticManager.GenerationManager
										.CurrentGeneration
										.Genomes;

			foreach (var genome in genomes)
				genome.Fitness = -ComputeDist(genome);

			var orderedGenomes = genomes.OrderByDescending(g => g.Fitness)
										.ToArray();

			geneticManager.GenerationManager
						  .CurrentGeneration
						  .Genomes = orderedGenomes;

			geneticManager.Evolve();
		}

		private MutationManager InitMutations()
		{
			var mutation = new TSSwapMutation();
			var result = new MutationManager();
			result.MutationEntries.Add(new MutationEntry(
				mutation,
				geneMutationChance,
				EMutationType.Independent
			));

			return result;
		}
        #endregion

		#region Utilities
		private float ComputeDist(IGenome genome)
        {
            var dist = 0f;

            var genes = genome.Genes;
            var lastPoint = Locations[(genes.Last().Value as ClonableIndex).index];

            foreach (var gene in genes)
            {
                var pos = Locations[(gene.Value as ClonableIndex).index];
                dist += Vector2.Distance(
                    pos,
                    lastPoint);
                lastPoint = pos;
            }
            return dist;
        }

		public IGenome GetBestGenome()
		{
			return geneticManager.GenerationManager
								 .CurrentGeneration
								 .Genomes
								 .OrderBy(ComputeDist)
								 .First();
		}

		public float CurrentBestDist()
		{
			return ComputeDist(GetBestGenome());
		}

		public int[] GetBestPath()
		{
			return GetBestGenome().Genes
								  .Select(g => g.Value as ClonableIndex)
								  .Select(v2 => v2.index)
								  .ToArray();

		}
		#endregion
	}
}
