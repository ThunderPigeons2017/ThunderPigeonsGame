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
    List<GameObject> players = new List<GameObject>();

    [SerializeField]
    GameObject playerPrefab;

    [SerializeField]
    Text player1Score;
    [SerializeField]
    Text player2Score;
    [SerializeField]
    Text player3Score;
    [SerializeField]
    Text player4Score;

    [SerializeField]
    bool allowContest;

    void Awake ()
    {
        zoneControl = zone.GetComponent<ZoneControl>();

        // Spawn players
        SpawnPlayer(1);

        // Set the player count
        playerCount = players.Count;
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

    void UpdateScoreBoard()
    {
        player1Score.text = ((int)scores[0]).ToString();
        player2Score.text = ((int)scores[1]).ToString();
        player3Score.text = ((int)scores[2]).ToString();
        player4Score.text = ((int)scores[3]).ToString();
    }
}
