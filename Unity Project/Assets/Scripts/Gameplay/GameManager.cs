﻿using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

using XboxCtrlrInput;

public class GameManager : MonoBehaviour
{
    float[] scores = new float[4];
    float[] respawnTimer = new float[4];

    [SerializeField]
    float respawnTime;

    [SerializeField]
    int winScore;

    bool gameWon = false;

    [SerializeField]
    bool allowContest = false;

    [SerializeField]
    float startTime;
    float timer;

    [SerializeField]
    float deathYLevel = -3f;

    GameObject zone;
    ZoneControl zoneControl;
    ZoneManager zoneManager;

    // Keep a list of each player in the game
    public GameObject[] players = new GameObject[4];

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
	Text restartMessage;

    [SerializeField]
    [Tooltip("The message to display when a player has won (all lower case x are replaced with the winning player number)")]
    string winMessageString;

    PlayerColourPicker playerColourPicker;

    int winningPlayerNumber = 0;

    void Awake()
    {
        zone = GameObject.FindGameObjectWithTag("Zone");
        zoneControl = zone.GetComponent<ZoneControl>();
        zoneManager = zone.transform.parent.GetComponent<ZoneManager>();
        playerColourPicker = GetComponent<PlayerColourPicker>();

    }

    void Start()
    {
        timer = startTime;

        gameWon = false;

        winMessageText.enabled = false;
        restartMessage.enabled = false;
    }

