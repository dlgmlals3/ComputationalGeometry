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
		public static float DegreeToRadian(float angle) { return angle * ((float)Math.PI / 180.0f); }
		public static float RadianToDegree(float angle) { return angle * (180.0f / (float)Math.PI); }

		public float AnglePlanes(Plane p1, Plane p2)
		{
			var dot = Vector3f.DotProduct(p1.GetNormal(), p2.GetNormal());
			float theta = (float)Math.Acos(Math.Abs(dot));
			return RadianToDegree(theta);
		}

		public float AngleLinePlane(Line3d l1, Plane p1)
		{
			var dot = Vector3f.DotProduct(l1.GetDirection(), p1.GetNormal());
			float theta = (float)Math.Acos(Math.Abs(dot));
			float angle = RadianToDegree(theta);
			return 90 - angle;
		}

		public float AngleLine3D(Line3d l1, Line3d l2)
		{
			var dot = Vector3f.DotProduct(l1.GetDirection(), l2.GetDirection());
			float theta = (float)Math.Acos(Math.Abs(dot));
			return RadianToDegree(theta);
		}

		public float AngleLine2D(Line2d l1, Line2d l2)
		{
			var dot = Vector3f.DotProduct(l1.GetDirection(), l2.GetDirection());
			float theta = (float)Math.Acos(Math.Abs(dot));
			return RadianToDegree(theta);
		}

		public bool Intersection(Line2d lineA, Line2d lineB, out Point2d result)
		{
			var a = lineA.GetStartPoint();
			var b = lineA.GetEndPoint();
			var c = lineB.GetStartPoint();
			var d = lineB.GetEndPoint();
			return Intersection(a, b, c, d, out result);
		}

		/// <summary>
		/// Verify : IntersectionTest() 
		/// 라인AB, 라인CD의 교차점을 구한다.
		/// dlgmlals3 그럼 3차원 점은 ????????
		/// </summary>
		public bool Intersection(Point2d a, Point2d b, Point2d c, Point2d d, out Point2d result)
		{
			result = Point2d.zero;
			Vector2f AB = b - a;
			Vector2f CD = d - c;
			Vector2f AC = c - a;
			Vector2f normal = new Vector2f(CD.y, -CD.x);
			float abDotNormal = Vector2f.DotProduct(normal, AB);

			if (abDotNormal != 0)
			{
				float acDotNormal = Vector2f.DotProduct(normal, AC);
				var t = acDotNormal / abDotNormal;
				result.x = (AB.x * t) + a.x;
				result.y = (AB.y * t) + a.y;
				return true;
			}
			return false;
		}

		/// <summary>
		/// 아직 검증 안됌
		/// </summary>
		/// <returns></returns>
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
		/// 검증 안됌..
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
