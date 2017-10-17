using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BumperCollision : MonoBehaviour
{

    [SerializeField]
    GameObject playerBall;

    Rigidbody playerBallrb;

    [Tooltip("Minimum Velocity for a collision to occur")]
    [SerializeField]
    float minimumVelocity = 1f;

    [Tooltip("Bounce force base value")]
    [SerializeField]
    float bounceForceBase = 4f;
    [Tooltip("Additional bounce force (how much the velocity impacts)")]
    [SerializeField]
    float bounceForceVelocityMultiplyer = 0.6f;

    [Tooltip("Jump force base value")]
    [SerializeField]
    float jumpForceBase = 4f;
    [Tooltip("Additional jump force (how much the velocity impacts)")]
    [SerializeField]
    float jumpForceVelocityMultiplyer = 0.4f;

    void Awake ()
    {
        playerBallrb = playerBall.GetComponent<Rigidbody>();
	}

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "PlayerBall") // If its colliding with a player
        {
            if (other.gameObject != playerBall) // If its not colliding with this player
            {
                if (playerBallrb.velocity.magnitude >= minimumVelocity) // Only if the velocity is above a threshold 
                {
                    Vector3 vecBetween = other.transform.position - playerBall.transform.position; // Get a vector that points from this player to the one we hit
                    vecBetween.y = 0f;
                    Vector3 tempVelocity = playerBallrb.velocity;
                    tempVelocity.y = 0f;

                    if (Mathf.Abs(Vector3.AngleBetween(vecBetween, tempVelocity)) < 10f)
                    {
                        // Bounce the other player away
                        other.GetComponent<Rigidbody>().AddForce(vecBetween.normalized * (bounceForceBase + tempVelocity.magnitude * bounceForceVelocityMultiplyer), ForceMode.Impulse);
                    
                        // Bump the other player up
                        other.GetComponent<Rigidbody>().AddForce(Vector3.up * (jumpForceBase + tempVelocity.magnitude * jumpForceVelocityMultiplyer), ForceMode.Impulse);
                    }
                }
            }
        }
    }
}
