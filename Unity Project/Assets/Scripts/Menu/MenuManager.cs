﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

using XboxCtrlrInput;

public class MenuManager : MonoBehaviour
{
    // Keep a list of each player in the game
    public GameObject[] players = new GameObject[4];

    [SerializeField]
    GameObject mainMenuUI;
    [SerializeField]
    GameObject characterSelectUI;
    [SerializeField]
    GameObject optionsUI;

    [SerializeField]
    EventSystem Event;

    [SerializeField]
    Camera camera;
    MenuCameraControl cameraControl;

    [SerializeField]
    string gameSceneName;

    [Header("Prefabs:")]
    [SerializeField]
    GameObject playerPrefab;

    [SerializeField] // Each players spawn pos
    Transform player1Spawn, player2Spawn, player3Spawn, player4Spawn;

    CSManager csManager;

    PlayerColourPicker playerColourPicker;
    
    XboxButton backButton = XboxButton.B;
    XboxButton videoTab = XboxButton.LeftBumper;
    XboxButton audioTab = XboxButton.RightBumper;

    [Header("Option Tabs")]
    [SerializeField]
    GameObject pnl_Audio;
    [SerializeField]
    GameObject pnl_Video;
    [SerializeField]
    GameObject res_DropDown;
    [SerializeField]
    GameObject Sldr_Master;
    [SerializeField]
    GameObject btn_Play;

    enum MenuStates
    {
        MainMenu,
        CharacterSelection,
        Options
    }

    MenuStates menuState;

    void Awake()
    {
        playerColourPicker = GetComponent<PlayerColourPicker>();

        cameraControl = camera.GetComponent<MenuCameraControl>();

        csManager = GetComponent<CSManager>();

    }

    void Start()
    {
        StartMainMenu();
        FindObjectOfType<AudioManager>().Play("Audio-Theme");
        FindObjectOfType<AudioManager>().Play("SFX-Ocean");
    }

    void Update()
    {
        if (menuState == MenuStates.CharacterSelection)
        {
            csManager.UpdateLogic();
        }

        if (menuState == MenuStates.Options)
        {
            if (XCI.GetButtonDown(backButton, XboxController.All) || Input.GetKeyDown(KeyCode.B))
            {
                StartMainMenu();
            }

            if (XCI.GetButtonDown(videoTab, XboxController.All) || Input.GetKeyDown(KeyCode.C))
            {
                pnl_Video.gameObject.SetActive(true);
                pnl_Audio.gameObject.SetActive(false);
                FindObjectOfType<AudioManager>().Play("SFX-Button-Click");
                Event.SetSelectedGameObject(res_DropDown);
            }

            if (XCI.GetButtonDown(audioTab, XboxController.All) || Input.GetKeyDown(KeyCode.V))
            {
                pnl_Video.gameObject.SetActive(false);
                pnl_Audio.gameObject.SetActive(true);
                FindObjectOfType<AudioManager>().Play("SFX-Button-Click");
                Event.SetSelectedGameObject(Sldr_Master);
            }

        }
    }

    public void StartMainMenu()
    {
        DespawnPlayers();

        menuState = MenuStates.MainMenu;

        cameraControl.MoveToMainMenu();

        mainMenuUI.SetActive(true);
        characterSelectUI.SetActive(false);
        optionsUI.SetActive(false);
        Event.SetSelectedGameObject(btn_Play);
    }

    public void StartCharacterSelect()
    {
        menuState = MenuStates.CharacterSelection;

        cameraControl.MoveToCharacterSelection();

        mainMenuUI.SetActive(false);
        characterSelectUI.SetActive(true);
        optionsUI.SetActive(false);
    }

    public void StartOptions()
    {
        menuState = MenuStates.Options;

        cameraControl.MoveToOptions();

        mainMenuUI.SetActive(false);
        characterSelectUI.SetActive(false);
        optionsUI.SetActive(true);
        Event.SetSelectedGameObject(res_DropDown);
    }

    public void StartGame()
    {
        for (int i = 0; i < 4; i++)
        {
            if (players[i] != null)
            {
                DontDestroyOnLoad(players[i]);
                players[i].GetComponentInChildren<Animator>().SetBool("Chosen", false);
            }
        }
        SceneManager.LoadScene(gameSceneName);
    }

    public void SpawnPlayer(int playerNumber, bool recreateGameObject = true)
    {
        if (recreateGameObject == false)
        {
            switch (playerNumber)
            {
                case 1:
                    players[playerNumber - 1].GetComponentInChildren<PlayerController>().transform.position = player1Spawn.position;
                    break;
                case 2:
                    players[playerNumber - 1].GetComponentInChildren<PlayerController>().transform.position = player2Spawn.position;
                    break;
                case 3:
                    players[playerNumber - 1].GetComponentInChildren<PlayerController>().transform.position = player3Spawn.position;
                    break;
                case 4:
                    players[playerNumber - 1].GetComponentInChildren<PlayerController>().transform.position = player4Spawn.position;
                    break;
                default:
                    break;
            }

            return;
        }

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
        newLookRotation.LookTowards(Vector3.back * 1000);

        playerColourPicker.SetUp(players);
    }

    public void DespawnPlayers()
    {
        for (int i = 0; i < 4; i++)
        {
            if (players[i] != null)
            {
                Destroy(players[i]);
            }
        }
    }

}
