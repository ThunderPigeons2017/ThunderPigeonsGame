/* Group Project Camera Control Codes
 * by RJ Ortega 
 * 
 * Important: Do not attach this to the Camera object, but instead to a blank object that holds the camera object as its child
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour
{

    public float m_DampTime = 0.2f;                                             //approximate time for the camera to move to new location
    public float minimumDistance = 5f;
    public float distanceScale = 2f;

    private PlayerController[] m_Targets = new PlayerController[4];             //array of gaming objects that would be the targets for camera to adjust to
    private Camera m_Camera;                                                    //reference to the camera attached as child
    private float m_ZoomSpeed;                                                  //damps zooming, slows it down to make it less jarring
    private Vector3 m_MoveVelocity;                                             //damps camera movement and panning to avoid jarring camera movements
    private Vector3 m_DesiredPosition;                                          //position that camera is trying to reach
    

    [SerializeField]
    private GameObject m_gmObject;
    private GameManager m_gm;

    // Use this for initialization
    private void Awake()
    {
        m_Camera = GetComponent<Camera>();	                                    //references camera and gets values

        m_gm = m_gmObject.GetComponent<GameManager>();
    }

    //this updates every runtime
    private void FixedUpdate()
    {
        for (int i = 0; i < 4; i++)
        {
            if (m_gm.players[i] != null)
            {
                m_Targets[i] = m_gm.players[i].GetComponentInChildren<PlayerController>();
            }
            else
            {
                m_Targets[i] = null;
            }
        }

        Move();                                                                 //calls move function to move camera
       
    }

    private void Move()
    {
        m_DesiredPosition = FindAveragePosition();                                //Function that tries to find all playerobjects target position

        float distance = FindDistance();

        if (distance <= minimumDistance)
        {
            distance = minimumDistance;
        }

        m_DesiredPosition += - m_Camera.transform.forward * distance * distanceScale;

        m_Camera.transform.position = Vector3.SmoothDamp(m_Camera.transform.position, m_DesiredPosition, ref m_MoveVelocity, m_DampTime);
    }

    private Vector3 FindAveragePosition()
    {
        Vector3 averagePos = new Vector3();
        int numTargets = 0;

        for (int i = 0; i < m_Targets.Length; i++)
        {
            if (m_Targets[i] == null || !m_Targets[i].isAlive)
                continue;

            averagePos += m_Targets[i].gameObject.transform.position;
            numTargets++;
        }

        if (numTargets > 0)
            averagePos /= numTargets;

        return averagePos;
    }

    public float FindDistance()
    {
        float distance = 0f;

        //Vector3 averagePos = FindAveragePosition();

        //float targetDistanceToAverage = 0f;
        //float temporaryContainer = 0f;

        //for (int i = 0; i < m_Targets.Length; i++)
        //{
        //    if (m_Targets[i] == null || !m_Targets[i].isAlive)
        //        continue;

        //    temporaryContainer = Vector3.Distance(m_Targets[i].gameObject.transform.position, averagePos);
        //    targetDistanceToAverage = Mathf.Max(temporaryContainer, targetDistanceToAverage);
        //}

        for (int i = 0; i < m_Targets.Length; i++)
        {
            if (m_Targets[i] == null || !m_Targets[i].isAlive)
                continue;

            PlayerController playerA = m_Targets[i];

            for (int j = i + 1; j < m_Targets.Length; j++)
            {
                if (m_Targets[j] == null || !m_Targets[j].isAlive)
                    continue;

                PlayerController playerB = m_Targets[j];

                float currentDistance = Vector3.Distance(playerA.transform.position, playerB.transform.position);
                if (currentDistance > distance)
                {
                    distance = currentDistance;
                }
            }

        }

        return distance;
    }
    
    public void SetStartPositionAndSize()
    {
        FindAveragePosition();

        transform.position = m_DesiredPosition;
    }

}
