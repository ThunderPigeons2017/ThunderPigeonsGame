using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XboxCtrlrInput;

public class CheatCode : MonoBehaviour {

	//this Script is used to activate the cheatcode in game. Have Fun!

	public GameObject objectRain;

	// Update is called once per frame
	void Update () {
		if ((XCI.GetButton (XboxButton.B)) && (XCI.GetButton (XboxButton.Y)) && (XCI.GetButton (XboxButton.X))) {
			objectRain.SetActive(true);
			Debug.Log ("Cheat Code Activated! Arr!");

		}
	}
}
