using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Computation.geograpy;
public class Test : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        //Vector<float> k = new Vector<float>(3);
        var k = new Vector3f(1, 2, 3);
        var u = new Vector3f(3, 2, 1);

        

        Debug.Log("k + u" + (k + u));
        Debug.Log("k - u" + (k - u));
        Debug.Log("k < u" + (k < u));
        Debug.Log("k < u" + (k == u));
        Debug.Log("k = " + k.Normalize());
    }

   
}
