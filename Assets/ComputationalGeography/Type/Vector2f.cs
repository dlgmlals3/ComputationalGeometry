using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Numerics;
using UnityEngine;

namespace Computation.geograpy
{
    /// <summary>
    /// Generic class for arithmetic operator overloading demonstration
    /// </summary>
    /// <typeparam name="T"></typeparam>
    class Vector2f : Vector
    {

        public Vector2f(float x, float y)
        {
            this.values = new float[2];
            this.values[0] = x;
            this.values[1] = y;
        }

        public float x
        {
            get => values[0];
            set => values[0] = value;
        }
        public float y
        {
            get => values[1];
            set => values[1] = value;
        }

		public float this[int key]
		{
            get => values[key];
            set => values[key] = value;
		}
	
        public static Vector2f operator +(Vector2f a, Vector2f b)
        {
            var c = new Vector2f(0, 0);
            c.x = a.x + b.x;
            c.y = a.y + b.y;
            return c;
        }

        public static Vector2f operator -(Vector2f a, Vector2f b)
        {
            var c = new Vector2f(0, 0);
            c.x = a.x - b.x;
            c.y = a.y - b.y;
            return c;
		}

        public static float CrossProduct2D(Vector2f v1, Vector2f v2)
		{
            return (v1.x * v2.y) - (v1.y * v2.x);
		}

        /// <summary>
        /// Some kind of conversion of Vector into string
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return string.Join(",", values);
        }
    }

}