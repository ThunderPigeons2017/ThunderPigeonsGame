using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectDestroyer : MonoBehaviour {
	
	// Destroys Objects tagged 'Destroyable Objects' when it enters the Trigger
	void OnTriggerEnter (Collider other) {

		if (other.tag == "Destroyable Objects") {
			Destroy (gameObject);
        }
	}
}
