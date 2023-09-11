using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Computation.geograpy;
public class Test : MonoBehaviour
{
    [SerializeField] public Transform A;
    [SerializeField] public Transform B;
    [SerializeField] public Transform C;
    [SerializeField] public Transform D;

    void Update()
    {
        GeoUtil geo = new GeoUtil();
        Vector3f a = new Vector3f(A.position.x, A.position.y, A.position.z);
        Vector3f b = new Vector3f(B.position.x, B.position.y, B.position.z);
        Vector3f c = new Vector3f(C.position.x, C.position.y, C.position.z);

        //Debug.Log(geo.Orientation3d(a, b, c));

        var intersectionRet = geo.Intersection(
            new Vector2f(A.position.x, A.position.z),
            new Vector2f(B.position.x, B.position.z),
            new Vector2f(C.position.x, C.position.z),
            new Vector2f(D.position.x, D.position.z)
        );
        Debug.Log("intersection : " + intersectionRet);
    }


}
