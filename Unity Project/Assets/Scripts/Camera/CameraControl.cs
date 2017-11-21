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
    public float minimumDistance = 5f;                                          //minimum distance for zoom
    public float maximumDistance = 75f;                                         //maximum distance for zoom
    public float distanceScale = 1.5f;                                          //distance zoom scale
    public bool followZone = true;                                              //checks if zone is to be followed
    
    private PlayerController[] m_Targets = new PlayerController[4];             //array of gaming objects that would be the targets for camera to adjust to
    private Camera m_Camera;                                                    //reference to the camera attached as child
    private float m_ZoomSpeed;                                                  //damps zooming, slows it down to make it less jarring
    private Vector3 m_MoveVelocity;                                             //damps camera movement and panning to avoid jarring camera movements
    private Vector3 m_DesiredPosition;                                          //position that camera is trying to reach
    private Vector3 screenPoint;                                                //variable that holds frustrum position
   
    [SerializeField]
    private GameObject m_gmObject;                                              //Instance for game Object
    private GameObject m_Zone;                                                  //Instance for game zone
    private GameManager m_gm;                                                   //Instance for Game Manager
    public GameObject indicatorLeft;                                            //Game Object left arrow
    public GameObject indicatorRight;                                           //Game Object right arrow
    public GameObject defaultCamPos;                                            //Game Object default Camera Position

    /// <summary>
    /// Awake gets initial values on Initialization
    /// </summary>
    private void Awake()
    {
        m_Camera = GetComponent<Camera>();	                                    //references camera and gets values

        m_Zone = GameObject.FindGameObjectWithTag("Zone");                      //gets zone Instance

        m_gm = m_gmObject.GetComponent<GameManager>();                          //gets values for game manager objects
    }

    /// <summary>
    /// Runs on every Update
    /// </summary>
    private void FixedUpdate()
    {
        for (int i = 0; i < 4; i++)
        {
            if (m_gm.players[i] != null)
            {
                m_Targets[i] = m_gm.players[i].GetComponentInChildren<PlayerController>();                    //checks number of active players
            }
            else
            {
                m_Targets[i] = null;
            }
        }
        
        if (!CanFindZone())                                                                                  //calls function to find if zone is in view of camera
        {
            screenPoint = m_Camera.WorldToViewportPoint(m_Zone.transform.position);                           //gets location oz zone in relation to camera frustrum
            if (screenPoint.x < 0)                                                                            //if zone is out of view and on the left
            {
                indicatorLeft.SetActive(true);
                indicatorRight.SetActive(false);
            }
            else                                                                                              //if zone is out of view and on right
            {
                indicatorLeft.SetActive(false);
                indicatorRight.SetActive(true);
            }
        }
        else                                                                                                  //if zone is in view
        {
            indicatorLeft.SetActive(false);
            indicatorRight.SetActive(false);
        }
        Move();                                                                 //calls move function to move camera

    }

    /// <summary>
    /// Function that checks if zone is in view of camera fustrum
    /// </summary>
    /// <returns>returns true if in view or false if not</returns>
    private bool CanFindZone()
    {
        screenPoint = m_Camera.WorldToViewportPoint(m_Zone.transform.position);                                        //gets location oz zone in relation to camera frustrum
        if (screenPoint.z > 0 && screenPoint.x > 0 && screenPoint.x < 1 && screenPoint.y > 0 && screenPoint.y < 1)     //checks if location of zone is within frustrum
            return true;
        else
            return false;
    }

    /// <summary>
    /// Moves Camera to desired position
    /// </summary>
    private void Move()
    {
        m_DesiredPosition = FindAveragePosition();                                //sets desired position to be result of function FindAveragePosition

        float distance = FindDistance();                                          //Function called to find Distance between players/zone

        if (distance <= minimumDistance)                                          //Checks if distance is under minimum distance
        {
            distance = minimumDistance;                                           //sets distance to minimum distance if distance is under minumum
        }
        else if (distance >= maximumDistance)                                     //Checks if distance is over maximum distance
        {
            distance = maximumDistance;                                           //sets distance to maximum distance if distance is over maximum
        }
        
        m_DesiredPosition += -m_Camera.transform.forward * distance * distanceScale; //sets zoom distance based on distance value and distance scale

        m_Camera.transform.position = Vector3.SmoothDamp(m_Camera.transform.position, m_DesiredPosition, ref m_MoveVelocity, m_DampTime); //transforms camera position based on desired position and movement velocity based on MoveVelocity and DampTime
    }

    /// <summary>
    /// Finds Average position between players and checks if there are any players left on the boat
    /// </summary>
    /// <returns> average position of players or zoom out position </returns>
    private Vector3 FindAveragePosition()
    {
        Vector3 averagePos = new Vector3();                                       //Vector pos 
        int numTargets = 0;                                                       //number of targets

        for (int i = 0; i < m_Targets.Length; i++)                                //loops through max number of players
        {
            if (m_Targets[i] == null || !m_Targets[i].isAlive)                    //checks if player is alive
                continue;

            //averagePos += m_Targets[i].gameObject.transform.position;             //if player is alive, get player position
            numTargets++;                                                         //add to numTarget counter
        }

        //if (numTargets <= 0)                                                      //checks if there are no active players
        //    followZone = true;                                                    //sets followzone to true
        //else
        //    followZone = false;                                                   //else set followzone to false


        //// Add the defaultCamPos to the avg position if followzone is true
        //if (followZone)
        //{
        //    //averagePos += (m_Zone.transform.position);                          //previous iteration follows zone if there are no players active
        //    averagePos += defaultCamPos.transform.position;                       //now it follows a blank game objects position called defaultCamPos
        //    numTargets++;
        //}

        //if (numTargets > 0)                                                       //quick checker to ensure that numTargets is not zero, otherwise camera script crashes
        //    averagePos /= numTargets;

        if (numTargets == 1)
        {
            for (int i = 0; i < m_Targets.Length; i++)                                //loops through max number of players
            {
                if (m_Targets[i] == null || !m_Targets[i].isAlive)                    //checks if player is alive
                    continue;

                return m_Targets[i].gameObject.transform.position;
            }
        }

        if (numTargets == 0)
        {
            return defaultCamPos.transform.position;
        }

        float distance = 0f;                                                                               //distance to return
        float currentDistance = 0f;                                                                        //distance current

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

                currentDistance = Vector3.Distance(playerA.transform.position, playerB.transform.position);
                if (currentDistance > distance)
                {
                    distance = currentDistance;
                    averagePos = (playerA.transform.position + playerB.transform.position) / 2; // Calculate the pos 
                }
            }

            // Check the distance to the zone if we are following the zone
            if (followZone)
            {
                currentDistance = Vector3.Distance(playerA.transform.position, m_Zone.transform.position);
                if (currentDistance > distance)
                {
                    distance = currentDistance;
                    averagePos = (playerA.transform.position + m_Zone.transform.position) / 2; // Calculate the pos 
                }
            }

        }

        return averagePos;                                                        //returns averagePos
    }

    /// <summary>
    /// Function Finds Distance between targets
    /// </summary>
    /// <returns> returns biggest distance between players or 0 if all players are notActive </returns>
    public float FindDistance()
    {
        float distance = 0f;                                                                               //distance to return
        float currentDistance = 0f;                                                                        //distance current

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

                currentDistance = Vector3.Distance(playerA.transform.position, playerB.transform.position);
                if (currentDistance > distance)
                {
                    distance = currentDistance;
                }
            }

            // Check the distance to the zone if we are following the zone
            if (followZone)
            {
                currentDistance = Vector3.Distance(playerA.transform.position, m_Zone.transform.position);
                if (currentDistance > distance)
                {
                    distance = currentDistance;
                }
            }

        }

        // Return the max distance we found
        return distance;
    }

    public void SetStartPositionAndSize()
    {
        FindAveragePosition();

        transform.position = m_DesiredPosition;
    }

}
