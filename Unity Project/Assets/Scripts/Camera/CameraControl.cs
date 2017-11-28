/***********************************************
 * Rummy Robots
 * by Thunder Pidgeons
 * 
 * In-game Camera Control Script
 ***********************************************/
//Codes to include basic Unity Functions
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// CameraControl Class Begins
/// </summary>
public class CameraControl : MonoBehaviour
{

    public float m_DampTime = 0.2f;                                                                                    //approximate time for the camera to move to new location
    public float minimumDistance = 5f;                                                                                 //minimum distance for zoom
    public float maximumDistance = 75f;                                                                                //maximum distance for zoom
    public float distanceScale = 1.5f;                                                                                 //distance zoom scale
    public bool followZone = true;                                                                                     //checks if zone is to be followed
    
    private PlayerController[] m_Targets = new PlayerController[4];                                                    //array of gaming objects that would be the targets for camera to adjust to
    private Camera m_Camera;                                                                                           //reference to the camera attached as child
    private float m_ZoomSpeed;                                                                                         //damps zooming, slows it down to make it less jarring
    private Vector3 m_MoveVelocity;                                                                                    //damps camera movement and panning to avoid jarring camera movements
    private Vector3 m_DesiredPosition;                                                                                 //position that camera is trying to reach
    private Vector3 screenPoint;                                                                                       //variable that holds frustrum position
   
    [SerializeField]
    private GameObject m_gmObject;                                                                                     //Instance for game Object
    private GameObject m_Zone;                                                                                         //Instance for game zone
    private GameManager m_gm;                                                                                          //Instance for Game Manager
    public GameObject indicatorLeft;                                                                                   //Game Object left arrow
    public GameObject indicatorRight;                                                                                  //Game Object right arrow
    public GameObject defaultCamPos;                                                                                   //Game Object default Camera Position

    /// <summary>
    /// Awake gets initial values on Initialization
    /// </summary>
    private void Awake()
    {
        m_Camera = GetComponent<Camera>();	                                                                           //references camera and gets values

        m_Zone = GameObject.FindGameObjectWithTag("Zone");                                                             //gets zone Instance

        m_gm = m_gmObject.GetComponent<GameManager>();                                                                 //gets values for game manager objects
    }

    /// <summary>
    /// Runs on every Update
    /// </summary>
    private void FixedUpdate()
    {
        for (int i = 0; i < 4; i++)                                                                                    //Loop that runs through and checks for active players
        {
            if (m_gm.players[i] != null)                                                                               //If there is a player present
            {
                m_Targets[i] = m_gm.players[i].GetComponentInChildren<PlayerController>();                             //Sets the player as a target from the game manager
            }
            else
            {
                m_Targets[i] = null;                                                                                   //if there is no player found with the associated number, then set target as "null"
            }
        }
        
        if (!CanFindZone())                                                                                            //calls function to find if zone is in view of camera if not then
        {
            screenPoint = m_Camera.WorldToViewportPoint(m_Zone.transform.position);                                    //gets location oz zone in relation to camera frustrum
            if (screenPoint.x < 0)                                                                                     //if zone is out of view and on the left
            {
                indicatorLeft.SetActive(true);                                                                         //set left indicator to visible
                indicatorRight.SetActive(false);                                                                       //and sets right indicator to hidden
            }
            else                                                                                                       //if zone is out of view and on right
            {
                indicatorLeft.SetActive(false);                                                                        //set left indicator to hidden
                indicatorRight.SetActive(true);                                                                        //and sets right indicator to visible
            }
        }
        else                                                                                                           //if zone is in view
        {
            indicatorRight.SetActive(false);                                                                           //sets left indicator as hidden
            indicatorLeft.SetActive(false);                                                                            //sets right indicator as hidden
        }

        Move();                                                                                                        //calls move function to move camera

    }

