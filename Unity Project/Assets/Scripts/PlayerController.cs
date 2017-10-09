using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using XboxCtrlrInput;

public class PlayerController : MonoBehaviour
{
    public XboxController controller;

    public bool isAlive;

    public int playerNumber;

    [SerializeField]
    float speed;
    [SerializeField]
    float forceDown;

    Rigidbody rb;

    public bool grounded;
    float distanceToGround;

    void Awake ()
    {
        rb = GetComponent<Rigidbody>();
        distanceToGround = GetComponent<Collider>().bounds.extents.y + 0.1f;
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


        // Add the movement as a force
        rb.AddForce(movement * speed * 100.0f * Time.fixedDeltaTime);

        if (rb.velocity.magnitude >= 20f)
        {
            rb.AddForce((rb.velocity.magnitude - 20f) * -rb.velocity.normalized);
        }
    }
}
