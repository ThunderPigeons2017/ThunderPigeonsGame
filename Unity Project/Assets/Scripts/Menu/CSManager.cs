using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using XboxCtrlrInput;

public class CSManager : MonoBehaviour
{
    XboxButton readyButton = XboxButton.A;
    XboxButton unReadyButton = XboxButton.B;

    [SerializeField] // The text for each players readyness
    Text[] readyText = new Text[4];

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

            if (menuManager.players[playerNum - 1] == null) // If the player doesnt exists
            {
                readyText[playerNum - 1].text = "Press A to Join";
            }
            else if (readyPlayers[playerNum - 1] == false) // Player exists but isnt ready
            {
                readyText[playerNum - 1].text = "Press A to Ready";

                PlayerMeshSelectInput(playerNum);

                canStart = false;
            }
            else // Player is ready
            {
                readyText[playerNum - 1].text = "Ready";
                readyPlayerCount++;
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
            }
        }

        if (XCI.GetButtonDown(unReadyButton, (XboxController)playerNum)) // Player unready input
        {
            if (readyPlayers[playerNum - 1] == true)
            {
                readyPlayers[playerNum - 1] = false;
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
            meshSetter.meshNumber++;

            if (meshSetter.meshNumber == 4) // Don't go out of index
            {
                meshSetter.meshNumber = 0;
            }

            changedMesh = true;
        }

        if (XCI.GetButtonDown(XboxButton.LeftBumper, (XboxController)playerNum)) // left input
        {
            meshSetter.meshNumber--;

            if (meshSetter.meshNumber == -1) // Don't go out of index
            {
                meshSetter.meshNumber = 3;
            }

            changedMesh = true;
        }

        if (changedMesh == true)
        {
            meshSetter.SetMeshPrefab(meshPrefabs[meshSetter.meshNumber]);
        }
    }
}
