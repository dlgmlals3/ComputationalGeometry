using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Numerics;
using UnityEngine;

namespace Computation.geograpy
{

    static class Constants
	{
        public const float FLT_MIN = -1;
	}

   
    public class Vector
    {
        protected float[] values;

		public float this[int key]
		{
            get => values[key];
            set => values[key] = value;
		}

		public static bool operator ==(Vector a, Vector b)
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

        public static bool operator !=(Vector a, Vector b)
        {
            return !(a == b);
        }

        
		public static bool operator <(Vector a, Vector b)
		{
			for (int i = 0; i < a.values.Length; i++)
			{
				if (a.values[i] < b.values[i]) return true;
				else if (a.values[i] > b.values[i]) return false;
			}
			return false;
		}

		public static bool operator > (Vector a, Vector b)
		{
            if (a == b) return false;
            return !(a < b);
		}

        public static float DotProduct(Vector v1, Vector v2)
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

        public float Magnitude()
		{
            double value = 0f;
            for (int i = 0; i < values.Length; i++)
			{
                value += Math.Pow(values[i], 2.0f);
			}
            return (float)Math.Sqrt(value);
		}

        public Vector Normalize()
		{
            var mag = Magnitude();
            for (int i = 0; i < values.Length; i++)
			{
                values[i] /= mag;
			}
            return this;
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
    class Vector3f : Vector
    {
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
        public float z
        {
            get => values[2];
            set => values[2] = value;
        }

        public Vector3f(float x, float y, float z)
        {
            this.values = new float[3];
            this.values[0] = x;
            this.values[1] = y;
            this.values[2] = z;
        }

        public static Vector3f CrossProduct3D(Vector3f v1, Vector3f v2)
        {
            float x_, y_, z_;
            x_ = (v1.y * v2.z) - (v1.z * v2.y);
            y_ = (v1.z * v2.x) - (v1.x * v2.z);
            z_ = (v1.x * v2.y) - (v2.x * v1.y);
            return new Vector3f(x_, y_, z_);
        }

        public static Vector3f operator +(Vector3f a, Vector3f b)
        {
            var c = new Vector3f(0, 0, 0);
            c.x = a.x + b.x;
            c.y = a.y + b.y;
            c.z = a.z + b.z;
            return c;
        }

        public static Vector3f operator -(Vector3f a, Vector3f b)
        {
            var c = new Vector3f(0, 0, 0);
            c.x = a.x - b.x;
            c.y = a.y - b.y;
            c.z = a.z - b.z;
            return c;
        }
    }

}