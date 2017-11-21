using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuCameraControl : MonoBehaviour
{
    [SerializeField]
    Transform mainMenuTransfrom;
    [SerializeField]
    Transform characterSelectTransform;
    [SerializeField]
    Transform optionsTransform;
    
    [SerializeField]
    float speed;

    Transform targetTransform;
    Transform previousTransform;

    Camera camera;

    private float timeSinceStart;
    private float journeyLength;

    [HideInInspector]
    public bool moving;

    void Awake()
    {
        camera = GetComponent<Camera>();
        targetTransform = mainMenuTransfrom;
    }
	
	void LateUpdate()
    {
        timeSinceStart += Time.deltaTime;
        float distCovered = timeSinceStart * speed;
        float fracJourney = 0;
        if (journeyLength != 0) // Do not divide by zero
        {
            fracJourney = distCovered / journeyLength;
        }

        // Lerp position
        camera.transform.position = Vector3.Lerp(previousTransform.position, targetTransform.position, fracJourney);
        // Lerp rotation
        camera.transform.rotation = Quaternion.Slerp(previousTransform.rotation, targetTransform.rotation, fracJourney);

        // If we get close the the target 
        if (Vector3.Distance(camera.transform.position, targetTransform.position) < 0.1f)
        {
            moving = false;
        }
    }

    public void MoveToMainMenu()
    {
        moving = true;
        MoveTo(mainMenuTransfrom);
    }

    public void MoveToCharacterSelection()
    {
        moving = true;
        MoveTo(characterSelectTransform);
    }

    public void MoveToOptions()
    {
        moving = true;
        MoveTo(optionsTransform);
    }

    void MoveTo(Transform target)
    {
        moving = true;

        previousTransform = targetTransform;
        targetTransform = target;

        timeSinceStart = 0;
        journeyLength = Vector3.Distance(camera.transform.position, targetTransform.position);
    }
}
