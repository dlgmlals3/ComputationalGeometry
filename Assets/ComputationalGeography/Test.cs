using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Computation.geograpy;
public class Test : MonoBehaviour
{
    [SerializeField] public Transform A;
    [SerializeField] public Transform B;
    [SerializeField] public Transform C;

    void Update()
    {
        GeoUtil geo = new GeoUtil();
        Vector3f a = new Vector3f(A.position.x, A.position.y, A.position.z);
        Vector3f b = new Vector3f(B.position.x, B.position.y, B.position.z);
        Vector3f c = new Vector3f(C.position.x, C.position.y, C.position.z);

        Debug.Log(geo.Oridentation3d(a, b, c));

    }

   
}
