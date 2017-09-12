using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateFoward : MonoBehaviour
{

	[SerializeField]
	float maxRotation;

	void LateUpdate ()
	{
		float moveHorizontal = Input.GetAxis("Horizontal");
		float moveVertical = Input.GetAxis("Vertical");

		Quaternion rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(moveVertical * maxRotation, 0.0f, -moveHorizontal * maxRotation), Time.deltaTime * 10.0f);

		transform.rotation = rotation;
	}
}
