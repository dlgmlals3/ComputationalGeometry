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

		/// <summary>
		/// 점에서 직선으로 수직인 지점에서 점과 선의 최소 거리를 구한다.
		/// </summary>
		public float Distance(Line3d l1, Point3d Y, out float t, out Point3d point)
		{
			Vector3f AY = Y - l1.GetPoint();
			t = Vector3f.DotProduct(l1.GetDirection(), AY);
			point = l1.GetPoint() + l1.GetDirection() * t;

			return (point - Y).Magnitude();
		}

		/// <summary>
		/// 4개의 점이 한 평면에 있는지 확인한다.
		/// </summary>
		public bool Coplaner(Point3d a, Point3d b, Point3d c, Point3d d)
		{
			var p1 = b - a;
			var p2 = c - a;
			var p3 = d - a;
			return CoPlaner(p1, p2, p3);
		}

		/// <summary>
		/// 3개의 벡터가 한평면에 있는지 확인한다.
		/// </summary>
		public bool CoPlaner(Vector3f p1, Vector3f p2, Vector3f p3)
		{
			var value = scalerTripleProduct(p1, p2, p3);
			return value == 0;
		}

		/// <summary>
		/// Point 3개가 동일한 라인 위에 있는지 확인한다.
		/// </summary>
		public bool Collinear(Point3d p1, Point3d p2, Point3d p3)
		{
			var v1 = p2 - p1;
			var v2 = p3 - p1;
			return Collinear(v1, v2);
		}

		/// <summary>
		/// 벡터 A, B가 동일한 라인 위에 있는지 확인한다.
		/// </summary>
		public bool Collinear(Vector3f a, Vector3f b)
		{
			var v1 = a.x * b.y - a.y * b.x;
			var v2 = a.y * b.z - a.z * b.y;
			var v3 = a.x * b.z - a.z * b.x;
			return v1 == 0 && v2 == 0 && v3 == 0;
		}

		/// <summary>
		/// 평면과 평면 사이의 각도를 구한다.
		/// </summary>
		public float AnglePlanes(Plane p1, Plane p2)
		{
			var dot = Vector3f.DotProduct(p1.GetNormal(), p2.GetNormal());
			float theta = (float)Math.Acos(Math.Abs(dot));
			return RadianToDegree(theta);
		}

		/// <summary>
		/// 라인과 평면사이의 각도를 구한다.
		/// </summary>
		public float AngleLinePlane(Line3d l1, Plane p1)
		{
			var dot = Vector3f.DotProduct(l1.GetDirection(), p1.GetNormal());
			float theta = (float)Math.Acos(Math.Abs(dot));
			float angle = RadianToDegree(theta);
			return 90 - angle;
		}

		/// <summary>
		/// 라인과 라인사이의 각도를 구한다.
		/// </summary>
		public float AngleLine3D(Line3d l1, Line3d l2)
		{
			var dot = Vector3f.DotProduct(l1.GetDirection(), l2.GetDirection());
			float theta = (float)Math.Acos(dot);
			return RadianToDegree(theta);
		}

		/// <summary>
		/// 2차원 라인과 라인 사이의 각도를 구한다.
		/// </summary>
		public float AngleLine2D(Line2d l1, Line2d l2)
		{
			var dot = Vector3f.DotProduct(l1.GetDirection(), l2.GetDirection());
			float theta = (float)Math.Acos(Math.Abs(dot));
			return RadianToDegree(theta);
		}

		/// <summary>
		/// 2차원 라인과 2차원 라인의 교차점을 구한다.
		/// </summary>
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
		/// 라인 세그먼트 단위로 교차점 검사 2d
		/// </summary>
		/// <returns></returns>
		public bool Intersection2D(Point2d a, Point2d b, Point2d c, Point2d d)
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

		/// <summary>
		/// AB 벡터와 AC 벡터를 잇는 삼각형의 넓이...
		/// </summary>
		public double AreaTriangle3D(Point3d a, Point3d b, Point3d c)
		{
			var ab = b - a;
			var ac = c - a;
			var crossProduct = Vector3f.CrossProduct3D(ab, ac);
			
			var root = crossProduct.Magnitude();
			return root / 2;
		}

		private double OrientationXY3D(Point3d a, Point3d b, Point3d c)
		{
			var ab = b - a;
			var ac = c - a;
			var crossProduct = Vector3f.CrossProduct3D(ab, ac);
			var up = new Vector3f(0, -1, 0);
			return Vector3f.DotProduct(up, crossProduct);
		}

		/// <summary>
		/// AB 벡터가 있을때 점C의 상대적 위치를 알수있다.
		/// </summary>
		public RelativePosition Orientation2d(Point2d a, Point2d b, Point2d c)
		{
			var area = AreaTriangle2D(a, b, c);

			if (area > 0 && area < TOLERANCE) area = 0;
			if (area < 0 && area > TOLERANCE) area = 0;

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
		/// AB 벡터가 있을때 점C의 상대적 위치를 알수있다.
		/// </summary>
		public RelativePosition Orientation3d(Point3d a, Point3d b, Point3d c)
		{
			var area = OrientationXY3D(a, b, c);

			if (area > 0 && area < TOLERANCE) area = 0;
			if (area < 0 && area > TOLERANCE) area = 0;

			Vector3f ab = b - a;
			Vector3f ac = c - a;

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

		private float scalerTripleProduct(Vector3f v1, Vector3f v2, Vector3f v3)
		{
			var cross = Vector3f.CrossProduct3D(v2, v3);
			return Vector3f.DotProduct(v1, cross);
		}
	}
}
