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
    class Vector2f
    {
        private float[] values;

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

		public static bool operator ==(Vector2f a, Vector2f b)
		{
            if (a.values.Length != b.values.Length) return false;            
            for (int i=0; i < a.values.Length; i++)
			{
                if (!a.values[i].IsEqual(b.values[i]))
				{
                    return false;
				}
			}
            return true;
		}

        public static bool operator !=(Vector2f a, Vector2f b)
        {
            return !(a == b);
        }

        public static Vector2f operator +(Vector2f a, Vector2f b)
        {
            var c = new Vector2f(0, 0);
            c.x = a.x + b.x;
            c.y = a.y + b.y;
            return c;
        }

        public static Vector3f operator -(Vector2f a, Vector2f b)
        {
            var c = new Vector3f(0, 0, 0);
            c.x = a.x - b.x;
            c.y = a.y - b.y;
            return c;
		}

		public static bool operator <(Vector2f a, Vector2f b)
		{
			for (int i = 0; i < a.values.Length; i++)
			{
				if (a.values[i] < b.values[i]) return true;
				else if (a.values[i] > b.values[i]) return false;
			}
			return false;
		}

		public static bool operator > (Vector2f a, Vector2f b)
		{
            if (a == b) return false;
            return !(a < b);
		}

        public static float dotProduct(Vector2f v1, Vector2f v2)
		{
            if (v1.values.Length != v2.values.Length)
			{
                return Constants.FLT_MIN;
			}
            float product = 0f;
            for (int i = 0; i < v1.values.Length; i++)
			{
                product += v1[i] * v2[i];
			}
            return product;
		}

        public static float crossProduct2D(Vector2f v1, Vector2f v2)
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