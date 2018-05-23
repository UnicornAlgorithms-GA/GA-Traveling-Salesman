using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using GeneticLib.Randomness;

namespace GATravelingSalesman
{
    public class LocationGenerator
    {
		private int nbOfLocations;
		private Vector2 maxPos;

		public LocationGenerator(int nbOfLocations, Vector2 maxPos)
        {
			this.nbOfLocations = nbOfLocations;
			this.maxPos = maxPos;
        }

		public Vector2[] GenerateRandomLocations(
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

		public Vector2[] RandomLocations()
        {
			var rnd = GARandomManager.Random;
			return Enumerable.Range(0, nbOfLocations)
                      .Select(i => new Vector2(
                          (float)rnd.NextDouble(0, maxPos.X),
                          (float)rnd.NextDouble(0, maxPos.Y)))
                      .ToArray();
        }

		public Vector2[] CircularLocations()
        {
			var locations = new List<Vector2>(nbOfLocations);
			AddCircularLocations(locations, maxPos / 2, nbOfLocations);
            return locations.ToArray();
        }

		public Vector2[] TwoCirclesLocations(float r2Modif)
        {
			var locations = new List<Vector2>(nbOfLocations);

            var r1 = maxPos / 2f;
            var r2 = maxPos * r2Modif;
			var n1 = nbOfLocations / 2;
			var n2 = nbOfLocations - n1;

            AddCircularLocations(locations, r1, n1);
            AddCircularLocations(locations, r2, n2);

            return locations.ToArray();
        }

        private void AddCircularLocations(
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
