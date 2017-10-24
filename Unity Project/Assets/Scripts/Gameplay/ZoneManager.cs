using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZoneManager : MonoBehaviour
{
    [SerializeField]
    List<Transform> zonePostitions;

    int currentZonePosIndex = -1;

    GameObject zone;

    void Awake()
    {
        zone = GameObject.FindGameObjectWithTag("Zone");
    }

    public void MoveZone()
    {
        if (zonePostitions.Count == 0) // If we have no positions don't move
            return;
        if (zonePostitions.Count == 1) // If we have 1 positions don't move
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
