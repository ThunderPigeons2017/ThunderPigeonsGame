using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.EventSystems;

using XboxCtrlrInput;

public class GameManager : MonoBehaviour
{
    float[] scores = new float[4];
    float[] respawnTimer = new float[4];

    [SerializeField]
    float respawnTime;

    [SerializeField]
    int winScore;

    [SerializeField]
    EventSystem Event;

    bool gameWon = false;

    [SerializeField]
    bool allowContest = false;

    [SerializeField]
    float startTime;
    float timer;

    [SerializeField]
    float deathYLevel = -3f;

    [SerializeField]
    [Range(0, 3)]
    float drunknessScoreScale;

    [SerializeField]
    int maximumDrunkeness = 300;

    [SerializeField]
    XboxButton goToMainMenuButton = XboxButton.B;
    [SerializeField]
    XboxButton restartGameButton = XboxButton.A;

    GameObject zone;
    ZoneControl zoneControl;
    ZoneManager zoneManager;

    // Keep a list of each player in the game
    public GameObject[] players = new GameObject[4];

    [SerializeField]
    [Range(0, 1)]
    float sliderMaxValue = 1f;

    [SerializeField] // Each players score objects
    GameObject[] scoreObjects = new GameObject[4];


    Slider[] sliders = new Slider[4];

    [SerializeField]
    Text timeText;

    [SerializeField]
    GameObject outerSpawnParent;
    [SerializeField]
    GameObject centerSpawnParent;

    Transform[] outerSpawns = new Transform[4];
    Transform[] centreSpawns = new Transform[4];

    [Header("")]
    [SerializeField]
    GameObject winMessage;

    [SerializeField]
    Text winMessageText;


    [SerializeField]
    [Tooltip("The message to display when a player has won (all lower case x are replaced with the winning player number)")]
    string winMessageString;

    bool gamePaused = false;
    bool options = false;

    [SerializeField]
    [Tooltip("Pause Menu and ingame Options Panel")]
    Transform optionsPanel;
    [SerializeField]
    Transform pauseMenu;

    PlayerColourPicker playerColourPicker;

    int winningPlayerNumber = 0;

    XboxButton backButton = XboxButton.B;
    XboxButton videoTab = XboxButton.LeftBumper;
    XboxButton audioTab = XboxButton.RightBumper;

    [Header("Option Tabs")]
    [SerializeField]
    GameObject pnl_Audio;
    [SerializeField]
    GameObject pnl_Video;
    [SerializeField]
    GameObject Sldr_Master;
    [SerializeField]
    GameObject btn_Resume;

    void Awake()
    {
        zone = GameObject.FindGameObjectWithTag("Zone");
        zoneControl = zone.GetComponent<ZoneControl>();
        zoneManager = zone.transform.parent.GetComponent<ZoneManager>();
        playerColourPicker = GetComponent<PlayerColourPicker>();

        for (int i = 0; i < 4; i++)
        {
            outerSpawns[i] = outerSpawnParent.transform.GetChild(i);
            centreSpawns[i] = centerSpawnParent.transform.GetChild(i);
            sliders[i] = scoreObjects[i].GetComponentInChildren<Slider>();
        }

    }

    void Start()
    {
#if UNITY_EDITOR

#else
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = (false);
#endif
        timer = startTime;

        gameWon = false;

        winMessage.SetActive(false);

        FindObjectOfType<AudioManager>().Play("Audio-Theme");
        FindObjectOfType<AudioManager>().Play("SFX-Ocean");

        for (int playerNum = 1; playerNum < 5; playerNum++)
        {
            if (players[playerNum - 1] == null)
            {
                scoreObjects[playerNum - 1].SetActive(false);
            }
        }
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
        GameObject[] playerBallsInScene = GameObject.FindGameObjectsWithTag("PlayerBall");
        for (int i = 0; i < playerBallsInScene.Length; i++)
        {
            PlayerController playerController = playerBallsInScene[i].GetComponent<PlayerController>();

            if (playerController == null)
                continue;

            players[playerController.playerNumber - 1] = playerBallsInScene[i];

            RespawnPlayer(playerController.playerNumber);

            playerBallsInScene[i].transform.parent.GetComponentInChildren<PlayerTextManager>().Show();
        }

    }

    void Update()
    {
#if UNITY_EDITOR

#else
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = (false);
#endif
        //RJ codes pause
        //waits for Start to press or escape then pauses game
        if ((XCI.GetButtonDown(XboxButton.Start, XboxController.All) || (Input.GetKeyDown(KeyCode.Escape)) && options == false))
        {
            if (!gamePaused)
            {
                Pause();
            }
            else
            {
                Unpause();
            }
        }

        if (options)
        {
            StartOptions();
        }

        // Restart scene
        if (Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

        // Loop through players
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
                    FindObjectOfType<AudioManager>().Play("SFX-Splash");
                    FindObjectOfType<AudioManager>().Play("VO_Barry_Death");
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
            winMessage.SetActive(true);

            SetWinMessage();
            
            // If a is pressed 
            if (XCI.GetButtonDown(goToMainMenuButton, XboxController.All))
            {
                GoToMainMenu();
            }
            // If b is pressed restart the level
            if (XCI.GetButtonDown(restartGameButton, XboxController.All))
            {
                RestartScene();
            }
        }
    }

