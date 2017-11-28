using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.Audio;
using System.IO;


public class VolumeMixerControl : MonoBehaviour
{
    public AudioMixer masterMixer;

    public GameSetting gameSettings;

    private void OnEnable()
    {
        gameSettings = new GameSetting();
        
    }
    
    public void MasterController(float sound)
    {
        masterMixer.SetFloat("MasterVol", sound);
        gameSettings.soundVolume = sound;
    }

    public void MusicController(float msfx)
    {
        masterMixer.SetFloat("MusicVol", msfx);
        gameSettings.musicVolume = msfx;
    }

    public void AmbienceController(float asfx)
    {
        masterMixer.SetFloat("AmbienceVol", asfx);
        gameSettings.ambienceVolume = asfx;
    }

    public void SFXController(float sfx)
    {
        masterMixer.SetFloat("FXVol", sfx);
        gameSettings.effectsVolume = sfx;
    }

    public void SaveSettings()
    {
        string Data = JsonUtility.ToJson(gameSettings, true);
        File.WriteAllText(Application.persistentDataPath + "/gamesettings.json", Data);
    }

    public void LoadSettings()
    {
        gameSettings = JsonUtility.FromJson<GameSetting>(File.ReadAllText(Application.persistentDataPath + "/gamesettings.json"));
        masterMixer.SetFloat("MasterVol", gameSettings.soundVolume);
        masterMixer.SetFloat("MusicVol", gameSettings.musicVolume);
        masterMixer.SetFloat("AmbienceVol", gameSettings.ambienceVolume);
        masterMixer.SetFloat("FXVol", gameSettings.effectsVolume);
    }

}
