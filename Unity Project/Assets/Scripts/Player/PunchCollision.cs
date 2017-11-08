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

    [Tooltip("Knock up force base value")]
    [SerializeField]
    float knockUpForceBase = 4f;

    [SerializeField]
    XboxButton spinButton = XboxButton.A;

    [SerializeField]
    GameObject spinVisual;

    float timer;

    [SerializeField]
    float spinTime = 0.5f;

    bool spinning = false;

    AudioManager audioManager;

    List<GameObject> hitPlayers = new List<GameObject>();

    void Awake()
    {
        playerBallrb = playerBall.GetComponent<Rigidbody>();
        audioManager = FindObjectOfType<AudioManager>();
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

        if (spinning == false && XCI.GetButtonDown(spinButton, (XboxController)playerBall.GetComponent<PlayerController>().playerNumber))
        {
            audioManager.Play("SFX-SpinAttack");

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
                        other.GetComponent<Rigidbody>().AddForce(vecBetween.normalized * punchForceBase, ForceMode.Impulse);

                        // knock up the other player
                        other.GetComponent<Rigidbody>().AddForce(Vector3.up * knockUpForceBase, ForceMode.Impulse);
                    }
                }
            }
        }
    }
}
