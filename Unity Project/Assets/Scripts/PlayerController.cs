using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using XboxCtrlrInput;

public class PlayerController : MonoBehaviour
{
    public XboxController controller;

    [SerializeField]
    float speed;

    private Rigidbody rb;

    void Start ()
    {
        rb = GetComponent<Rigidbody>();
        
    }

    void FixedUpdate ()
    {
        // Left stick
        float moveHorizontal = XCI.GetAxis(XboxAxis.LeftStickX, controller);
        float moveVertical = XCI.GetAxis(XboxAxis.LeftStickY, controller);

        // Create a vector 3 from the input axis'
        Vector3 movement = new Vector3(moveHorizontal, 0.0f, moveVertical);

        // Add the movement as a force
        rb.AddForce(movement * speed * 100.0f * Time.fixedDeltaTime);
    }
}
