using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BumperCollision : MonoBehaviour
{

    [SerializeField]
    GameObject playerBall;

    Rigidbody playerBallrb;
	void Start ()
    {
        playerBallrb = playerBall.GetComponent<Rigidbody>();
	}

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "PlayerBody")
        {
            Rigidbody otherRigid = other.GetComponent<Rigidbody>();

            Vector3 vecBetween = playerBall.transform.position - other.transform.position;
            vecBetween.y = 0.0f;
            if (Mathf.Abs(Vector3.Angle(playerBallrb.velocity, vecBetween)) > 10f)
            {
                otherRigid.AddForce(playerBallrb.velocity, ForceMode.Impulse);
                //playerBallrb.AddForce(-playerBallrb.velocity, ForceMode.Impulse);
            }
        }
    }
}
