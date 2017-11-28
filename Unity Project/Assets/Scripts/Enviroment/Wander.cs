/***********************************************
 * Rummy Robots
 * by Thunder Pidgeons
 * 
 * Menu Camera Control Script
 ***********************************************/
//Codes to include basic Unity Functions
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Wander Class
/// </summary>
public class Wander : MonoBehaviour
{
    Vector3 targetPos;                                                                                                       //
                                                                                                                             //
    [SerializeField]                                                                                                         //
    float wanderRadius = 4.0f;                                                                                               //
    [SerializeField]                                                                                                         //
    float wanderAngle = 35.0f;                                                                                               //
                                                                                                                             //
    [SerializeField]                                                                                                         //
    float speed = 35.0f;                                                                                                     //
                                                                                                                             //
    Vector3 startPos;                                                                                                        //
    Quaternion startRot;                                                                                                     //
                                                                                                                             //
    void Start ()                                                                                                            //
    {                                                                                                                        //
        startPos = transform.position;                                                                                       //
        startRot = transform.rotation;                                                                                       //
                                                                                                                             //
        FindNewTargetPos();                                                                                                  //
        MoveToTargetPos();                                                                                                   //
    }                                                                                                                        //
                                                                                                                             //
    void Update ()                                                                                                           //
    {                                                                                                                        //
        MoveToTargetPos();                                                                                                   //
                                                                                                                             //
        // If we reached the target pos                                                                                      //
        if (Vector3.Distance(targetPos, transform.position) < 1.0f)                                                          //
        {                                                                                                                    //
            // Get a new position                                                                                            //
            FindNewTargetPos();                                                                                              //
        }                                                                                                                    //
                                                                                                                             //
        Vector3 vecBetween = (targetPos - transform.position).normalized;                                                    //
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(vecBetween), Time.deltaTime);      //
    }                                                                                                                        //
                                                                                                                             //
    void MoveToTargetPos()                                                                                                   //
    {                                                                                                                        //
        float step = speed * Time.deltaTime;                                                                                 //
        transform.position = Vector3.MoveTowards(transform.position, targetPos, step);                                       //
    }                                                                                                                        //
                                                                                                                             //
    void FindNewTargetPos()                                                                                                  //
    {                                                                                                                        //
        Vector3 newPos = new Vector3();                                                                                      //
                                                                                                                             //
        Quaternion rot = Quaternion.Euler(Vector3.up * Random.Range(-wanderAngle, wanderAngle));                             //
        Vector3 newForward = rot * transform.forward;                                                                        //
                                                                                                                             //
        // Get the position at the end of the wander radius                                                                  //
        newPos = newForward * wanderRadius + transform.position;                                                             //
                                                                                                                             //
        // Return our new position                                                                                           //
        targetPos = newPos;                                                                                                  //
    }                                                                                                                        //
                                                                                                                             //
    public void Respawn()                                                                                                    //
    {                                                                                                                        //
        transform.position = startPos;                                                                                       //
        transform.rotation = startRot;                                                                                       //
                                                                                                                             //
        FindNewTargetPos();                                                                                                  //
        MoveToTargetPos();                                                                                                   //
    }                                                                                                                        //
}
