using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Numerics;
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
    class Program
    {
		// The slowmotion is necessary, because at the begninning, there are 
        // more changes.
		public static bool drawGif = true;
		public static float gifSlowMotionGradient = 1.05f;
		public static bool drawResult = true;

		public static int numberOfLocations = 20;
		public static Vector2 maxPos = new Vector2(1, 1);

		public Vector2[] Locations { get; }
        
		int genomesCount = 50;
        float geneMutationChance = 0.4f;

        float crossoverPart = 0.80f;
        float reinsertionPart = 0.2f;

		GeneticManagerClassic geneticManager;
        
		public static bool targetReached = false;
		public static int maxIterations = 5000;

        static void Main(string[] args)
        {
			GARandomManager.Random = new RandomClassic((int)DateTime.Now.Ticks);

			//var salesman = new Program(CircularLocations());
			//var salesman = new Program(TwoCirclesLocations(1f / 40));
			var salesman = new Program(RandomLocations());

			List<string> gifFrames = new List<string>();
			var gifSlowMotionCount = 1f;

			for (var i = 1; i < maxIterations; i++)
			{
				if (targetReached)
					break;

				salesman.Evolve();

                // Debugging stuff...
				Console.WriteLine(String.Format(
                    "{0}) dist = {1}",
                    i,
                    salesman.CurrentBestDist()));

				if (drawGif)
				{
					if (i % (int)gifSlowMotionCount == 0)
					{
						gifSlowMotionCount *= gifSlowMotionGradient;
      
						gifFrames.Add(
							TSGraph.LocationsToJson(
								salesman.Locations,
								salesman.GetBestPath()
							));
					}
				}
			}

			if (drawGif)
			{
				TSGraph.DrawGifResult(
					maxPos.X,
					maxPos.Y,
					gifFrames.ToArray());
			}

			if (drawResult)
			{
				TSGraph.DrawGrapghResult(
					maxPos.X, maxPos.Y,
					salesman.Locations, salesman.GetBestPath());
			}

            // Made sure I didn't mess up the production.
			Debug.Assert(TSSwapMutation.indexes.Count == salesman.Locations.Count());
			Debug.Assert(TSCrossover.indexes.Count == salesman.Locations.Count());
   
        }

		public Program(Vector2[] locations)
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

		private static Vector2[] GenerateRandomLocations(
			int n,
			Vector2 minpos,
			Vector2 maxpos)
		{
			var rnd = GARandomManager.Random;
			return Enumerable.Range(0, n)
					  .Select(i => new Vector2(
				          (float)rnd.NextDouble(minpos.X, maxPos.X),
				          (float)rnd.NextDouble(minpos.Y, maxPos.Y)))
			          .ToArray();
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

		private float ComputeDist(IGenome genome)
		{
			var dist = 0f;

			var genes = genome.Genes;
			var lastPoint = Locations[(genes.Last().Value as ClonableV2).index];

			foreach (var gene in genes)
			{
				var pos = Locations[(gene.Value as ClonableV2).index];
				dist += Vector2.Distance(
					pos,
					lastPoint);
				lastPoint = pos;
			}
			return dist;
		}

		public float CurrentBestDist()
		{
			var best = geneticManager.GenerationManager
			                         .CurrentGeneration
			                         .Genomes
			                         .First();

			return ComputeDist(best);
		}

		public int[] GetBestPath()
		{
			var best = geneticManager.GenerationManager
                                     .CurrentGeneration
                                     .Genomes
			                         .OrderBy(ComputeDist)
                                     .First();
			return best.Genes
					   .Select(g => g.Value as ClonableV2)
					   .Select(v2 => v2.index)
					   .ToArray();

		}

		private static Vector2[] RandomLocations()
		{
			return GenerateRandomLocations(
				numberOfLocations,
				new Vector2(0, 0),
				maxPos);
		}

		private static Vector2[] CircularLocations()
		{
			var locations = new List<Vector2>(numberOfLocations);
			AddCircularLocations(locations, maxPos / 2, numberOfLocations);
			return locations.ToArray();
		}

		private static Vector2[] TwoCirclesLocations(float r2Modif)
        {
			var locations = new List<Vector2>(numberOfLocations);

			var r1 = maxPos / 2f;
			var r2 = maxPos * r2Modif;
			var n1 = numberOfLocations / 2;
			var n2 = numberOfLocations - n1;

			AddCircularLocations(locations, r1, n1);
			AddCircularLocations(locations, r2, n2);
            
			return locations.ToArray();
        }

		private static void AddCircularLocations(
			List<Vector2> locations,
			Vector2 r,
		    int nbOfPoints)
		{
			for (int i = 0; i < nbOfPoints; i++)
			{
				var part = 1f * i / nbOfPoints;
				var x = MathF.Sin(2 * MathF.PI * part) * r.X + maxPos.X / 2;
				var y = MathF.Cos(2 * MathF.PI * part) * r.Y + maxPos.Y / 2;
				locations.Add(new Vector2(x, y));
			}
		}
    }
}
