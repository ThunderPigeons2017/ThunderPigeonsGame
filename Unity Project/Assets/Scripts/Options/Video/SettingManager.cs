using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class SettingManager : MonoBehaviour
{
    public Toggle fullscreenToggle;
    public Dropdown resolutionDropDown;
    public Dropdown textureQualityDropdown;



    private bool optionPanel;
    private bool VideoPanel;
    private bool AudioPanel;

    public Resolution[] resolution;
    public GameSetting gameSettings;

    private void OnEnable()
    {
        gameSettings = new GameSetting();

        fullscreenToggle.onValueChanged.AddListener(delegate { OnFullScreenToggle(); });

        resolutionDropDown.onValueChanged.AddListener(delegate { OnResolutionChange(); });

        textureQualityDropdown.onValueChanged.AddListener(delegate { OnTextureChange(); });

        resolution = Screen.resolutions;

        resolutionDropDown.captionText.text = (Screen.currentResolution.width.ToString() + " x " + Screen.currentResolution.height.ToString());

        foreach (Resolution resolution in resolution)
        {
            resolutionDropDown.options.Add(new Dropdown.OptionData(resolution.width.ToString() + " x " + resolution.height.ToString()));
        }

        LoadSettings();
    }

    public void OnFullScreenToggle()
    {
        gameSettings.Fullscreen = Screen.fullScreen = fullscreenToggle.isOn;
    }

    public void OnResolutionChange()
    {
        Screen.SetResolution(resolution[resolutionDropDown.value].width, resolution[resolutionDropDown.value].height, Screen.fullScreen);
    }

    public void OnTextureChange()
    {
        QualitySettings.masterTextureLimit = gameSettings.textureQuality = textureQualityDropdown.value;
        
    }
    
    public void SaveSettings()
    {
        string Data = JsonUtility.ToJson(gameSettings,true);
        File.WriteAllText(Application.persistentDataPath + "/gamesettings.json",Data);
    }

    public void LoadSettings()
    {
        gameSettings = JsonUtility.FromJson<GameSetting>(File.ReadAllText(Application.persistentDataPath + "/gamesettings.json"));
        fullscreenToggle.isOn = gameSettings.Fullscreen;
        resolutionDropDown.value = gameSettings.resolutionIndex;
        textureQualityDropdown.value = gameSettings.textureQuality;
    }

}
