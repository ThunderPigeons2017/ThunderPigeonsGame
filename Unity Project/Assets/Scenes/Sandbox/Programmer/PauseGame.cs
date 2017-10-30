using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using XboxCtrlrInput;

public class PauseGame : MonoBehaviour
{

    public Transform canvas;
   // public Transform canvas2;
    public Transform optionsPanel;
    public Transform pauseMenu;
    public XboxController controller;

  	void Update ()
    {
        //Waits for press start or escape key
        if (XCI.GetButtonDown(XboxButton.Start, controller)||(Input.GetKeyDown(KeyCode.Escape)))
        {
            Pause();
        }
	}

    /// <summary>
    /// Pause function
    /// </summary>
    public void Pause()
    {
        //checks if Canvas is active/inactive
        if (canvas.gameObject.activeInHierarchy == false)
        {
            canvas.gameObject.SetActive(true);
            //canvas2.gameObject.SetActive(false);
            optionsPanel.gameObject.SetActive(false);
            pauseMenu.gameObject.SetActive(true);
            Time.timeScale = 0;
        }
        else
        {
            canvas.gameObject.SetActive(false);
           // canvas2.gameObject.SetActive(true);
            optionsPanel.gameObject.SetActive(false);
            pauseMenu.gameObject.SetActive(true);
            Time.timeScale = 1;
        }
    }
}
