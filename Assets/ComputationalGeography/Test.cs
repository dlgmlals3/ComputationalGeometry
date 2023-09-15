using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Computation.geograpy;

public class Test : MonoBehaviour
{

    [SerializeField] public LineRenderer lineRendererA;
    [SerializeField] public LineRenderer lineRendererB;
    [SerializeField] public LineRenderer lineRendererC;
    [SerializeField] public LineRenderer lineRendererPlane;
    [SerializeField] public Transform A;
    [SerializeField] public Transform B;
    [SerializeField] public Transform C;
	[SerializeField] public Transform D;
    [SerializeField] public Transform E;
    [SerializeField] public Transform F;
    [SerializeField] public GameObject plane;
    [SerializeField] public GameObject target;
    private Vector2f vector2fa;
    private Vector2f vector2fb;
    private Vector2f vector2fc;
    private Vector2f vector2fd;
    private Vector2f vector2fe;
    private Vector2f vector2ff;

    private Vector3f vector3fa;
    private Vector3f vector3fb;
    private Vector3f vector3fc;
    private Vector3f vector3fd;
    private Vector3f vector3fe;
    private Vector3f vector3ff;

    private GeoUtil geo;
    void Start()
	{
        geo = new GeoUtil();
        UpdateCoordinates();
    }

    void Update()
    {
        UpdateCoordinates();
        IntersectionTest1();
    }

    /// <summary>
    /// 점에서 직선으로 수직인 지점을 찾고, 그 지점과 거리를 구한다.
    /// </summary>
    void DistanceTest()
	{
        GeoUtil geo = new GeoUtil();
        var line3d = new Line3d(vector3fa, vector3fb);
        float distance = geo.Distance(line3d, vector3fc, out float t, out Vector3f point);
        //vector3fc, point
        lineRendererPlane.SetPosition(0, new Vector3(vector3fc.x, vector3fc.y, vector3fc.z));
        lineRendererPlane.SetPosition(1, new Vector3(point.x, point.y, point.z));

        Debug.Log("DistanceTest : t " + t + " distance : " + distance);
    }

    /// <summary>
    /// 두 직선이 교차하는지 검사한다.
    /// </summary>
    void CoLinearTest()
    {
        GeoUtil geo = new GeoUtil();
        var v1 = vector3fb - vector3fa;
        var v2 = vector3fd - vector3fc;
        Debug.Log("Collinear test : " + geo.Collinear(v1, v2));
    }

    /// <summary>
    /// 두 평면이 교차하는지 검사한다.
    /// </summary>
    void CoplanerTest()
    {
        GeoUtil geo = new GeoUtil();
        var v1 = vector3fb - vector3fa;
        var v2 = vector3fc - vector3fa;
        var v3 = vector3fd - vector3fa;
        var v4 = vector3fe - vector3fa;
        Debug.Log("Collinear test : " + geo.Coplaner(v1, v2, v3, v4));
    }
    

    /// <summary>
    /// 두 라인의 사이 각도를 리턴한다.
    /// </summary>
    void AngleBetweenLine()
	{
        GeoUtil geo = new GeoUtil();
        var l1 = new Line2d(vector2fa, vector2fb);
        var l2 = new Line2d(vector2fc, vector2fd);
        Debug.Log("2d angle : " + geo.AngleLine2D(l1, l2));

        var l13d = new Line3d(vector3fa, vector3fb);
        var l23d = new Line3d(vector3fc, vector3fd);
        Debug.Log("3d angle : " + geo.AngleLine3D(l13d, l23d));
    }

    /// <summary>
    /// 두 라인의 교차점을 구한다.
    /// </summary>
    void IntersectionTest1()
    {
        if (geo.Intersection2D(vector2fa, vector2fb, vector2fc, vector2fd))
        {
            Debug.Log("Intersection is generated.");
        }
        else
        {
            Debug.Log("ntersection is NOT !! generated");
        }
    }

    void IntersectionTest2()
    {
        if (geo.Intersection(vector2fa, vector2fb, vector2fc, vector2fd, out Vector2f e))
        {
            target.transform.position = new Vector3(e.x, 0, e.y);            
            Debug.Log("intersection point : " + e);
        }
        else
        {
            Debug.Log("intersection point : " + e);
        }
    }

    void OrientationTest()
	{
        Debug.Log("2d orientation : " + geo.Orientation2d(vector2fa, vector2fb, vector2fc));
        Debug.Log("3d orientation : " + geo.Orientation3d(vector3fa, vector3fb, vector3fc));
    }

    private void UpdateCoordinates()
    {
        vector2fa = new Vector2f(A.position.x, A.position.z);
        vector2fb = new Vector2f(B.position.x, B.position.z);
        vector2fc = new Vector2f(C.position.x, C.position.z);
        vector2fd = new Vector2f(D.position.x, D.position.z);
        vector2fe = new Vector2f(E.position.x, E.position.z);
        vector2ff = new Vector2f(F.position.x, F.position.z);

        vector3fa = new Vector3f(A.position.x, A.position.y, A.position.z);
        vector3fb = new Vector3f(B.position.x, B.position.y, B.position.z);
        vector3fc = new Vector3f(C.position.x, C.position.y, C.position.z);
        vector3fd = new Vector3f(D.position.x, D.position.y, D.position.z);
        vector3fc = new Vector3f(C.position.x, C.position.y, C.position.z);
        vector3fd = new Vector3f(D.position.x, D.position.y, D.position.z);
        vector3fe = new Vector3f(E.position.x, E.position.y, E.position.z);
        vector3ff = new Vector3f(F.position.x, F.position.y, F.position.z);

        lineRendererA.SetPosition(0, A.position);
        lineRendererA.SetPosition(1, B.position);
        lineRendererB.SetPosition(0, C.position);
        lineRendererB.SetPosition(1, D.position);
        lineRendererC.SetPosition(0, E.position);
        lineRendererC.SetPosition(1, F.position);

        lineRendererPlane.SetPosition(0, plane.transform.position);
        lineRendererPlane.SetPosition(1, plane.transform.up * 100);
    }
}
