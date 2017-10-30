using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakableObject : MonoBehaviour
{

    [SerializeField]
    [Tooltip("The object that will be instantiated when this is destroyed")]
    GameObject brokenPrefab;

    [SerializeField]
    [Tooltip("The relative velocity on the impact needs to be over this amount for the object to break")]
    float relativeVelocityMinimum = 2;

    [SerializeField]
    [Range(0, 1)]
    [Tooltip("How much of the impact force is added to the newly instantiated broken prefab")]
    float collisionForceScale = 1;

    private void OnCollisionEnter(Collision collision)
    {
        // If colliding with a player
        if (collision.gameObject.tag == "PlayerBall")
        {
            // If we are above the velocity threshold
            if (collision.relativeVelocity.magnitude > relativeVelocityMinimum)
            {
                // Create the broken object
                GameObject newObject = Instantiate(brokenPrefab, gameObject.transform.position, gameObject.transform.rotation);

                // Get the rigidbody
                Rigidbody rb = newObject.GetComponent<Rigidbody>();

                // If that rigidbody exists
                if (rb != null)
                {
                    // Add a force depending on the relitive velocity
                    rb.AddForce(collision.relativeVelocity * collisionForceScale, ForceMode.Impulse);
                }
                else // Look for rigidbodys in the children
                {
                    // Get the rigidbodys in all children
                    Rigidbody[] rigidbodys = newObject.GetComponentsInChildren<Rigidbody>();
                    foreach (Rigidbody rigidbody in rigidbodys)
                    {
                        // Add a force depending on the relitive velocity
                        rigidbody.AddForce(collision.relativeVelocity * collisionForceScale, ForceMode.Impulse);
                    }
                }

                // Destroy the old object
                Destroy(gameObject);
            }
        }
    }
}
