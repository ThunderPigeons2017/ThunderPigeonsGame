using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZoneManager : MonoBehaviour
{
    [SerializeField]
    int pointsBeforeMoving = 10;

    [SerializeField]
    List<Transform> zonePostitions;

    int currentZonePosIndex = -1;

    GameObject zone;

    float pointsLeft = 0;

    void Awake()
    {
        zone = GameObject.FindGameObjectWithTag("Zone");
    }

    void Start()
    {
        pointsLeft = pointsBeforeMoving;
    }

    public void PointsGiven(float points)
    {
        pointsLeft -= points;

        if (pointsLeft <= 0)
        {
            MoveZone();
            pointsLeft = pointsBeforeMoving;
        }
    }

    void MoveZone()
    {
        if (zonePostitions.Count == 0) // If we have no positions don't move
            return;
        if (zonePostitions.Count == 1) // If we have 1 position don't move
            return;

        int newZonePosIndex = Random.Range(0, zonePostitions.Count);
        // Make sure we chose a new position
        while(newZonePosIndex == currentZonePosIndex)
        {
            newZonePosIndex = Random.Range(0, zonePostitions.Count);
            Debug.Log(newZonePosIndex);
        }

        currentZonePosIndex = newZonePosIndex; // Set the current index

        zone.transform.position = zonePostitions[currentZonePosIndex].position; // Move the zone to that position
    }
}
