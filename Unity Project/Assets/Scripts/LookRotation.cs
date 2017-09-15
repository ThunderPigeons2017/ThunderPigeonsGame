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

        // Only update if there is input
        if (xAxis != 0f || yAxis != 0f)
        {
            lookRotation.x = xAxis;
            lookRotation.z = yAxis;
        }
        else if (moveHorizontal != 0f || moveVertical != 0f)
        {
            lookRotation.x = moveHorizontal;
            lookRotation.z = moveVertical;
        }
        else
        {
            lookRotation = rb.velocity.normalized;
            lookRotation.y = 0f;
        }

        // Make the forward rotation
        Quaternion fowardRotation = Quaternion.LookRotation(lookRotation);

        // Make the tilt rotation
        Quaternion tiltRotation = Quaternion.Euler(moveVertical * maxTiltDegrees, 0.0f, -moveHorizontal * maxTiltDegrees);

        Quaternion targetRotation = tiltRotation * fowardRotation;

        // Apply the rotation smoothly
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 10.0f * Time.deltaTime);
    }
}
