using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Computation.geograpy;
public class Test : MonoBehaviour
{
    [SerializeField] public LineRenderer lineRendererA;
    [SerializeField] public LineRenderer lineRendererB;
    [SerializeField] public LineRenderer lineRendererPlane;
    [SerializeField] public Transform A;
    [SerializeField] public Transform B;
    [SerializeField] public Transform C;
	[SerializeField] public Transform D;
    [SerializeField] public GameObject plane;

    private Vector2f vector2fa;
    private Vector2f vector2fb;
    private Vector2f vector2fc;
    private Vector2f vector2fd;

    private Vector3f vector3fa;
    private Vector3f vector3fb;
    private Vector3f vector3fc;
    private Vector3f vector3fd;

    private void Start()
	{
        //IntersectionTest();
        //AngleBetweenLine2d();
        //AngleBetweenLine3d();
    }

    void Update()
    {
        UpdateCoordinates();
        //AngleBetweenLine2d();
        
    }

	private void UpdateCoordinates()
	{
        vector2fa = new Vector2f(A.position.x, A.position.z);
        vector2fb = new Vector2f(B.position.x, B.position.z);
        vector2fc = new Vector2f(C.position.x, C.position.z);
        vector2fd = new Vector2f(D.position.x, D.position.z);

        vector3fa = new Vector3f(A.position.x, A.position.y, A.position.z);
        vector3fb = new Vector3f(B.position.x, B.position.y, B.position.z);
        vector3fc = new Vector3f(C.position.x, C.position.y, C.position.z);
        vector3fd = new Vector3f(D.position.x, D.position.y, D.position.z);

        lineRendererA.SetPosition(0, A.position);
        lineRendererA.SetPosition(1, B.position);
        lineRendererB.SetPosition(0, C.position);
        lineRendererB.SetPosition(1, D.position);


        lineRendererPlane.SetPosition(0, plane.transform.position);
        lineRendererPlane.SetPosition(1, plane.transform.up * 100);

    }

	void IntersectionTest()
	{
        GeoUtil geo = new GeoUtil();
        Vector2f a = new Vector2f(A.position.x, A.position.z);
        Vector2f b = new Vector2f(B.position.x, B.position.z);
        Vector2f c = new Vector2f(C.position.x, C.position.z);
        Vector2f d = new Vector2f(D.position.x, D.position.z);

        if (geo.Intersection(a, b, c, d, out Vector2f e))
		{
            var cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
            cube.transform.position = new Vector3(e.x, 0, e.y);
            Debug.Log("intersection point : " + e);
        }
        else
		{
            Debug.Log("intersection point : " + e);
        }
    }

    void AngleBetweenLine2d()
	{
        GeoUtil geo = new GeoUtil();
        var l1 = new Line2d(vector2fa, vector2fb);
        var l2 = new Line2d(vector2fc, vector2fd);
        Debug.Log("angle : " + geo.AngleLine2D(l1, l2));
    }

    void AngleBetweenLine3d()
    {
        GeoUtil geo = new GeoUtil();
        var l1 = new Line3d(vector3fa, vector3fb);
        var l2 = new Line3d(vector3fc, vector3fd);
        Debug.Log("angle : " + geo.AngleLine3D(l1, l2));
    }

    void AngleBetweenPlane()
	{
        // dlgmlals3 Plane의 평면방정식 가져와서 해보자...
	}
}
