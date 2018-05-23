using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Numerics;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace GATravelingSalesman.Utils.TSGraph
{
    public class TSGraph
    {      
		static readonly string pyFile = "./Utils/TSGraph/graph.py";

		public static string LocationsToJson(
			Vector2[] locations,
			int[] path)
		{
			var obj = new
			{
				points = path.Select(p => new
				{
					x = locations[p].X,
					y = locations[p].Y,
                    i = p
				})
			};
			return JsonConvert.SerializeObject(obj).Replace("\"", "\\\"");
		}

		public static void DrawGifResult(
            float maxX,
            float maxY,
			string[] frames)
		{
			var argv = "" + maxX + " " + maxY + " gif " + String.Join(" ", frames);
			RunPyGraph(pyFile, argv);
		}

		public static void DrawGrapghResult(
			float maxX,
			float maxY,
			Vector2[] locations,
		    int[] bestPath)
		{         
			var result = LocationsToJson(locations, bestPath);  
			RunPyGraph(pyFile, "" + maxX + " " + maxY + " single " + result);
		}

		public static void RunPyGraph(string filePath, string args)
        {
            using (var p = new Process())
            {
                var info = new ProcessStartInfo("python3");
				info.Arguments = filePath + " " + args;
                info.RedirectStandardInput = false;
                info.RedirectStandardError = false;
                info.RedirectStandardOutput = false;
                info.UseShellExecute = false;

                p.StartInfo = info;
                p.Start();
            }
        }
    }
}
