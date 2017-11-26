using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using XboxCtrlrInput;

public class SpinCollision : MonoBehaviour
{
    [SerializeField]
    GameObject playerBall;

    TrailRenderer[] spinTrail = new TrailRenderer[2];

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

    float timer;

    [SerializeField]
    float spinTime = 0.5f;

    bool spinning = false;

    List<GameObject> hitObjects = new List<GameObject>();

    PlayerController playerController;

    GameManager gameManager;

    Animator animator;

    void Awake()
    {
        playerBallrb = playerBall.GetComponent<Rigidbody>();
        playerController = playerBall.GetComponent<PlayerController>();
        gameManager = FindObjectOfType<GameManager>();
    }

    void Start()
    {
        animator = playerBall.transform.parent.GetComponentInChildren<Animator>();
    }

    void Update()
    {
        if (spinning)
        {
        }
        else
        {
        }

        if (timer >= 0)
        {
            timer -= Time.deltaTime;
        }
        else
        {
            spinning = false;
            // Check if the animator has disappeared
            if (animator == null)
                animator = playerBall.transform.parent.GetComponentInChildren<Animator>();
            animator.SetBool("Spinning", spinning);
            StopTrails();
            hitObjects.Clear();
        }

        if (gameManager == null)
        {
            gameManager = FindObjectOfType<GameManager>();
        }

        // Check for input and punch
        if (spinning == false && XCI.GetButtonDown(spinButton, (XboxController)playerController.playerNumber) && (gameManager == null || gameManager.gamePause() == false))
        {
            FindObjectOfType<AudioManager>().PlayOneShot("SFX-SpinAttack");

            // Left stick
            float moveHorizontal = XCI.GetAxis(XboxAxis.LeftStickX, (XboxController)playerController.playerNumber);
            float moveVertical = XCI.GetAxis(XboxAxis.LeftStickY, (XboxController)playerController.playerNumber);

            playerBallrb.AddForce(new Vector3(moveHorizontal, 0.0f, moveVertical) * dashForce, ForceMode.Impulse);

            spinning = true;

            // Play the spin trails
            StartTrails();

            // Check if the animator has disappeared
            if (animator == null)
                animator = playerBall.transform.parent.GetComponentInChildren<Animator>();
            // Start the animation
            animator.SetTrigger("SpinAttack");
            animator.SetBool("Spinning", spinning);
            timer = spinTime;
        }
        
    }

    void OnTriggerStay(Collider other)
    {
        if (other.tag == "PlayerBall" || other.tag == "Destroyable Objects") // If its colliding with a player
        {
            //if (XCI.GetButtonDown(punchButton, (XboxController)playerBall.GetComponent<PlayerController>().playerNumber))
            if (spinning)
            {
                if (other.gameObject != playerBall) // If its not colliding with this player
                {
                    if (!hitObjects.Contains(other.gameObject))
                    {
                        hitObjects.Add(other.gameObject);

                        Vector3 vecBetween = other.transform.position - playerBall.transform.position; // Get a vector that points from this player to the one we hit
                        vecBetween.y = 0f;
                        Vector3 tempVelocity = playerBallrb.velocity;
                        tempVelocity.y = 0f;

                        Rigidbody otherRB = other.GetComponent<Rigidbody>();
                        // Push the other player away
                        otherRB.AddForce(vecBetween.normalized * spinForce, ForceMode.Impulse);

                        // knock up the other player
                        otherRB.AddForce(Vector3.up * knockUpForce, ForceMode.Impulse);

                        // Let the other player know its been hit
                        if (other.tag == "PlayerBall")
                        {
                            other.GetComponent<PlayerController>().TakeDamage();
                        }
                    }
                }
            }
        }
    }

    void StartTrails()
    {
        if (spinTrail[0] == null || spinTrail[1] == null)
        {
            spinTrail = transform.parent.gameObject.GetComponentsInChildren<TrailRenderer>();
        }
        spinTrail[0].enabled = true;
        spinTrail[1].enabled = true;
    }

    void StopTrails()
    {
        if (spinTrail[0] == null || spinTrail[1] == null)
        {
            spinTrail = transform.parent.gameObject.GetComponentsInChildren<TrailRenderer>();
        }
        spinTrail[0].enabled = false;
        spinTrail[1].enabled = false;
    }
}
