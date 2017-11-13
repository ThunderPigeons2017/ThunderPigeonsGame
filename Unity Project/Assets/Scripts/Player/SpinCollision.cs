using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using XboxCtrlrInput;

public class SpinCollision : MonoBehaviour
{
    [SerializeField]
    GameObject playerBall;

    Rigidbody playerBallrb;

    [Tooltip("How much force to use for the initial dash/charge")]
    [SerializeField]
    float dashForce = 5;

    [Tooltip("How force to hit the other player away with")]
    [SerializeField]
    float spinForce = 4f;

    [Tooltip("How force to hit the other player up with")]
    [SerializeField]
    float knockUpForce = 4f;

    [SerializeField]
    XboxButton spinButton = XboxButton.A;

    [SerializeField]
    GameObject spinVisual;

    float timer;

    [SerializeField]
    float spinTime = 0.5f;

    bool spinning = false;

    List<GameObject> hitPlayers = new List<GameObject>();

    PlayerController playerController;

    void Awake()
    {
        playerBallrb = playerBall.GetComponent<Rigidbody>();
        playerController = playerBall.GetComponent<PlayerController>();
    }

    void Start()
    {
        spinVisual.SetActive(false);
    }

    void Update()
    {
        if (spinning)
        {
            spinVisual.SetActive(true);
        }
        else
        {
            spinVisual.SetActive(false);
        }

        if (timer >= 0)
        {
            timer -= Time.deltaTime;
        }
        else
        {
            spinning = false;

            hitPlayers.Clear();
        }

        // Check for input and punch
        if (spinning == false && XCI.GetButtonDown(spinButton, (XboxController)playerController.playerNumber))
        {
            FindObjectOfType<AudioManager>().Play("SFX-SpinAttack");

            // Left stick
            float moveHorizontal = XCI.GetAxis(XboxAxis.LeftStickX, (XboxController)playerController.playerNumber);
            float moveVertical = XCI.GetAxis(XboxAxis.LeftStickY, (XboxController)playerController.playerNumber);

            playerBallrb.AddForce(new Vector3(moveHorizontal, 0.0f, moveVertical) * dashForce, ForceMode.Impulse);

            spinning = true;
            timer = spinTime;
        }
    }

    void OnTriggerStay(Collider other)
    {
        if (other.tag == "PlayerBall") // If its colliding with a player
        {
            //if (XCI.GetButtonDown(punchButton, (XboxController)playerBall.GetComponent<PlayerController>().playerNumber))
            if (spinning)
            {
                if (other.gameObject != playerBall) // If its not colliding with this player
                {
                    if (!hitPlayers.Contains(other.gameObject))
                    {
                        hitPlayers.Add(other.gameObject);

                        Vector3 vecBetween = other.transform.position - playerBall.transform.position; // Get a vector that points from this player to the one we hit
                        vecBetween.y = 0f;
                        Vector3 tempVelocity = playerBallrb.velocity;
                        tempVelocity.y = 0f;

                        // Push the other player away
                        other.GetComponent<Rigidbody>().AddForce(vecBetween.normalized * spinForce, ForceMode.Impulse);

                        // knock up the other player
                        other.GetComponent<Rigidbody>().AddForce(Vector3.up * knockUpForce, ForceMode.Impulse);
                    }
                }
            }
        }
    }
}