    /// <summary>
    /// Function that checks if zone is in view of camera fustrum
    /// </summary>
    /// <returns>returns true if in view or false if not</returns>
    private bool CanFindZone()
    {
        screenPoint = m_Camera.WorldToViewportPoint(m_Zone.transform.position);                                        //gets location oz zone in relation to camera frustrum
        if (screenPoint.z > 0 && screenPoint.x > 0 && screenPoint.x < 1 && screenPoint.y > 0 && screenPoint.y < 1)     //checks if location of zone is within frustrum
            return true;                                                                                               //returns true if zone is in view of camera
        else
            return false;                                                                                              //returns false if zone is in camera view
    }

    /// <summary>
    /// Moves Camera to desired position
    /// </summary>
    private void Move()
    {
        m_DesiredPosition = FindAveragePosition();                                                                     //sets desired position to be result of function FindAveragePosition

        float distance = FindDistance();                                                                               //Function called to find Distance between players/zone

        if (distance <= minimumDistance)                                                                               //Checks if distance is under minimum distance
        {
            distance = minimumDistance;                                                                                //sets distance to minimum distance if distance is under minumum
        }
        else if (distance >= maximumDistance)                                                                          //Checks if distance is over maximum distance
        {
            distance = maximumDistance;                                                                                //sets distance to maximum distance if distance is over maximum
        }
        
        m_DesiredPosition += -m_Camera.transform.forward * distance * distanceScale;                                   //sets zoom distance based on distance value and distance scale

        m_Camera.transform.position = Vector3.SmoothDamp(m_Camera.transform.position, m_DesiredPosition, ref m_MoveVelocity, m_DampTime); //transforms camera position based on desired position and movement velocity based on MoveVelocity and DampTime
    }

    /// <summary>
    /// Finds Average position between players and checks if there are any players left on the boat
    /// </summary>
    /// <returns> average position of players or zoom out position </returns>
    private Vector3 FindAveragePosition()
    {
        if (m_gm.gameWon)                                                                                              //if the game has been won
        {
            if (m_Targets[m_gm.winningPlayerNumber - 1].isAlive)                                                       //only if that player is alive
            {
                return m_Targets[m_gm.winningPlayerNumber - 1].transform.position;                                     //return the winning players position
            }
        }


        Vector3 averagePos = new Vector3();                                                                            //Vector pos 
        int numTargets = 0;                                                                                            //number of targets

        for (int i = 0; i < m_Targets.Length; i++)                                                                     //loops through max number of players
        {
            if (m_Targets[i] == null || !m_Targets[i].isAlive)                                                         //checks if player is alive
                continue;                                                                                              

            numTargets++;                                                                                              //add to numTarget counter
        }
        
        if (numTargets == 1)                                                                                           //if there is a player present
        {
            for (int i = 0; i < m_Targets.Length; i++)                                                                 //loops through max number of players
            {                                                                                                          
                if (m_Targets[i] == null || !m_Targets[i].isAlive)                                                     //checks if player is alive
                    continue;

                return m_Targets[i].gameObject.transform.position;                                                     //returns the player position
            }
        }

        if (numTargets == 0)                                                                                           //if there are no active players in the field
        {
            return defaultCamPos.transform.position;                                                                   //return default cam position as the position to move the camera to
        }

        float distance = 0f;                                                                                           //distance to return
        float currentDistance = 0f;                                                                                    //distance current
                                                                                                                       
        for (int i = 0; i < m_Targets.Length; i++)                                                                     //Loops through  and checks for players
        {                                                                                                              
            if (m_Targets[i] == null || !m_Targets[i].isAlive)                                                         //if target is alive continue
                continue;                                                                                              
                                                                                                                       
            PlayerController playerA = m_Targets[i];                                                                   //Target is set as playerA
                                                                                                                       
            for (int j = i + 1; j < m_Targets.Length; j++)                                                             //Loops through another set of players except for the previous one
            {                                                                                                          
                if (m_Targets[j] == null || !m_Targets[j].isAlive)                                                     //if player is active
                    continue;                                                                                          //continue
                                                                                                                       
                PlayerController playerB = m_Targets[j];                                                               //sets that player as playerB
                                                                                                                       
                currentDistance = Vector3.Distance(playerA.transform.position, playerB.transform.position);            //Calculate distance between playerA and PlayerB

                if (currentDistance > distance)                                                                        //if the new distance is greater than the previous distance
                {                                                                                                      
                    distance = currentDistance;                                                                        //set that distance as the new distance
                    averagePos = (playerA.transform.position + playerB.transform.position) / 2;                        //Calculate the pos as the distance between the two players
                }                                                                                                      
            }                                                                                                          
                                                                                                                       
            if (followZone)                                                                                            //Check if we are following the zone
            {                                                                                                          
                currentDistance = Vector3.Distance(playerA.transform.position, m_Zone.transform.position);             //Calculate currentDistance between player position and the zone

                if (currentDistance > distance)                                                                        //if currentDistance is greater than the previous distance
                {                                                                                                      
                    distance = currentDistance;                                                                        //set distance as the currentDistance
                    averagePos = (playerA.transform.position + m_Zone.transform.position) / 2;                         //Calculate the pos
                }                                                                                                      
            }                                                                                                          
                                                                                                                       
        }                                                                                                              
                                                                                                                       
        return averagePos;                                                                                             //returns averagePos
    }                                                                                                                  

