using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerTextManager : MonoBehaviour
{
    Canvas canvas;
    Text text;
    PlayerController playerController;

    float timeLeft;
    [SerializeField]
    float timeStart = 3.0f;

    void Awake()
    {
        playerController = transform.parent.GetComponentInChildren<PlayerController>();
        canvas = GetComponentInChildren<Canvas>();
        text = canvas.GetComponentInChildren<Text>();
    }

    void Start()
    {
        text.text = "P" + playerController.playerNumber.ToString();
        timeLeft = timeStart;
        canvas.enabled = false;
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
        timeLeft = timeStart;
    }
}
