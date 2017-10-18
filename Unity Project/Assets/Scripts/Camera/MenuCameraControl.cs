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
    float speed;

    Transform targetTransform;

    Camera camera;

    private float timeSinceStart;
    private float journeyLength;

    void Awake()
    {
        camera = GetComponent<Camera>();
        targetTransform = mainMenuTransfrom;
    }
	
	void FixedUpdate()
    {
        timeSinceStart += Time.fixedDeltaTime;
        float distCovered = timeSinceStart * speed;
        float fracJourney = 0;
        if (journeyLength != 0) // Do not divide by zero
        {
            fracJourney = distCovered / journeyLength;
        }

        // Lerp position
        camera.transform.position = Vector3.Lerp(camera.transform.position, targetTransform.position, fracJourney);
        // Lerp rotation
        camera.transform.rotation = Quaternion.Slerp(camera.transform.rotation, targetTransform.rotation, fracJourney);
    }

    public void MoveToMainMenu()
    {
        targetTransform = mainMenuTransfrom;

        timeSinceStart = 0;
        journeyLength = Vector3.Distance(camera.transform.position, targetTransform.position);
    }

    public void MoveToCharacterSelection()
    {
        targetTransform = characterSelectTransform;

        timeSinceStart = 0;
        journeyLength = Vector3.Distance(camera.transform.position, targetTransform.position);
    }
}
