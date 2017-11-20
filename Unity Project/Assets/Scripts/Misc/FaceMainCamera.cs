using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FaceMainCamera : MonoBehaviour
{
	void Update ()
    {
        GameObject objectToFace = Camera.main.gameObject;
        Vector3 v = objectToFace.transform.position - transform.position;
        v.x = v.z = 0.0f;
        transform.LookAt(objectToFace.transform.position - v);
        transform.Rotate(0, 180, 0);
    }
}
