using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using XboxCtrlrInput;

public class PlayerController : MonoBehaviour
{
    public XboxController controller;

    public bool isAlive;

    public int playerNumber;

    //[HideInInspector]
    public bool grounded;

    public bool canMove = true;

    [SerializeField]
    float speed;
    [SerializeField]
    float forceDown;

    [SerializeField]
    [Tooltip("The rigidbody drag value default")]
    float normalDrag;
    [SerializeField]
    [Tooltip("The rigidbody drag value when slowing down")]
    float slowDrag;

    Rigidbody rb;

    float distanceToGround;

    [SerializeField]
    float distanceToGroundOffset = 0.1f;

    Animator animator;

    [HideInInspector]
    public float drunkenness = 0;

    [SerializeField]
    [Range(0, 5)]
    float drunkSpeed = 0.1f;

    float drunkOffset;

    public enum AnimState
    {
        Idle,
        WindUp,
        Punch
    }
    [HideInInspector]
    public AnimState animState = 0;

    [HideInInspector]
    public float drunkHorizontal;
    [HideInInspector]
    public float drunkVertical;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        distanceToGround = GetComponent<Collider>().bounds.extents.y + distanceToGroundOffset;
        animator = transform.parent.GetComponentInChildren<Animator>();
    }

    void FixedUpdate ()
    {
        // Add time to the drunk offset
        drunkOffset += Time.fixedDeltaTime * drunkSpeed;

        drunkHorizontal = Helper.Remap(Mathf.PerlinNoise(0, drunkOffset), 0, 1, -1f, 1f) * drunkenness / 100;
        drunkVertical = Helper.Remap(Mathf.PerlinNoise(drunkOffset, 0), 0, 1, -1f, 1f) * drunkenness / 100;

        // Raycast down to see if we are touching the ground
        grounded = Physics.Raycast(transform.position, Vector3.down, distanceToGround);

        // Left stick
        float moveHorizontal = XCI.GetAxis(XboxAxis.LeftStickX, controller) + drunkHorizontal;
        float moveVertical = XCI.GetAxis(XboxAxis.LeftStickY, controller) + drunkVertical;

        if (moveHorizontal > 1)
        {
            moveHorizontal -= Mathf.Abs(moveHorizontal - 1);
        }
        else if (moveHorizontal < -1)
        {
            moveHorizontal += Mathf.Abs(moveHorizontal + 1);
        }

        if (moveVertical > 1)
        {
            moveVertical -= Mathf.Abs(moveVertical - 1);
        }
        else if (moveVertical < -1)
        {
            moveVertical += Mathf.Abs(moveVertical + 1);
        }

        Vector3 movement = Vector3.zero;

        if (grounded && canMove) // Only allow the player to move if they're grounded and can move
        {
            // Create a vector 3 from the input axis'
            movement += new Vector3(moveHorizontal, 0.0f, moveVertical);
        }
        else
        {
            movement += Vector3.down * forceDown; // Apply a force down to keep the player on the ground
        }

        if (!canMove)
        {
            rb.drag = slowDrag;
        }
        else
        {
            rb.drag = normalDrag;
        }

        // Add the movement as a force
        rb.AddForce(movement * speed * 100.0f * Time.fixedDeltaTime);
        
        // Trigger input
        //float leftTrigHeight = XCI.GetAxis(XboxAxis.LeftTrigger, controller);
        //float rightTrigHeight = XCI.GetAxis(XboxAxis.RightTrigger, controller);

        //// If right bumper is pressed down, start windup animation
        //if (XCI.GetButtonDown(XboxButton.RightBumper, controller))
        //{
        //    animator.SetTrigger("WindUp");
        //}

        //// If right bumper is released, start windup animation
        //if (XCI.GetButtonUp(XboxButton.RightBumper, controller))
        //{
        //    animator.SetTrigger("Punch");
        //}

        // Set drag to the normal value
        rb.drag = normalDrag;

        if (grounded)
        {
            if (animState == AnimState.WindUp || animState == AnimState.Punch)
            {
                rb.drag = slowDrag;
            }
        }
    }

    float Remap(float value, float from1, float to1, float from2, float to2)
    {
        return (value - from1) / (to1 - from1) * (to2 - from2) + from2;
    }
}
