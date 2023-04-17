#include "Vector.h"
using namespace jmk;

float corssProduct2d(Vector2f v1, Vector2f v2)
{
	return v1[X] * v2[Y] - v1[Y] * v2[X];
}

Vector3f crossProduct3d(Vector3f v1, Vector3f v2) 
{
	Vector3f a = { (v1[Y] * v2[Z] - v1[Z] * v2[Y]) , 
		(v1[Z] * v2[X] - v1[X] * v2[Z]) , 
		(v1[X] * v2[Y] - v1[Y] * v2[X]) };
	return a;
}


