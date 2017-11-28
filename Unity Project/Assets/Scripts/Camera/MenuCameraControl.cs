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
/// Menu Camera Control Class
/// </summary>
public class MenuCameraControl : MonoBehaviour
{
    [SerializeField]                                                                                                             //Public Objects set on the Scene for the camera to shift towards when a certain action is done
    Transform mainMenuTransfrom;                                                                                                 //Main Menu Game Object
    [SerializeField]                                                                                                             //
    Transform characterSelectTransform;                                                                                          //Character Select Object
    [SerializeField]                                                                                                             //
    Transform optionsTransform;                                                                                                  //Options Object
    [SerializeField]                                                                                                             //
    Transform creditsTransform;                                                                                                  //Credits Object
                                                                                                                                 
    [SerializeField]                                                                                                             //Public Variable for Designers to set Camera Speed
    float speed;                                                                                                                 //
                                                                                                                                 
    Transform targetTransform;                                                                                                   //Variable that will be the camera position
    Transform previousTransform;                                                                                                 //Variable that will be camera previous position
                                                                                                                                 
    Camera camera;                                                                                                               //Variable for Scene Camera
                                                                                                                                 
    private float timeSinceStart;                                                                                                //Variable for Time
    private float journeyLength;                                                                                                 //Varaible for Camera Distance Travel
                                                                                                                                 
    [HideInInspector]                                                                                                            //Variable hidden from Inspector that checks if camera is moving, used for debugging
    public bool moving;                                                                                                          //

    ///On Game Boot                                                                                                                             
    void Awake()                                                                                                                 
    {                                                                                                                            
        camera = GetComponent<Camera>();                                                                                         //camera Variable gets components from Camera Game Object
        targetTransform = mainMenuTransfrom;                                                                                     //moves the camera to location of Main Menu Transform
    }                                                                                                                            
	
    /// <summary>
    /// On late Update
    /// </summary>
	void LateUpdate()                                                                                                            
    {                                                                                                                            
        timeSinceStart += Time.deltaTime;                                                                                        //time is recorded in timeSinceStart
        float distCovered = timeSinceStart * speed;                                                                              //distCovered varaible created which will be the distance the camera moved from previous location
        float fracJourney = 0;                                                                                                   //variable fracJourney created, which will be the curve of the Lerp

        if (journeyLength != 0)                                                                                                  //Ensures that Journey Length doesn't divide by zero
        {                                                                                                                        
            fracJourney = distCovered / journeyLength;                                                                           //Lerp curve is measured with the distCovered / journeyLength
        }                                                                                                                        
                                                                                                                                 
        
        camera.transform.position = Vector3.Lerp(previousTransform.position, targetTransform.position, fracJourney);             //Lerp Position
        
        camera.transform.rotation = Quaternion.Slerp(previousTransform.rotation, targetTransform.rotation, fracJourney);         //Lerp Rotation
                                                                                                                                 
                                                                                                                                 
        if (Vector3.Distance(camera.transform.position, targetTransform.position) < 0.1f)                                        //If Camera is close to destination
        {                                                                                                                        
            moving = false;                                                                                                      //set movement to false
        }                                                                                                                        
    }                                                                                                                            
    
    /// <summary>
    /// Function to Move Camera to Main Menu
    /// </summary>
    public void MoveToMainMenu()                                                                                                 
    {                                                                                                                            
        moving = true;                                                                                                           //set movement to True
        MoveTo(mainMenuTransfrom);                                                                                               //Calls MoveTo Function with target position as mainMenuTransform
    }

    /// <summary>
    /// Function to Move Camera to CharacterSelection position
    /// </summary>
    public void MoveToCharacterSelection()                                                                                       
    {                                                                                                                            
        moving = true;                                                                                                           //set movement to true
        MoveTo(characterSelectTransform);                                                                                        //Calls MoveTo Function with target position as characterSelectTransform
    }                                                                                                                            

    /// <summary>
    /// Function to move Camera to Options location
    /// </summary>
    public void MoveToOptions()                                                                                                  
    {                                                                                                                            
        moving = true;                                                                                                           //set movement to true
        MoveTo(optionsTransform);                                                                                                //Calls MoveTo Function with target position as optionsTransform
    }

    /// <summary>
    /// Function to move Camera to Credits location
    /// </summary>
    public void MoveToCredits()                                                                                                  
    {                                                                                                                            
        moving = true;                                                                                                           //set movement to true
        MoveTo(creditsTransform);                                                                                                //Calls MoveTo Function with target position as creditsTransform
    }

    /// <summary>
    /// Function to move the camera
    /// </summary>
    /// <param name="target"> target is the next location position </param>
    void MoveTo(Transform target)                                                                                                
    {                                                                                                                            
        moving = true;                                                                                                           //set movement varaible to true
                                                                                                                                 
        previousTransform = targetTransform;                                                                                     //previousTransform is set as targetTransform
        targetTransform = target;                                                                                                //targetTransform is set as the target position
                                                                                                                                 
        timeSinceStart = 0;                                                                                                      //time is set again to zero
        journeyLength = Vector3.Distance(camera.transform.position, targetTransform.position);                                   //journeyLength is set as the distance between the camera position and target position
    }
}
