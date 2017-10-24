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
    }

    public void MoveToMainMenu()
    {
        MoveTo(mainMenuTransfrom);
    }

    public void MoveToCharacterSelection()
    {
        MoveTo(characterSelectTransform);
    }

    public void MoveToOptions()
    {
        MoveTo(optionsTransform);
    }

    void MoveTo(Transform target)
    {
        previousTransform = targetTransform;
        targetTransform = target;

        timeSinceStart = 0;
        journeyLength = Vector3.Distance(camera.transform.position, targetTransform.position);
    }
}
