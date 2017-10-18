/***********************************************
 * 
 * Thunder Pidgeon - Team 13
 * Volume Mixer Script
 * 
 * by Rouie Ortega
 * 
 **********************************************/

 /* Unity Codes to make C# Script applicaple for Unity Game */
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.Audio;                                                //required for Audio Control

/* VolumeMixerControl class begins */
public class VolumeMixerControl : MonoBehaviour
{
    public AudioMixer masterMixer;                                      //Creates Instance of AudioMixer and calls it masterMixer

    /// <summary>
    /// Function for MasterController; passes in variable float sound
    /// </summary>
    /// <param name="sound"> master volume sound </param>
    public void MasterController(float sound)
    {
        masterMixer.SetFloat("MasterVol", sound);                       //sets the sound value of MasterVol to the value of sound
    }

    
    /// <summary>
    /// Function for MusicController; passes in variable float sound
    /// </summary>
    /// <param name="msfx"> music sound volume </param>
    public void MusicController(float msfx)                     
    {
        masterMixer.SetFloat("MusicVol", msfx);                         //sets the sound value of MusicVol to the value of msfx
    }

    /// <summary>
    /// Function for AmbienceController; passes in variable float asfx
    /// </summary>
    /// <param name="asfx"> ambience sound volume </param>
    public void AmbienceController(float asfx)
    {
        masterMixer.SetFloat("AmbienceVol", asfx);                       //sets the sound value of AmbienceVol to the value of asfx
    }

}
