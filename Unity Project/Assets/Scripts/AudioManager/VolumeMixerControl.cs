using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.Audio;


public class VolumeMixerControl : MonoBehaviour
{
    public AudioMixer masterMixer;

    public void MasterController(float sound)
    {
        masterMixer.SetFloat("MasterVol", sound);
    }

    public void MusicController(float sfx)
    {
        masterMixer.SetFloat("MusicVol", sfx);
    }

    public void AmbienceController(float msfx)
    {
        masterMixer.SetFloat("AmbienceVol", msfx);
    }

}
