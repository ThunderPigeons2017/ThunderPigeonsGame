using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerTextManager : MonoBehaviour
{
    Canvas canvas;
    Text text;
    Outline outline;
    PlayerController playerController;
    ColourSetter colourSetter;

    float timeLeft;
    [SerializeField]
    float timeStart = 3.0f;

    [SerializeField]
    int dynamicPixelsPerUnit = 300;

    [SerializeField]
    bool useSecondaryColourForOutline;

    void Awake()
    {
        canvas = GetComponentInChildren<Canvas>();
        text = canvas.GetComponentInChildren<Text>();
        outline = text.GetComponent<Outline>();
        playerController = transform.parent.GetComponentInChildren<PlayerController>();
        colourSetter = GetComponent<ColourSetter>();
    }

    void Start()
    {
        text.text = "P" + playerController.playerNumber.ToString();
        timeLeft = timeStart;
        canvas.enabled = false;
        text.color = colourSetter.primaryColour;
        if (useSecondaryColourForOutline)
        {
            outline.effectColor = colourSetter.secondaryColour;
        }
        else
        {
            outline.effectColor = ChangeColorBrightness(colourSetter.primaryColour, -0.75f);
            //outline.effectColor = Color.Lerp(colourSetter.primaryColour, Color.black, 0.7f);
        }

        switch (playerController.playerNumber)
        {
            case 1:
                outline.effectColor = new Color(0.6f, 0.6f, 0.6f, 1);
                break;
            case 2:
                break;
            case 3:
                break;
            case 4:
                break;
            default:
                break;
        }
    }

    void Update()
    {
        if (timeLeft > 0)
        {
            timeLeft -= Time.deltaTime;
        }
        else
        {
            canvas.enabled = false;
        }
    }

    public void Show()
    {
        canvas.enabled = true;
        canvas.GetComponent<CanvasScaler>().dynamicPixelsPerUnit = 300;
        timeLeft = timeStart;
    }


    /// <summary>
    /// Creates color with corrected brightness.
    /// </summary>
    /// <param name="color">Color to correct.</param>
    /// <param name="correctionFactor">The brightness correction factor. Must be between -1 and 1. 
    /// Negative values produce darker colors.</param>
    /// <returns>
    /// Corrected <see cref="Color"/> structure.
    /// </returns>
    public static Color ChangeColorBrightness(Color color, float correctionFactor)
    {
        float red = color.r;
        float green = color.g;
        float blue = color.b;

        if (correctionFactor < 0)
        {
            correctionFactor = 1 + correctionFactor;
            red *= correctionFactor;
            green *= correctionFactor;
            blue *= correctionFactor;
        }
        else
        {
            red = (255 - red) * correctionFactor + red;
            green = (255 - green) * correctionFactor + green;
            blue = (255 - blue) * correctionFactor + blue;
        }

        return new Color(red, green, blue);
    }
}
