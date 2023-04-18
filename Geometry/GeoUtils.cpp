#include "GeoUtils.h"

double jmk::areaTriangle2d(const Point2d& a, const Point2d& b, const Point2d& c) 
{
	auto AB = b - a;
	auto AC = c - a;
	auto result = crossProduct2d(AB, AC);
	return result / 2;
}

int jmk::orientation2d(const Point2d& a, const Point2d& b, const Point2d& c) 
{
	auto area = areaTriangle2d(a, b, c);
	if (area > 0 && area < TOLERANCE) {
		area = 0;
	}

	if (area < 0 && area > TOLERANCE) {
		area = 0;
	}

	Vector2f ab = b - a;
	Vector2f ac = c - a;

	if (area > 0)
		return LEFT;
	if (area < 0)
		return RIGHT;
	if ((ab[X] * ac[X] < 0) || (ab[Y] * ac[Y] < 0))
		return BEHIND;
	if (ab.magnitude() < ac.magnitude())
		return BEYOND;
	if (a == c)
		return ORIGIN;
	if (b == c)
		return DESTINATION;

	return BETWEEN;
}