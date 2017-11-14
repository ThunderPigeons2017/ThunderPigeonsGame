using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using XboxCtrlrInput;

public class CSManager : MonoBehaviour
{
    [SerializeField]
    XboxButton readyButton = XboxButton.A;
    [SerializeField]
    XboxButton unReadyButton = XboxButton.B;

    [SerializeField]
    float yDeathLevel = -4;

    [SerializeField]
    string joinString = "to Join";
    [SerializeField]
    string readyUpString = "to Ready";
    [SerializeField]
    string readyString = "Ready";

    [SerializeField] // The text for each players readyness
    Text[] readyText = new Text[4];

    [SerializeField]
    GameObject[] canvasReadyButton = new GameObject[4];

    [SerializeField]
	GameObject startText;

    [SerializeField]
    GameObject[] meshPrefabs = new GameObject[4];

    MenuManager menuManager;

    bool[] readyPlayers = new bool[4];

    private void Awake()
    {
        menuManager = GetComponent<MenuManager>();
    }

    void Start()
    {
		startText.SetActive(false);
    }

    public void UpdateLogic()
    {
        bool canStart = true;

        int readyPlayerCount = 0;

        // Loop through each player number
        for (int playerNum = 1; playerNum < 5; playerNum++)
        {
            PlayerReadyInput(playerNum);

            if (menuManager.players[playerNum - 1] == null) // If the player doesn't exist
            {
                readyText[playerNum - 1].text = joinString;
                canvasReadyButton[playerNum - 1].SetActive(true);

                // Added back button to go back during character selection
                if (XCI.GetButtonDown(unReadyButton, (XboxController)playerNum))
                { 
                    for (int players = 1; players < 5; players++)
                    {
                        if (menuManager.players[players - 1] != null)
                        {
                            Destroy(menuManager.players[players - 1]);
                            menuManager.players[players - 1] = null;
                        }
                    }
                    menuManager.StartMainMenu();
                    break;
                }

            }
            else // The player exists
            {
                PlayerController playerController = menuManager.players[playerNum - 1].GetComponentInChildren<PlayerController>();
                // If the player fallsoff the ship respawn them
                if (playerController.transform.position.y <= yDeathLevel)
                {
                    menuManager.SpawnPlayer(playerNum, false);
                }

                if (readyPlayers[playerNum - 1] == false) // Player exists but isnt ready
                {
                    readyText[playerNum - 1].text = readyUpString;
                    canvasReadyButton[playerNum - 1].SetActive(true);

                    PlayerMeshSelectInput(playerNum);

                    canStart = false;

                    //menuManager.SpawnPlayer(playerNum, false); // Constantly respawn the player
                    //playerController.canMove = false;
                }
                else // Player is ready
                {

                    readyText[playerNum - 1].text = readyString;
                    canvasReadyButton[playerNum - 1].SetActive(false);

                    //playerController.canMove = true;
                    readyPlayerCount++;
                }
            }
        }

        if (readyPlayerCount > 1 & canStart)
        {
			startText.SetActive(true);
            if (XCI.GetButtonDown(XboxButton.Start, XboxController.All)) // Player start input
            {
                menuManager.StartGame();
            }
        }
        else
        {
			startText.SetActive(false);
        }
    }

    void PlayerReadyInput(int playerNum)
    {
        if (XCI.GetButtonDown(readyButton, (XboxController)playerNum)) // Player ready input
        {
            if (menuManager.players[playerNum - 1] == null) // If the player doesnt exist
            {
                menuManager.SpawnPlayer(playerNum);
            }
            else
            {
                readyPlayers[playerNum - 1] = true;
                menuManager.players[playerNum - 1].GetComponentInChildren<Animator>().SetBool("Chosen", true);
            }
        }

        if (XCI.GetButtonDown(unReadyButton, (XboxController)playerNum)) // Player unready input
        {
            if (readyPlayers[playerNum - 1] == true)
            {
                readyPlayers[playerNum - 1] = false;
                menuManager.players[playerNum - 1].GetComponentInChildren<Animator>().SetBool("Chosen", false);

            }
            else
            {
                GameObject.Destroy(menuManager.players[playerNum - 1]);
                menuManager.players[playerNum - 1] = null;
            }
        }
    }

    void PlayerMeshSelectInput(int playerNum)
    {
        MeshSetter meshSetter = menuManager.players[playerNum - 1].GetComponentInChildren<MeshSetter>();
        if (meshSetter == null)
            return;

        bool changedMesh = false;

        if (XCI.GetButtonDown(XboxButton.RightBumper, (XboxController)playerNum)) // right input
        {
            meshSetter.currentCharacter++;

            if ((int)meshSetter.currentCharacter == 4) // Don't go out of index
            {
                meshSetter.currentCharacter = (MeshSetter.Character)0;
            }

            changedMesh = true;
        }

        if (XCI.GetButtonDown(XboxButton.LeftBumper, (XboxController)playerNum)) // left input
        {
            meshSetter.currentCharacter--;

            if ((int)meshSetter.currentCharacter == -1) // Don't go out of index
            {
                meshSetter.currentCharacter = (MeshSetter.Character)3;
            }

            changedMesh = true;
        }

        if (changedMesh == true)
        {
            meshSetter.SetMeshPrefab(meshPrefabs[(int)meshSetter.currentCharacter]);
        }
    }
}
