using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuCameraControl : MonoBehaviour
{
    [SerializeField]
    Transform startTransfrom;
    [SerializeField]
    Transform characterSelectTransform;
    [SerializeField]
    float speed;

    Transform targetTransform;

    Camera camera;

    private float startTime;
    private float journeyLength;

    void Awake()
    {
        camera = GetComponent<Camera>();
        targetTransform = startTransfrom;
	}
	
	void LateUpdate()
    {
        float distCovered = (Time.time - startTime) * speed;
        float fracJourney = 0;
        if (journeyLength != 0) // Do not divide by zero
        {
            fracJourney = distCovered / journeyLength;
        }

        // Lerp position
        camera.transform.position = Vector3.Lerp(camera.transform.position, targetTransform.position, fracJourney);
        // Lerp rotation
        camera.transform.rotation = Quaternion.Lerp(camera.transform.rotation, targetTransform.rotation, fracJourney);
    }

    public void MoveToStart()
    {
        targetTransform = startTransfrom;

        startTime = Time.time;
        journeyLength = Vector3.Distance(camera.transform.position, targetTransform.position);
    }

    public void MoveToCharacterSelection()
    {
        targetTransform = characterSelectTransform;

        startTime = Time.time;
        journeyLength = Vector3.Distance(camera.transform.position, targetTransform.position);
    }
}
