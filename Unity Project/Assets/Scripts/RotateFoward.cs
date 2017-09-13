using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateFoward : MonoBehaviour
{

	[SerializeField]
	float maxRotation;

    [SerializeField]
    GameObject playerBall;

    Rigidbody rb;

    void Awake()
    {
        rb = playerBall.GetComponent<Rigidbody>();
    }

    void FixedUpdate ()
	{
		float moveHorizontal = Input.GetAxis("Horizontal");
		float moveVertical = Input.GetAxis("Vertical");

        Vector3 velocity = rb.velocity;
        velocity.y = 0.0f;

        //Quaternion lookRotation = Quaternion.LookRotation(velocity);
        //transform.rotation = lookRotation;

        float yRotation = Vector3.SignedAngle(Vector3.forward, velocity.normalized, Vector3.up);

        Quaternion rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(transform.rotation.eulerAngles.x, yRotation, transform.rotation.eulerAngles.z), Time.deltaTime * 100.0f);
        rotation = Quaternion.Slerp(rotation, Quaternion.Euler(moveVertical * maxRotation, rotation.eulerAngles.y, -moveHorizontal * maxRotation), Time.deltaTime * 10.0f);

		transform.rotation = rotation;
	}
}
