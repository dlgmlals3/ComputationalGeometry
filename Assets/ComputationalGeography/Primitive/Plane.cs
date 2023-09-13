using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Computation.geograpy
{
	using Point3d = Vector3f;
	using Point2d = Vector2f;

	public class Plane
	{
		private Vector3f normal;
		private float d;
		public Plane()
		{
			d = 0;
			normal = Vector3f.zero;
		}

		public Plane(Vector3f normal, float constant)
		{
			this.normal = normal;
			this.normal.Normalize();
			this.d = constant;
		}

		public Plane(Point3d p1, Point3d p2, Point3d p3)
		{
			var v12 = p2 - p1;
			var v13 = p3 - p1;
			this.normal = Vector3f.CrossProduct3D(v12, v13);
			this.normal.Normalize();
			this.d = Vector3f.DotProduct(this.normal, p3);
		}
		public Vector3f GetNormal() => normal;


	}
}
