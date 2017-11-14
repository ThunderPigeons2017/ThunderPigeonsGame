using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerColourPicker : MonoBehaviour
{
    ColourSetter[] colourSetter = new ColourSetter[4];

    [Header("Player 1")]
    public Color p1PrimaryColour;
    public Color p1SecondaryColour;

    [Header("Player 2")]
    public Color p2PrimaryColour;
    public Color p2SecondaryColour;

    [Header("Player 3")]
    public Color p3PrimaryColour;
    public Color p3SecondaryColour;

    [Header("Player 4")]
    public Color p4PrimaryColour;
    public Color p4SecondaryColour;

    public void SetUp(GameObject[] players)
    {
        for (int i = 0; i < 4; i++)
        {
            if (players[i] != null)
            {
                colourSetter[i] = players[i].GetComponentInChildren<ColourSetter>();

                switch (i)
                {
                    case 0:
                        colourSetter[i].SetColours(p1PrimaryColour, p1SecondaryColour);
                        break;
                    case 1:
                        colourSetter[i].SetColours(p2PrimaryColour, p2SecondaryColour);
                        break;
                    case 2:
                        colourSetter[i].SetColours(p3PrimaryColour, p3SecondaryColour);
                        break;
                    case 3:
                        colourSetter[i].SetColours(p4PrimaryColour, p4SecondaryColour);
                        break;
                    default:
                        break;
                }

            }
        }

    }
}
