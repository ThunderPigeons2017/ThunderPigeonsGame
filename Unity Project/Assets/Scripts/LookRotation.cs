using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using XboxCtrlrInput;

public class LookRotation : MonoBehaviour
{
    XboxController controller;

    [SerializeField]
    GameObject playerBall;

    [SerializeField]
    [Range(1, 90)]
    float maxTiltDegrees;

    [SerializeField]
    float minTiltingVelocity;
    [SerializeField]
    float maxTiltingVelocity;

    Vector3 lookRotation;

    Rigidbody rb;

    void Awake()
    {
        controller = playerBall.GetComponent<PlayerController>().controller;
        rb = playerBall.GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        // Left stick
        float moveHorizontal = XCI.GetAxis(XboxAxis.LeftStickX, controller);
        float moveVertical = XCI.GetAxis(XboxAxis.LeftStickY, controller);

        // Right stick
        float xAxis = XCI.GetAxis(XboxAxis.RightStickX, controller);
        float yAxis = XCI.GetAxis(XboxAxis.RightStickY, controller);

        // Only look rotation update if there is input
        if (xAxis != 0f || yAxis != 0f) // Prioritise right stick for the rotation
        {
            lookRotation.x = xAxis;
            lookRotation.z = yAxis;
        }
        else if (moveHorizontal != 0f || moveVertical != 0f) // Also use the right stick for rotation
        {
            lookRotation.x = moveHorizontal;
            lookRotation.z = moveVertical;
        }

        // Make the forward rotation
        Quaternion fowardRotation = Quaternion.identity;
        if (lookRotation != Vector3.zero) // Only set if lookRotation isn't zero
        { 
            fowardRotation = Quaternion.LookRotation(lookRotation);
        }

        // Figure out the tilt degrees depending on the velocity magnitude
        float tiltDegrees = 0.0f;
        if (rb.velocity.magnitude >= maxTiltingVelocity)
        {
            tiltDegrees = maxTiltDegrees;
        }
        else if (rb.velocity.magnitude >= minTiltingVelocity)
        {
            tiltDegrees = rb.velocity.magnitude / maxTiltingVelocity * maxTiltDegrees;
        }

        // Make the tilt rotation
        Quaternion tiltRotation = Quaternion.Euler(moveVertical * tiltDegrees, 0.0f, -moveHorizontal * tiltDegrees);

        // Create the rotation my combining the tilt & foward correctly
        Quaternion targetRotation = tiltRotation * fowardRotation;

        // Apply the rotation smoothly
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 5.0f * Time.deltaTime);
    }
}