    public void Pause()
    {
        if (gameWon)
        {
            return;
        }

        gamePaused = true;

        
        optionsPanel.gameObject.SetActive(false);
        pauseMenu.gameObject.SetActive(true);
        Event.SetSelectedGameObject(btn_Resume);
        Time.timeScale = 0; //pauses game
    }

    public void Unpause()
    {
        gamePaused = false;
        Event.SetSelectedGameObject(null);
        optionsPanel.gameObject.SetActive(false);
        pauseMenu.gameObject.SetActive(false);
        Time.timeScale = 1; //unpauses game
    }

    /// <summary>
    /// options menu is opened
    /// </summary>
    public void StartOptions()
    {
        gamePaused = true;
        options = true;
        optionsPanel.gameObject.SetActive(true);
        pauseMenu.gameObject.SetActive(false);
        

        if (XCI.GetButtonDown(backButton, XboxController.All) || Input.GetKeyDown(KeyCode.B))
        {
            options = false;
            pnl_Video.gameObject.SetActive(true);
            pnl_Audio.gameObject.SetActive(false);
            Pause();
        }

        if (XCI.GetButtonDown(videoTab, XboxController.All) || Input.GetKeyDown(KeyCode.C))
        {
            pnl_Video.gameObject.SetActive(true);
            pnl_Audio.gameObject.SetActive(false);
            FindObjectOfType<AudioManager>().Play("SFX-Button-Click");
        }

        if (XCI.GetButtonDown(audioTab, XboxController.All) || Input.GetKeyDown(KeyCode.V))
        {
            pnl_Video.gameObject.SetActive(false);
            pnl_Audio.gameObject.SetActive(true);
            FindObjectOfType<AudioManager>().Play("SFX-Button-Click");
            Event.SetSelectedGameObject(Sldr_Master);
        }

    }

    /// <summary>
    /// goes back to main menu
    /// </summary>
    public void GoToMainMenu()
    {
        Unpause();
        SceneManager.LoadScene("Main Menu");
        for (int x = 0; x < 4; x++)
        {
            if (players[x] != null)
            {
                Destroy(players[x].transform.parent.gameObject);
            }
        }
    }

    public void RestartScene()
    {
        SceneManager.LoadScene("Gold");

        for (int playerNum = 1; playerNum < 5; playerNum++)
        {
            RespawnPlayer(playerNum);
        }
    }

    void RespawnPlayer(int playerNumber)
    {
        if (players[playerNumber - 1] != null)
        {
            if (zoneManager.zonePosition == ZoneManager.ZonePosition.Centre) // If the zone is in the centre
            {
                players[playerNumber - 1].transform.position = outerSpawns[playerNumber - 1].position; // Spawn on outer spawn
            }
            else
            {
                players[playerNumber - 1].transform.position = centreSpawns[playerNumber - 1].position; // Spawn in the centre
            }

            //// Switch on the different players to spawn them in respective spots
            //switch (playerNumber)
            //{
            //    case 1:
            //        players[playerNumber - 1].transform.position = player1Spawn.position;
            //        break;
            //    case 2:
            //        players[playerNumber - 1].transform.position = player2Spawn.position;
            //        break;
            //    case 3:
            //        players[playerNumber - 1].transform.position = player3Spawn.position;
            //        break;
            //    case 4:
            //        players[playerNumber - 1].transform.position = player4Spawn.position;
            //        break;
            //    default:
            //        break;
            //}

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
        for (int playerNum = 1; playerNum < 5; playerNum++)
        {
            sliders[playerNum - 1].value = Helper.Remap(scores[playerNum - 1], 0, winScore, 0, sliderMaxValue);
        }
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
                if (scores[i] * drunknessScoreScale > maximumDrunkeness)
                {
                    players[i].GetComponentInChildren<PlayerController>().drunkenness = maximumDrunkeness;
                }
                else
                {
                    players[i].GetComponentInChildren<PlayerController>().drunkenness = scores[i] * drunknessScoreScale;
                }
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
            FindObjectOfType<AudioManager>().Play("VO_Barry_Win");
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

    public void ExitButton()
    {
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
                Application.Quit();
        #endif
    }

    ///This is for the Spin Attack to Cancel out bug
    public bool gamePause()
    {
        return gamePaused;
    }

    private void OnApplicationPause(bool pause)
    {
        gamePaused = true;
        Pause();
    }
}

