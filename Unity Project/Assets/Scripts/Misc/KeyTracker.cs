using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class KeyTracker : MonoBehaviour
{

    [SerializeField]
    string code = "cheat";

    int currentIndex = 0;

    [HideInInspector]
    public bool isCodeEntered;

    [SerializeField]
    UnityEvent myUnityEvent;

    void Start ()
    {
        DontDestroyOnLoad(gameObject);
	}
	
	void Update ()
    {
        if (isCodeEntered == false)
        {
            // Get the keys pressed this frame
            string input = Input.inputString;

            // Check each char
            foreach (char character in input)
            {
                // If this char is the next one in our code
                if (code[currentIndex] == character)
                {
                    currentIndex++; // Increment to check the next one

                    if (currentIndex >= (code.Length - 1))
                    {
                        // Cheat code entered
                        isCodeEntered = true;
                    
                        currentIndex = 0;
                    }
                }
                else // If not then reset
                {
                    currentIndex = 0;
                }
            }
        }
        else
        {
            // So the event
            myUnityEvent.Invoke();

            isCodeEntered = false;
        }
    }
}
