using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Wander : MonoBehaviour
{
    Vector3 targetPos;

    [SerializeField]
    float wanderRadius = 4.0f;
    //[SerializeField]
    //float jitterRadius = 1.0f;
    [SerializeField]
    float wanderAngle = 35.0f;

    [SerializeField]
    float speed = 35.0f;

    [SerializeField]
    Rigidbody rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Start ()
    {
        FindNewTargetPos();
        MoveToTargetPos();
    }

    void Update ()
    {
        MoveToTargetPos();

        // If we reached the target pos
        if (Vector3.Distance(targetPos, transform.position) < 1.0f)
        {
            // Get a new position
            FindNewTargetPos();
        }
    }

    void MoveToTargetPos()
    {
        // Add a force towards the position
        Vector3 vecBetween = (transform.position - targetPos).normalized;
        rb.AddForce(vecBetween * speed);

        // Look towards velocity
        transform.rotation.SetFromToRotation(transform.position, rb.velocity);
    }

    void FindNewTargetPos()
    {
        Vector3 newPos = new Vector3();

        // Get a copy of our transform
        Transform tempTransform = transform;
        // Rotate the copy
        tempTransform.Rotate(Vector3.up, Random.Range(-wanderAngle, wanderAngle));
        // Get the position at the end of the wander radius
        newPos = tempTransform.forward * wanderAngle;

        // Return our new position
        targetPos = newPos;
    }
}
