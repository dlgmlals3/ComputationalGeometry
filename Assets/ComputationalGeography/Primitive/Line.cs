using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Computation.geograpy
{
    public class Line<T>
    {
        public Line(T p1, T p2)
		{
            dir = (dynamic)p2 - p1;
            point = p1;
		}

        T GetPoint() { return point; }
        T GetDirection() { return dir; }
        private T point;
        private T dir;
    }

}
