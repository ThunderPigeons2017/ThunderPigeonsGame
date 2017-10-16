using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    float[] scores = new float[4];

    [SerializeField]
    int winScore;

    [SerializeField]
    bool allowContest = false;

    [SerializeField]
    float startTime;
    float timer;

    [SerializeField]
    float deathYLevel = -3f;

    [SerializeField]
    GameObject zone;
    ZoneControl zoneControl;

    // Keep a list of each player in the game
    public GameObject[] players = new GameObject[4];

    [Header("Prefabs:")]
    [SerializeField]
    GameObject playerPrefab;

    [SerializeField] // Each players score text
    Text player1Score, player2Score, player3Score, player4Score;

    [SerializeField]
    Text timeText;

    [SerializeField] // Each players spawn pos
    Transform player1Spawn, player2Spawn, player3Spawn, player4Spawn;

    [Header("")]
    [SerializeField]
    Text winMessageText;

    [SerializeField]
    string winMessageString;

    PlayerColourPicker playerColourPicker;

    int winningPlayerNumber = 0;

    void Awake ()
    {
        zoneControl = zone.GetComponent<ZoneControl>();
        playerColourPicker = GetComponent<PlayerColourPicker>();

        timer = startTime;

        // Spawn players
        SpawnPlayer(1);

        winMessageText.enabled = false;
	}

    void Update ()
    {
        for (int i = 0; i < 4; i++)
        {
            if (players[i] != null)
            {
                PlayerController playerController = players[i].GetComponentInChildren<PlayerController>();
                if (playerController.transform.position.y <= deathYLevel)
                {
                    playerController.isAlive = false;
                }

                if (playerController.isAlive == false)
                {
                    SpawnPlayer(playerController.playerNumber);
                }
            }
        }


        UpdateTimer();

        GivePoints();

        // After the scores are added 
        UpdateScoreBoard();

        CheckForWinningPlayer();

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
                break;
            case 2:
                newPlayer = Instantiate(playerPrefab, player2Spawn.position, player2Spawn.rotation) as GameObject;
                break;
            case 3:
                newPlayer = Instantiate(playerPrefab, player3Spawn.position, player3Spawn.rotation) as GameObject;
                break;
            case 4:
                newPlayer = Instantiate(playerPrefab, player4Spawn.position, player4Spawn.rotation) as GameObject;
                break;
            default:
                break;
        }

        // Set the gameObject name
        newPlayer.name = "Player " + playerNumber.ToString();

        // Add to the player array in the correct spot
        players[playerNumber - 1] = newPlayer;

        PlayerController newPlayerController = newPlayer.GetComponentInChildren<PlayerController>();
        // Set each players number
        newPlayerController.playerNumber = playerNumber;
        // Set the controller number
        newPlayerController.controller = (XboxCtrlrInput.XboxController)playerNumber;
        // Set player as alive
        newPlayerController.isAlive = true;

        LookRotation newLookRotation = newPlayer.GetComponentInChildren<LookRotation>();
        newLookRotation.StartUp();
        newLookRotation.LookTowards(zone.transform.position);


        //TODO do this is a better spot or a better way
        playerColourPicker.SetUp(players);
    }

    void GivePoints()
    {
        // Every second give the players in the zoneControl.playersInZone a point
        if (allowContest && zoneControl.playersInZone.Count == 1)
        {
            foreach (GameObject player in zoneControl.playersInZone)
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
    }

    void UpdateScoreBoard()
    {
        player1Score.text = ((int)scores[0]).ToString();
        player2Score.text = ((int)scores[1]).ToString();
        player3Score.text = ((int)scores[2]).ToString();
        player4Score.text = ((int)scores[3]).ToString();
    }

    void UpdateTimer()
    {
        // If there is still time left
        if (timer > 0.0f)
        {
            timer -= Time.deltaTime; // Count down
        }
        else
        {
            // Timer Run out!
        }

        // Miniutes and seconds
        //timeText.text = ((int)(timer / 60f)).ToString() + ":" + ((int)(timer % 60)).ToString();
        // Timer in seconds
        timeText.text = ((int)timer).ToString();
    }

    void SetWinMessage()
    {
        winMessageText.text = winMessageString.Replace('x', winningPlayerNumber.ToString()[0]);
    }

    void CheckForWinningPlayer()
    {
        for (int i = 0; i < scores.Length; i++)
        {
            if (scores[i] >= winScore)
            {
                winningPlayerNumber = i + 1;
                winMessageText.enabled = true;
                SetWinMessage();
            }
        }
    }
}
    
