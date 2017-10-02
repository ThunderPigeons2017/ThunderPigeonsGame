/* Group Project Camera Control Codes
 * by RJ Ortega 
 * 
 * Important: Do not attach this to the Camera object, but instead to a blank object that holds the camera object as its child
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour {

    public float m_DampTime = 0.2f;                             //approximate time for the camera to move to new location
    public float m_ScreenEdgeBufer = 4f;                        //added to sides to ensure players don't go off camera
    public float m_MinSize = 6.5f;                              //stops camera from zooming in
    public Transform[] m_Targets;                               //array of gaming objects that would be the targets for camera to adjust to

    private Camera m_Camera;                                    //reference to the camera attached as child
    private float m_ZoomSpeed;                                  //damps zooming, slows it down to make it less jarring
    private Vector3 m_MoveVelocity;                             //damps camera movement and panning to avoid jarring camera movements
    private Vector3 m_DesiredPosition;                          //position that camera is trying to reach

	// Use this for initialization
	private void Awake ()
    {
        m_Camera = GetComponentInChildren<Camera>();	        //references camera and gets values
	}

    //this updates every runtime
    private void FixedUpdate()                                  
    {
        Move();                                                //calls move function to move camera
        Zoom();                                                //calls Zoom function to Zoom Camera
    }

    private void Move()
    {
        FindAveragePosition();                                //Function that tries to find all playerobjects target position

        transform.position = Vector3.SmoothDamp(transform.position, m_DesiredPosition, ref m_MoveVelocity, m_DampTime); //
    }

    private void FindAveragePosition()
    {
        Vector3 averagePos = new Vector3();
        int numTargets = 0;

        for (int i = 0; i < m_Targets.Length; i++)
        {
            if (!m_Targets[i].gameObject.activeSelf)
                continue;

            averagePos += m_Targets[i].position;
            numTargets++;
        }

        if (numTargets > 0)
            averagePos /= numTargets;

        averagePos.y = transform.position.y;

        m_DesiredPosition = averagePos;
    }

    private void Zoom()
    {
        float requiredSize = FindRequiredSize();
        m_Camera.orthographicSize = Mathf.SmoothDamp(m_Camera.orthographicSize, requiredSize, ref m_ZoomSpeed, m_DampTime);
    }

    private float FindRequiredSize()
    {
        Vector3 desiredLocalPos = transform.InverseTransformPoint(m_DesiredPosition);

        float size = 0f;

        for (int i = 0; i < m_Targets.Length; i++)
        {
            if (!m_Targets[i].gameObject.activeSelf)
                continue;

            Vector3 targetLocalPos = transform.InverseTransformPoint(m_Targets[i].position);

            Vector3 desiredPosToTarget = targetLocalPos - desiredLocalPos;

            size = Mathf.Max(size, Mathf.Abs(desiredPosToTarget.y));
            size = Mathf.Max(size, Mathf.Abs(desiredPosToTarget.x) / m_Camera.aspect);
        }

        size += m_ScreenEdgeBufer;

        size = Mathf.Max(size, m_MinSize);

        return size;
    }

    public void SetStartPositionAndSize()
    {
        FindAveragePosition();

        transform.position = m_DesiredPosition;

        m_Camera.orthographicSize = FindRequiredSize();
    }

}
