using System;
using System.Numerics;

namespace GATravelingSalesman.GA
{
	public class ClonableV2 : ICloneable
	{
		public float x;
		public float y;

		public ClonableV2(float x, float y)
		{
			this.x = x;
			this.y = y;
		}

		public ClonableV2(Vector2 vector)
		{
			this.x = vector.X;
			this.y = vector.Y;
		}

		public ClonableV2(ClonableV2 other)
		{
			this.x = other.x;
			this.y = other.y;
		}

		public object Clone()
		{
			return new ClonableV2(x, y);
		}
	}
}
