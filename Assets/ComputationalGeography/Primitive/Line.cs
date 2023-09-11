using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Microsoft.CSharp;

namespace Computation.geograpy
{
    public class Line
    {
        public Line(Vector3f p1, Vector3f p2)
		{
            dir = p2 - p1;
            point = p1;
		}

        Vector3f GetPoint() { return point; }
        Vector3f GetDirection() { return dir; }
        private Vector3f point;
        private Vector3f dir;
    }
}
