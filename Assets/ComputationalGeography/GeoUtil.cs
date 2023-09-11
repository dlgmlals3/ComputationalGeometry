using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Computation.geograpy
{
	using static Computation.geograpy.Extension;
	using Point2d = Vector2f;
	using Point3d = Vector3f;


	class GeoUtil
	{
		public double TOLERANCE { get; private set; }

		public bool Intersection(Point2d a, Point2d b, Point2d c, Point2d d)
		{
			var ab_c = Orientation2d(a, b, c);
			var ab_d = Orientation2d(a, b, d);
			var cd_a = Orientation2d(c, d, a);
			var cd_b = Orientation2d(c, d, b);

			if (
				ab_c == RelativePosition.Beetween || ab_c == RelativePosition.origin || ab_c == RelativePosition.destination ||
				ab_d == RelativePosition.Beetween || ab_d == RelativePosition.origin || ab_d == RelativePosition.destination ||
				cd_a == RelativePosition.Beetween || cd_a == RelativePosition.origin || cd_a == RelativePosition.destination ||
				cd_b == RelativePosition.Beetween || cd_b == RelativePosition.origin || cd_b == RelativePosition.destination
			) return true;

			var a1 = _xor(ab_c == RelativePosition.Left, ab_d == RelativePosition.Left);
			var a2 = _xor(cd_a == RelativePosition.Left, cd_b == RelativePosition.Left);
			return a1 && a2;
		}


		/// <summary>
		/// AB 벡터와 AC 벡터를 잇는 삼각형의 넓이...
		/// </summary>
		public double AreaTriangle2D(Point2d a, Point2d b, Point2d c)
		{
			var ab = b - a;
			var ac = c - a;
			var result = Vector2f.CrossProduct2D(ab, ac);

			return result / 2;
		}

		public double AreaTriangle3D(Point3d a, Point3d b, Point3d c)
		{
			var ab = b - a;
			var ac = c - a;
			var crossProduct = Vector3f.CrossProduct3D(ab, ac);
			
			var root = crossProduct.Magnitude();
			return root / 2;
		}

		public RelativePosition Orientation2d(Point2d a, Point2d b, Point2d c)
		{
			var area = AreaTriangle2D(a, b, c);

			if (area > 0 && area < TOLERANCE) area = 0;
			if (area < 0 && area < TOLERANCE) area = 0;

			Vector2f ab = b - a;
			Vector2f ac = c - a;

			if (area > 0) return RelativePosition.Left;
			if (area < 0) return RelativePosition.Right;
			if (ab.x * ac.x < 0 || ab.y * ac.y < 0) return RelativePosition.Behind;
			if (ab.Magnitude() < ac.Magnitude()) return RelativePosition.Beyond;
			if (a == c) return RelativePosition.origin;
			if (b == c) return RelativePosition.destination;
			return RelativePosition.Beetween;
		}

		/// <summary>
		/// 제대로 동작안하는  듯?
		/// </summary>
		public RelativePosition Orientation3d(Point3d a, Point3d b, Point3d c)
		{
			var area = AreaTriangle3D(a, b, c);

			if (area > 0 && area < TOLERANCE) area = 0;
			if (area < 0 && area < TOLERANCE) area = 0;

			Vector3f ab = b - a;
			Vector3f ac = c - a;
			Debug.Log("Area : " + area + "point c : " + c);

			if (area > 0f) { return RelativePosition.Left; }
			if (area < 0f) { return RelativePosition.Right; }
			if (ab.x * ac.x < 0f || ab.z * ac.z < 0f) { return RelativePosition.Behind; }
			if (ab.Magnitude() < ac.Magnitude()) { return RelativePosition.Beyond; }
			if (a == c) { return RelativePosition.origin; }
			if (b == c) { return RelativePosition.destination; }
			return  RelativePosition.Beetween;
		}

		/// <summary>
		/// A, B를 직선으로 봤을때 C의 상대적 위치
		/// </summary>
		public bool IsLeft(Point3d a, Point3d b, Point3d c)
		{
			return Orientation3d(a, b, c) == RelativePosition.Left;
		}
		public bool IsRight(Point3d a, Point3d b, Point3d c)
		{
			return Orientation3d(a, b, c) == RelativePosition.Right;
		}
		public bool Beyond(Point3d a, Point3d b, Point3d c)
		{
			return Orientation3d(a, b, c) == RelativePosition.Beyond;
		}
		public bool Behind(Point3d a, Point3d b, Point3d c)
		{
			return Orientation3d(a, b, c) == RelativePosition.Behind;
		}
	}
}
