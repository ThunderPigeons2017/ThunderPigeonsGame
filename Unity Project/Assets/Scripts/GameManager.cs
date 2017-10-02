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
        SpawnPlayer(1);
        SpawnPlayer(2);
        SpawnPlayer(3);
        SpawnPlayer(4);

        // Set the player count
        playerCount = players.Count;

        // 
	}

    void Update ()
    {
        // Every second give the players in the zoneControl.playersInZone a point

        foreach ( GameObject player in zoneControl.playersInZone )
        {
            scores[player.GetComponent<PlayerController>().playerNumber - 1] += Time.deltaTime;
        }
    }

    void SpawnPlayer(int playerNumber)
    {
        GameObject newPlayer;
        newPlayer = Instantiate(playerPrefab, Vector3.zero, Quaternion.identity) as GameObject;

        // Add to the players list
        players.Add(newPlayer);

        PlayerController newPlayerController = newPlayer.GetComponentInChildren<PlayerController>();
        // Set each players number
        newPlayerController.playerNumber = playerNumber;
        // Set the controller number
        newPlayerController.controller = (XboxCtrlrInput.XboxController)playerNumber;

        LookRotation newLookRotation = newPlayer.GetComponentInChildren<LookRotation>();
        newLookRotation.StartUp();
    }
}
