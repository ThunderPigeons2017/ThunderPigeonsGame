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

    public void MusicController(float msfx)
    {
        masterMixer.SetFloat("MusicVol", msfx);
    }

    public void AmbienceController(float asfx)
    {
        masterMixer.SetFloat("AmbienceVol", asfx);
    }

    public void SFXController(float sfx)
    {
        masterMixer.SetFloat("FXVol", sfx);
    }

}
