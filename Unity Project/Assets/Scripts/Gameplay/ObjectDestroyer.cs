using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectDestroyer : MonoBehaviour
{
    [SerializeField]
    string tagToDestroy = "";

    // Destroys Objects with tagToDestroy when it enters the Trigger
    void OnTriggerEnter(Collider other)
    {
		if (other.tag == tagToDestroy)
        {
			Destroy(other.gameObject);
        }
	}
}
