using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    int playerCount;

    float[] scores = new float[4];

    [SerializeField]
    GameObject zone;
    ZoneControl zoneControl;

    // Keep a list of each player in the game
    public GameObject[] players = new GameObject[4];

    [SerializeField]
    GameObject playerPrefab;

    [SerializeField] // Each players score text
    Text player1Score, player2Score, player3Score, player4Score;

    [SerializeField] // Each players spawn pos
    Transform player1Spawn, player2Spawn, player3Spawn, player4Spawn;

    [SerializeField]
    bool allowContest;

    void Awake ()
    {
        zoneControl = zone.GetComponent<ZoneControl>();

        // Spawn players
        SpawnPlayer(1);
	}

    void Update ()
    {
        // Every second give the players in the zoneControl.playersInZone a point

        if (allowContest && zoneControl.playersInZone.Count == 1)
        {
            foreach ( GameObject player in zoneControl.playersInZone )
            {
                scores[player.GetComponent<PlayerController>().playerNumber - 1] += Time.deltaTime;
            }
        }
        else if (!allowContest)
        {
            foreach (GameObject player in zoneControl.playersInZone)
            {
                scores[player.GetComponent<PlayerController>().playerNumber - 1] += Time.deltaTime;
            }
        }
        // After the scores are added 
        UpdateScoreBoard();

        // Temp player spawning
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            SpawnPlayer(1);
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            SpawnPlayer(2);
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            SpawnPlayer(3);
        }
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            SpawnPlayer(4);
        }
    }

    void SpawnPlayer(int playerNumber)
    {
        // If we have a player in that spot, delete it
        if (players[playerNumber - 1] != null)
        {
            Destroy(players[playerNumber - 1]);
            players[playerNumber - 1] = null;
        }

        GameObject newPlayer = null;

        // Switch on the different players to spawn them in different spots
        switch (playerNumber)
        {
            case 1:
                newPlayer = Instantiate(playerPrefab, player1Spawn.position, player1Spawn.rotation) as GameObject;
                newPlayer.name = "Player 1";
                break;
            case 2:
                newPlayer = Instantiate(playerPrefab, player2Spawn.position, player2Spawn.rotation) as GameObject;
                newPlayer.name = "Player 2";
                break;
            case 3:
                newPlayer = Instantiate(playerPrefab, player3Spawn.position, player3Spawn.rotation) as GameObject;
                newPlayer.name = "Player 3";
                break;
            case 4:
                newPlayer = Instantiate(playerPrefab, player4Spawn.position, player4Spawn.rotation) as GameObject;
                newPlayer.name = "Player 4";
                break;
            default:
                break;
        }

        // Add to the player array in the correct spot
        players[playerNumber - 1] = newPlayer;

        PlayerController newPlayerController = newPlayer.GetComponentInChildren<PlayerController>();
        // Set each players number
        newPlayerController.playerNumber = playerNumber;
        // Set the controller number
        newPlayerController.controller = (XboxCtrlrInput.XboxController)playerNumber;

        LookRotation newLookRotation = newPlayer.GetComponentInChildren<LookRotation>();
        newLookRotation.StartUp();
    }

    void UpdateScoreBoard()
    {
        player1Score.text = ((int)scores[0]).ToString();
        player2Score.text = ((int)scores[1]).ToString();
        player3Score.text = ((int)scores[2]).ToString();
        player4Score.text = ((int)scores[3]).ToString();
    }
}
