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
    Text startText;

    MenuManager menuManager;

    bool[] readyPlayers = new bool[4];

    private void Awake()
    {
        menuManager = GetComponent<MenuManager>();
    }

    void Start()
    {
        startText.enabled = false;
    }

    public void UpdateLogic()
    {
        int readyPlayerCount = 0;

        for (int playerNum = 1; playerNum < 5; playerNum++)
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

            if (menuManager.players[playerNum - 1] == null) // If the player doesnt exists
            {
                readyText[playerNum - 1].text = "Press A to Join";
            }
            else if (readyPlayers[playerNum - 1] == false)
            {
                readyText[playerNum - 1].text = "Press A Ready Up";
            }
            else
            {
                readyText[playerNum - 1].text = "Ready";
                readyPlayerCount++;
            }
        }

        if (readyPlayerCount > 1)
        {
            startText.enabled = true;
            if (XCI.GetButtonDown(XboxButton.Start, XboxController.All)) // Player start input
            {
                menuManager.StartGame();
            }
        }
        else
        {
            startText.enabled = false;
        }
    }
}
