using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZoneControl : MonoBehaviour
{
    public List<GameObject> playersInZone;

	void Start ()
    {
        playersInZone = new List<GameObject>();
	}

    private void OnTriggerEnter(Collider other)
    {
        // Add the player to the list
        if (other.tag == "Player")
        {
            playersInZone.Add(other.gameObject);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // Remove the players from the list
        if (other.tag == "Player")
        {
            playersInZone.Remove(other.gameObject);
        }
    }
}
