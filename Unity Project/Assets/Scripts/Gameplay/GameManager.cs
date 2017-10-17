using UnityEngine;
using UnityEngine.SceneManagement;
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
    [Tooltip("The message to display when a player has won (all lower case x are replaced with the winning player number)")]
    string winMessageString;

    PlayerColourPicker playerColourPicker;

    int winningPlayerNumber = 0;

    void Awake()
    {
        zoneControl = zone.GetComponent<ZoneControl>();
        playerColourPicker = GetComponent<PlayerColourPicker>();

        timer = startTime;


        winMessageText.enabled = false;
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
        GameObject newObject = new GameObject("WHAT");
        for (int i = 0; i < playersInScene.Length; i++)
        {
            //DontDestroyOnLoad(playersInScene[i]);
            //playersInScene[i].transform.SetParent(newObject.transform);

            PlayerController playerController = playersInScene[i].GetComponent<PlayerController>();

            players[playerController.playerNumber - 1] = playersInScene[i];

            RespawnPlayer(playerController.playerNumber);
        }

    }

    void Update ()
    {
        for (int i = 0; i < 4; i++)
        {
            if (players[i] != null)
            {
                PlayerController playerController = players[i].GetComponent<PlayerController>();
                if (playerController.transform.position.y <= deathYLevel)
                {
                    playerController.isAlive = false;
                }

                if (playerController.isAlive == false)
                {
                    RespawnPlayer(playerController.playerNumber);
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
            RespawnPlayer(1);
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            RespawnPlayer(2);
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            RespawnPlayer(3);
        }
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            RespawnPlayer(4);
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
    