    /// <summary>
    /// Function Finds Distance between targets
    /// </summary>
    /// <returns> returns biggest distance between players or 0 if all players are notActive </returns>
    public float FindDistance()
    {
        if (m_gm.gameWon)                                                                                              //if the game has been won
        {
            if (m_Targets[m_gm.winningPlayerNumber - 1].isAlive)                                                       //only if that player is alive
            {
                return 0;                                                                                              //return 0 so we use the minimum distance
            }
        }

        float distance = 0f;                                                                                           //distance to return
        float currentDistance = 0f;                                                                                    //distance current

        for (int i = 0; i < m_Targets.Length; i++)                                                                     //Loops through players 
        {                                                                                                              
            if (m_Targets[i] == null || !m_Targets[i].isAlive)                                                         //If target is an active player
                continue;                                                                                              
                                                                                                                       
            PlayerController playerA = m_Targets[i];                                                                   //PlayerA is set as a target
                                                                                                                       
            for (int j = i + 1; j < m_Targets.Length; j++)                                                             //Loops through and compares distance between other players
            {                                                                                                          
                if (m_Targets[j] == null || !m_Targets[j].isAlive)                                                     //checks if other players are active
                    continue;                                                                                          
                                                                                                                       
                PlayerController playerB = m_Targets[j];                                                               //PlayerB is set as other target for comparison
                                                                                                                       
                currentDistance = Vector3.Distance(playerA.transform.position, playerB.transform.position);            //set currentDistance as the distance between PlayerA and PlayerB

                if (currentDistance > distance)                                                                        //if currentDistance is greater than the distance
                {                                                                                                      
                    distance = currentDistance;                                                                        //then set the distance as the currentDistance
                }                                                                                                      
            }

            if (followZone)                                                                                            //Check if we are following the zone
            {                                                                                                          
                currentDistance = Vector3.Distance(playerA.transform.position, m_Zone.transform.position);             //if following zone, set currentDistance as the distance between PlayerA and the zone
                if (currentDistance > distance)                                                                        //check if currentDistance is greater than distance
                {                                                                                                      
                    distance = currentDistance;                                                                        //if yes then set distance as currentDistance
                }                                                                                                      
            }                                                                                                          
        }
        return distance;                                                                                               // Return the max distance we found
    }                                                                                                                  

    /// <summary>
    /// Function to Set Start Position And Size
    /// </summary>
    public void SetStartPositionAndSize()                                                                              
    {                                                                                                                  
        FindAveragePosition();                                                                                         //Call FindAveragePosition Function
                                                                                                                       
        transform.position = m_DesiredPosition;                                                                        //set Camera position to the desired position
    }                                                                                                                  
}                                                                                                                      
