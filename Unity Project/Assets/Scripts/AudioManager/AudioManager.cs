/***********************************************
 * 
 * Thunder Pidgeon - Team 13
 * Audio Manager Script
 * 
 * by Rouie Ortega
 * 
 **********************************************/

/* Unity Codes to make C# Script applicaple for Unity Game */
using UnityEngine.Audio;                                                              //required for Audio Control
using System;
using UnityEngine;

/*  */
public class AudioManager : MonoBehaviour {

    public Sound[] sounds;

    public static AudioManager instance;

    int x = 0;

	void Awake ()
    {

        if (instance == null)
            instance = this;
        else
        {
            Destroy(gameObject);
            return;
        }

        foreach (Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;

            s.source.volume = s.volume;
            s.source.loop = s.Loop;

            s.source.outputAudioMixerGroup = s.output;
        }

        Play("Music");
        Play("Ocean");
	}

    // Update is called once per frame
    public void Play(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);

        if (s == null)
            return;

        s.source.Play();
    }

    public void Update()
    {
        if (x == 1000000)
        {
            Play("Gulls");
            x = 0;
        }
        else
            x++;
    }

}