    void OnEnable()
    {
        //Tell our 'OnLevelFinishedLoading' function to start listening for a scene change as soon as this script is enabled.
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable()
    {
        //Tell our 'OnLevelFinishedLoading' function to stop listening for a scene change as soon as this script is disabled. Remember to always have an unsubscription for every delegate you subscribe to!
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Search for players in the scene
        GameObject[] playersInScene = GameObject.FindGameObjectsWithTag("PlayerBall");
        for (int i = 0; i < playersInScene.Length; i++)
        {
            //DontDestroyOnLoad(playersInScene[i]);
            //playersInScene[i].transform.SetParent(newObject.transform);

            PlayerController playerController = playersInScene[i].GetComponent<PlayerController>();

            players[playerController.playerNumber - 1] = playersInScene[i];

            RespawnPlayer(playerController.playerNumber);
        }

    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
        for (int i = 0; i < 4; i++)
        {
            if (players[i] != null)
            {
                // Get the current player
                PlayerController playerController = players[i].GetComponent<PlayerController>();

                // If the player falls below a y level
                if (playerController.transform.position.y <= deathYLevel && playerController.isAlive == true)
                {
                    // Kill the player
                    playerController.isAlive = false;
                    respawnTimer[i] = respawnTime;
                }

                // If the player is dead
                if (playerController.isAlive == false)
                {
                    if (respawnTimer[i] <= 0)
                    {
                        RespawnPlayer(playerController.playerNumber);
                    }
                    else
                    {
                        respawnTimer[i] -= Time.deltaTime;
                    }
                }
            }
        }

        UpdateTimer();

        GivePoints();
        SetPlayerDrunkenness();

        // After the scores are added 
        UpdateScoreBoard();

        CheckForWinningPlayer();

        // If the game has been won
        if (gameWon)
        {
            // Turn on the messages
            restartMessage.enabled = true;
            winMessageText.enabled = true;
            SetWinMessage();
            
            // If a is pressed 
            if (XCI.GetButtonDown(XboxButton.A, XboxController.All))
            {
                SceneManager.LoadScene("Main Menu");
                for (int x = 0; x < 4; x++)
                {
                    if (players[x] != null)
                    {
                        Destroy(players[x].transform.parent.gameObject);
                    }
                }
            }
            // If b is pressed restart the level
            if (XCI.GetButtonDown(XboxButton.B, XboxController.All))
            {
                SceneManager.LoadScene("Beta");

                for (int playerNum = 1; playerNum < 5; playerNum++)
                {
                    RespawnPlayer(playerNum);
                }
            }
        }
    }

    void RespawnPlayer(int playerNumber)
    {
        if (players[playerNumber - 1] != null)
        {
            // Switch on the different players to spawn them in respective spots
            switch (playerNumber)
            {
                case 1:
                    players[playerNumber - 1].transform.position = player1Spawn.position;
                    break;
                case 2:
                    players[playerNumber - 1].transform.position = player2Spawn.position;
                    break;
                case 3:
                    players[playerNumber - 1].transform.position = player3Spawn.position;
                    break;
                case 4:
                    players[playerNumber - 1].transform.position = player4Spawn.position;
                    break;
                default:
                    break;
            }

            // Set player as alive
            players[playerNumber - 1].GetComponent<PlayerController>().isAlive = true;

            players[playerNumber - 1].GetComponent<Rigidbody>().velocity = Vector3.zero;

            LookRotation lookRotation = players[playerNumber - 1].transform.parent.GetComponentInChildren<LookRotation>();
            lookRotation.LookTowards(zone.transform.position);
        }

        //// Set the gameObject name
        //newPlayer.name = "Player " + playerNumber.ToString();

        //// Add to the player array in the correct spot
        //players[playerNumber - 1] = newPlayer;

        //PlayerController newPlayerController = newPlayer.GetComponentInChildren<PlayerController>();
        //// Set each players number
        //newPlayerController.playerNumber = playerNumber;
        //// Set the controller number
        //newPlayerController.controller = (XboxCtrlrInput.XboxController)playerNumber;
        //// Set player as alive
        //newPlayerController.isAlive = true;

        //LookRotation newLookRotation = newPlayer.GetComponentInChildren<LookRotation>();
        //newLookRotation.StartUp();
        //newLookRotation.LookTowards(zone.transform.position);

        ////TODO do this is a better spot or a better way
        //playerColourPicker.SetUp(players);
    }

    void GivePoints()
    {
        if (gameWon == false)
        {
            // Every second give the players in the zoneControl.playersInZone a point
            if (allowContest && zoneControl.playersInZone.Count == 1)
            {
                foreach (GameObject player in zoneControl.playersInZone)
                {
                    scores[player.GetComponent<PlayerController>().playerNumber - 1] += Time.deltaTime;
                    zoneManager.PointsGiven(Time.deltaTime);
                }
            }
            else if (!allowContest)
            {
                foreach (GameObject player in zoneControl.playersInZone)
                {
                    scores[player.GetComponent<PlayerController>().playerNumber - 1] += Time.deltaTime;
                    zoneManager.PointsGiven(Time.deltaTime);
                }
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
            SetHightestScorerAsWinner();
            gameWon = true;
        }

        // Miniutes and seconds
        //timeText.text = ((int)(timer / 60f)).ToString() + ":" + ((int)(timer % 60)).ToString();
        // Timer in seconds
        timeText.text = ((int)timer).ToString();
    }

    void SetPlayerDrunkenness()
    {
        for (int i = 0; i < 4; i++)
        {
            if (players[i] != null)
            {
                players[i].GetComponentInChildren<PlayerController>().drunkenness = scores[i] * 2;
            }
        }
    }

    void SetWinMessage()
    {
        if (winningPlayerNumber == 0)
        {
            winMessageText.text = "It's a tie";
        }
        else
        {
            winMessageText.text = winMessageString.Replace('x', winningPlayerNumber.ToString()[0]);
        }
    }

    void CheckForWinningPlayer()
    {
        for (int i = 0; i < scores.Length; i++)
        {
            if (scores[i] >= winScore)
            {
                gameWon = true;
                winningPlayerNumber = i + 1;
            }
        }
    }

    void SetHightestScorerAsWinner()
    {
        float highestScore = 0;
        int playernum = 0;
        for (int i = 0; i < 4; i++)
        {
            if (scores[i] > highestScore)
            {
                highestScore = scores[i];
                playernum = i + 1;
            }
        }

        winningPlayerNumber = playernum;
    }
}

