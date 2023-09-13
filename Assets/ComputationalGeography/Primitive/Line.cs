using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Microsoft.CSharp;

namespace Computation.geograpy
{
    using Point3d = Vector3f;
    using Point2d = Vector2f;

    public class Line3d
    {
        public Line3d(Vector3f p1, Vector3f p2)
        {
            dir = p2 - p1;
            point = p1;
        }

        public Vector3f GetPoint() { return point; }
        public Vector3f GetDirection() { return dir; }
        private Vector3f point;
        private Vector3f dir;
    };

    public class Line2d
    {
        public Line2d(Point2d p1, Point2d p2)
        {
            dir = p2 - p1;
            dir.Normalize();
            startPoint = p1;
            endPoint = p2;
        }

        public Vector2f GetStartPoint() => startPoint;
        public Vector2f GetEndPoint() => endPoint;
        public Vector2f GetDirection() => dir;

        private Vector2f startPoint;
        private Vector2f endPoint;
        private Vector2f dir;
    }

   

   
}
