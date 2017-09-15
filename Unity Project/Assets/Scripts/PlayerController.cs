﻿using System.Collections;
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

        Vector3 movement = new Vector3(moveHorizontal, 0.0f, moveVertical);

        rb.AddForce(movement * speed * 100.0f * Time.deltaTime);

    }
}
