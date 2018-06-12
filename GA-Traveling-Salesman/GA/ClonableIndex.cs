using System;
using System.Numerics;
using GeneticLib.Utils;

namespace GATravelingSalesman.GA
{
	public class ClonableIndex : IDeepClonable<ClonableIndex>
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

		public ClonableIndex Clone()
		{
			return new ClonableIndex(index);
		}
	}
}
