using System;
using System.Collections.Generic;
using System.Numerics;
using GA_Traveling_Salesman;
using GATravelingSalesman.Utils.TSGraph;

namespace GATravelingSalesman.Utils
{
	/// <summary>
	/// The gif won't work if there are more than 20 locations.
    /// </summary>
    public class GifDrawer
    {
		private List<string> frames = new List<string>();
		private float slowMotionGradient = 1.05f;
		private float slowMotionCount = 1f;
		private Vector2[] locations;
		private Func<int[]> getBestPathFunc;

		public GifDrawer(
			Vector2[] locations,
			Func<int[]> getBestPathFunc,
			float gifSlowMotionGradient,
			float gifSlowMotionCount = 1f)
        {
			this.locations = locations;
			this.getBestPathFunc = getBestPathFunc;
			slowMotionGradient = gifSlowMotionGradient;
			slowMotionCount = gifSlowMotionCount;
        }

		public void Tick(int iteration)
		{
			var counter = (int)slowMotionCount;
			counter = counter == 0 ? 1 : counter;

			if (iteration % counter == 0)
            {
                slowMotionCount *= slowMotionGradient;

                frames.Add(
					TSGraph.TSGraph.LocationsToJson(
                        locations,
						getBestPathFunc()
                    ));
            }
		}

		public void ShowGif(Vector2 maxPos)
		{
			TSGraph.TSGraph.DrawGifResult(
				maxPos.X,
                maxPos.Y,
                frames.ToArray());
		}
    }
}
