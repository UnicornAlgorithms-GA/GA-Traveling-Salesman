using System;
using System.Numerics;

namespace GATravelingSalesman.GA
{
	public class ClonableV2 : ICloneable
	{
		public int index;     

		public ClonableV2(int index)
		{
			this.index = index;
		}

		public ClonableV2(ClonableV2 other)
		{
			this.index = other.index;
		}

		public object Clone()
		{
			return new ClonableV2(index);
		}
	}
}
