using System;
using System.Numerics;

namespace GATravelingSalesman.GA
{
	public class ClonableIndex : ICloneable
	{
		public int index;     

		public ClonableIndex(int index)
		{
			this.index = index;
		}

		public ClonableIndex(ClonableIndex other)
		{
			this.index = other.index;
		}

		public object Clone()
		{
			return new ClonableIndex(index);
		}
	}
}
