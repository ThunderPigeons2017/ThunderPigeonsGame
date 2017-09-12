using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPosition : MonoBehaviour
{

	[SerializeField]
	Transform target;

	Vector3 offset;

	void Start ()
	{
		offset = transform.position - target.transform.position;
	}
	
	void LateUpdate ()
	{
		transform.position = target.transform.position + offset;
	}
}
