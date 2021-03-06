﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollCredits : MonoBehaviour
{
    Vector3 startPos;

    [SerializeField]
    float speed = 50;

    [SerializeField]
    float maxTop = -2820;

    [HideInInspector]
    public bool hasReachedTop;

    RectTransform rectTransform;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    void Start()
    {
        startPos = rectTransform.position;
	}
	
	public void ScrollUpdate()
    {
        if (rectTransform.localPosition.y > maxTop)
        {
            hasReachedTop = true;
        }
        else
        {
            rectTransform.position += new Vector3(0, speed * Time.deltaTime, 0);
        }
	}

    public void ResetPosition()
    {
        rectTransform.position = startPos;

        hasReachedTop = false;
    }
}
