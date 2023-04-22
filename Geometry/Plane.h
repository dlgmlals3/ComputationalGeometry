#pragma once

#include "Point.h"

namespace jmk {
	template <class coord_type>
	class Plane {
		Vector3f normal;
		float d = 0;
	public:
		Plane() {}
		Plane(Vector3f& _normal, float _constant) : normal(_normal), d(_constant) {}
		Plane(Point3d& _p1, Point3d& _p2, Point3d& _p3) {
			auto v12 = _p2 - _p1;
			auto v13 = _p3 - _p1;

			normal = crossProduct(v12, v13);
			d = dotProduct(normal, _p1);
		}
	};
}