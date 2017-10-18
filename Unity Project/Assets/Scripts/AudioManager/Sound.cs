/***********************************************
 * 
 * Thunder Pidgeon - Team 13
 * Sound Class Script
 * 
 * by Rouie Ortega
 * 
 **********************************************/

/* Unity Codes to make C# Script applicaple for Unity Game */
using UnityEngine.Audio;                                               //required for Audio Control
using UnityEngine;

[System.Serializable]                                                  //required to view in Instance

/* Class Sound begins */
 public class Sound
 {
    // Variable creation begins
    public string name;                                                //String Name for the name of the Audio File
    public AudioClip clip;                                             //Created Instance of AudioClip to access clip

    [Range(0f,100f)]                                                   //Set Volume Range
    public float volume;                                               //create public variable float for volume

    [Range(0f,3f)]                                                     //Set Pitch Range
    public float pitch;                                                //Create public variable float for pitch

    public bool Loop;                                                  //Create public variable bool for Loop check

    public AudioMixerGroup output;                                     //Create public Instance AudioMixerGroup called output

    [HideInInspector]                                                  //hides next variables from Inspector
    public AudioSource source;                                         //Creates public Instance for AudioSource called source
 }