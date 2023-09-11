using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Computation.geograpy
{
    public static class Extension
    {
        private static readonly float TOLERANCE = 0.000001f;
        public static bool IsEqual(this float x, float y)
        {
            return Math.Abs(x - y) < TOLERANCE;
        }

        public enum RelativePosition
        {
            Left, Right, Behind, Beyond, Beetween, origin, destination
        }

        public static bool _xor(bool x, bool y)
		{
            return x ^ y;
		}
    }
}
