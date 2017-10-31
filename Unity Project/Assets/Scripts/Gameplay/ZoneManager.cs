using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZoneManager : MonoBehaviour
{
    [SerializeField]
    int pointsBeforeMoving = 10;

    bool zoneInCentre = true;

    [SerializeField]
    Transform zoneCentre;
    [SerializeField]
    Transform zoneFront;
    [SerializeField]
    Transform zoneBack;

    int currentZonePosIndex = -1;

    GameObject zone;

    float pointsLeft = 0;

    public enum ZonePosition
    {
        Centre,
        Front,
        Back
    }

    public ZonePosition zonePosition;

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

        switch (zonePosition)
        {
            case ZonePosition.Centre:
                if (Random.Range(0, 2) == 0)
                {
                    zone.transform.position = zoneFront.position; // Move the zone to front
                }
                else
                {
                    zone.transform.position = zoneBack.position; // Move the zone to back
                }
                break;
            case ZonePosition.Front:
                if (Random.Range(0, 2) == 0)
                {
                    zone.transform.position = zoneCentre.position; // Move the zone to centre
                }
                else
                {
                    zone.transform.position = zoneBack.position; // Move the zone to back
                }
                break;
            case ZonePosition.Back:
                if (Random.Range(0, 2) == 0)
                {
                    zone.transform.position = zoneFront.position; // Move the zone to front
                }
                else
                {
                    zone.transform.position = zoneCentre.position; // Move the zone to centre
                }
                break;
            default:
                break;
        }

        //if (zonePostitions.Count == 0) // If we have no positions don't move
        //    return;
        //if (zonePostitions.Count == 1) // If we have 1 position don't move
        //    return;

        //int newZonePosIndex = Random.Range(0, zonePostitions.Count);
        //// Make sure we chose a new position
        //while(newZonePosIndex == currentZonePosIndex)
        //{
        //    newZonePosIndex = Random.Range(0, zonePostitions.Count);
        //    Debug.Log(newZonePosIndex);
        //}

        //currentZonePosIndex = newZonePosIndex; // Set the current index

        //zone.transform.position = zonePostitions[currentZonePosIndex].position; // Move the zone to that position
    }
}
