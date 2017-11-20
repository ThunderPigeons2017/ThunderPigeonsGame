using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FaceObject : MonoBehaviour
{
    [SerializeField]
    public GameObject objectToFace;

	void Update ()
    {
        Vector3 v = objectToFace.transform.position - transform.position;
        v.x = v.z = 0.0f;
        transform.LookAt(objectToFace.transform.position - v);
        transform.Rotate(0, 180, 0);
    }
}
