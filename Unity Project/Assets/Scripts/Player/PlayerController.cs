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

    public enum AnimState
    {
        Idle,
        WindUp,
        Punch
    }
    [HideInInspector]
    public AnimState animState = 0;

    void Awake ()
    {
        rb = GetComponent<Rigidbody>();
        distanceToGround = GetComponent<Collider>().bounds.extents.y + distanceToGroundOffset;
        animator = transform.parent.GetComponentInChildren<Animator>();
    }

    void FixedUpdate ()
    {
        // Raycast down to see if we are touching the ground
        grounded = Physics.Raycast(transform.position, Vector3.down, distanceToGround);

        // Left stick
        float moveHorizontal = XCI.GetAxis(XboxAxis.LeftStickX, controller);
        float moveVertical = XCI.GetAxis(XboxAxis.LeftStickY, controller);

        Vector3 movement = Vector3.zero;

        if (grounded) // Only allow the player to move if they're grounded
        {
            // Create a vector 3 from the input axis'
            movement += new Vector3(moveHorizontal, 0.0f, moveVertical);
        }
        else
        {
            movement += Vector3.down * forceDown; // Apply a force down to keep the player on the ground
        }

        if (animState != AnimState.Punch) // Only move if not punching
        {
            // Add the movement as a force
            rb.AddForce(movement * speed * 100.0f * Time.fixedDeltaTime);
        }

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
}
