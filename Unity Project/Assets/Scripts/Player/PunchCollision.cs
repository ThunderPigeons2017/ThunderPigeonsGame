using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using XboxCtrlrInput;

public class PunchCollision : MonoBehaviour
{
    [SerializeField]
    GameObject playerBall;

    Rigidbody playerBallrb;

    [Tooltip("Punch force base value")]
    [SerializeField]
    float punchForceBase = 4f;
    //[Tooltip("Aditional punch force depending on wind up time")]
    //[SerializeField]
    //float punchForceTimeMultiplyer = 0.6f;

    [Tooltip("Knock up force base value")]
    [SerializeField]
    float knockUpForceBase = 4f;
    //[Tooltip("Aditional Knock up force depending on wind up time")]
    //[SerializeField]
    //float knockUpForceTimeMultiplyer = 0.4f;

    [SerializeField]
    XboxButton punchButton = XboxButton.A;

    [SerializeField]
    GameObject punchVisual;

    float timer;

    void Awake()
    {
        playerBallrb = playerBall.GetComponent<Rigidbody>();
    }

    void Start()
    {
        punchVisual.SetActive(false);
    }

    void Update()
    {
        if (timer <= 0)
        {
            punchVisual.SetActive(false);
        }
        else
        {
            timer -= Time.deltaTime;
        }

        if (XCI.GetButtonDown(punchButton, (XboxController)playerBall.GetComponent<PlayerController>().playerNumber))
        {
            punchVisual.SetActive(true);
            FindObjectOfType<AudioManager>().Play("SFX-SpinAttack");
            timer = 0.1f;
        }
    }

    void OnTriggerStay(Collider other)
    {
        if (other.tag == "PlayerBall") // If its colliding with a player
        {
            if (XCI.GetButtonDown(punchButton, (XboxController)playerBall.GetComponent<PlayerController>().playerNumber))
            {


                if (other.gameObject != playerBall) // If its not colliding with this player
                {
                    Vector3 vecBetween = other.transform.position - playerBall.transform.position; // Get a vector that points from this player to the one we hit
                    vecBetween.y = 0f;
                    Vector3 tempVelocity = playerBallrb.velocity;
                    tempVelocity.y = 0f;

                    // Push the other player away
                    other.GetComponent<Rigidbody>().AddForce(vecBetween.normalized * punchForceBase, ForceMode.Impulse);

                    // knock up the other player
                    other.GetComponent<Rigidbody>().AddForce(Vector3.up * knockUpForceBase, ForceMode.Impulse);
                }
            }
        }
    }
}
