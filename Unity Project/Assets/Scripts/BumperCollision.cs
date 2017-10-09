using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BumperCollision : MonoBehaviour
{

    [SerializeField]
    GameObject playerBall;

    Rigidbody playerBallrb;

    [Header("Bounce force base value")]
    [SerializeField]
    float bounceForceBase = 4f;
    [Header("Bounce force (velocity impact amount)")]
    [SerializeField]
    float bounceForceVelocityMultiplyer = 0.6f;

    [Header("Jump force base value")]
    [SerializeField]
    float jumpForceBase = 4f;
    [Header("Jump force (velocity impact amount)")]
    [SerializeField]
    float jumpForceVelocityMultiplyer = 0.4f;

    void Awake ()
    {
        playerBallrb = playerBall.GetComponent<Rigidbody>();
	}

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player") // If its colliding with a player
        {
            if (other.gameObject != playerBall) // If its not colliding with this player
            {
                if (playerBallrb.velocity.magnitude > 1f) // Only if the velocity is above a threshold 
                {
                    Vector3 vecBetween = other.transform.position - playerBall.transform.position; // Get a vector that points from this player to the one we hit
                    vecBetween.y = 0f;
                    Vector3 tempVelocity = playerBallrb.velocity;
                    tempVelocity.y = 0f;

                    if (Mathf.Abs(Vector3.AngleBetween(vecBetween, tempVelocity)) < 10f)
                    {
                        Debug.Log("Hit - " + ((int)tempVelocity.magnitude).ToString(), gameObject);

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
