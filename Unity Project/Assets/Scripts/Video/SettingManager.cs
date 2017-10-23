using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class SettingManager : MonoBehaviour
{
    public Toggle fullscreenToggle;
    public Dropdown resolutionDropDown;
    public Dropdown textureQualityDropdown;
    public Button applyButton;

    public Resolution[] resolution;
    public GameSetting gameSettings;

    private void OnEnable()
    {
        gameSettings = new GameSetting();

        fullscreenToggle.onValueChanged.AddListener(delegate { OnFullScreenToggle(); });

        resolutionDropDown.onValueChanged.AddListener(delegate { OnResolutionChange(); });

        textureQualityDropdown.onValueChanged.AddListener(delegate { OnTextureChange(); });

        applyButton.onClick.AddListener(delegate { OnApplyButtonClick(); });

        resolution = Screen.resolutions;
        foreach (Resolution resolution in resolution)
        {
            resolutionDropDown.options.Add(new Dropdown.OptionData(resolution.ToString()));
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

    public void OnApplyButtonClick()
    {
        SaveSettings();
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
