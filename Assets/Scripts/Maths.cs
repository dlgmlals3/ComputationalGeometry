using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace computation.geograpy
{
	public static class Extension
	{
		public static readonly double TOLERENCE = 0.000001f;
		public static bool IsEqualD(this double x, double y)
		{
			return Math.Abs(x - y) < TOLERENCE;
		}
	}

	
}