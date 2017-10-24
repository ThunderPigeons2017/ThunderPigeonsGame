using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.Audio;


public class SliderMusicScript : MonoBehaviour
{
    public AudioMixer masterMixer;
    
    public Slider volumeSlider;
    
    public void VolumeController(float sfx)
    {
        masterMixer.SetFloat("Music", sfx);
    }
	
}
