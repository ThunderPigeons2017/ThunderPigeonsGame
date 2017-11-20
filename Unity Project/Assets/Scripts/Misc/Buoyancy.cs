using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(MeshFilter))]
public class FloatingObject : MonoBehaviour
{
    [SerializeField]
    private float density = 0.75f;

    [SerializeField]
    private float dragInWater = 1f;

    [SerializeField]
    private float angularDragInWater = 1f;

    Rigidbody rb;
    private float initialDrag;
    private float initialAngularDrag;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();

        initialDrag = rb.drag;
        initialAngularDrag = rb.angularDrag;
    }

    private void FixedUpdate()
    {

    }
}