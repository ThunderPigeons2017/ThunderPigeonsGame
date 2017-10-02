using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    int playerCount;

    float[] scores = new float[4];

    [SerializeField]
    GameObject zone;
    ZoneControl zoneControl;

    // Keep a list of each player in the game
    List<GameObject> players = new List<GameObject>();

    [SerializeField]
    GameObject playerPrefab;

    void Awake ()
    {
        zoneControl = zone.GetComponent<ZoneControl>();

        // Spawn players
        SpawnPlayer(0, 0);
        SpawnPlayer(1, 1);

        // Set the player count
        playerCount = players.Count;

        // 
	}

    void Update ()
    {
        // Every second give the players in the zoneControl.playersInZone a point

        foreach ( GameObject player in zoneControl.playersInZone )
        {
            scores[player.GetComponent<PlayerController>().playerNumber] += Time.deltaTime;
        }
    }

    void SpawnPlayer (int playerNumber, int controllerNumber)
    {
        GameObject newPlayer = Instantiate(playerPrefab);
        // Add to the players list
        players.Add(newPlayer);
        // Set each players number
        //newPlayer.GetComponent<PlayerController>().playerNumber = playerNumber;
    }
}
