using System;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ManagementOptions : MonoBehaviour
{
    public TMP_Dropdown dropdownLanguage;
    public TMP_Dropdown dropdownResolution;
    public SoundInfo[] soundInfo;
    public GameObject homeButton;
    public GameObject muteCheck;
    public GameObject fullScreenCheck;
    public GameManagerHelper gameManagerHelper;

    void OnEnable()
    {
        if (dropdownLanguage.options.Count == 0)
        {
            InitializeLanguageDropdown();
        }
        if (dropdownResolution.options.Count == 0)
        {
            InitializeResolutionDropdown();
        }
        InitializeSliders();
        muteCheck.SetActive(ManagementData.saveData.configurationsInfo.soundConfiguration.isMute);
        fullScreenCheck.SetActive(ManagementData.saveData.configurationsInfo.resolutionConfiguration.isFullScreen);
        for (int i = 0; i < SceneManager.sceneCount; i++)
        {
            if (SceneManager.GetSceneAt(i).name == "HomeScene")
            {
                homeButton.SetActive(false);
                break;
            }
        }
        Time.timeScale = 0;
    }
    void OnDisable()
    {
        Time.timeScale = 1;        
    }
    public void InitializeLanguageDropdown()
    {
        foreach (ManagementData.TypeLanguage language in Enum.GetValues(typeof(ManagementData.TypeLanguage)))
        {
            TMP_Dropdown.OptionData option = new TMP_Dropdown.OptionData
            {
                text = language.ToString()
            };
            dropdownLanguage.options.Add(option);
        }
        for (int i = 0; i < dropdownLanguage.options.Count; i++)
        {
            if (dropdownLanguage.options[i].text == ManagementData.saveData.configurationsInfo.currentLanguage.ToString())
            {
                dropdownLanguage.value = i;
                break;
            }
        }
    }
    public void InitializeResolutionDropdown()
    {
        foreach (var resolutions in ManagementData.saveData.configurationsInfo.resolutionConfiguration.allResolutions)
        {
            TMP_Dropdown.OptionData option = new TMP_Dropdown.OptionData
            {
                text = $"{resolutions.width}X{resolutions.height}"
            };
            dropdownResolution.options.Add(option);
        }
        for (int i = 0; i < dropdownResolution.options.Count; i++)
        {
            ManagementData.ResolutionsInfo resolutionsInfo = GetCurrentResolution(dropdownResolution.options[i].text);
            if (resolutionsInfo.width == ManagementData.saveData.configurationsInfo.resolutionConfiguration.currentResolution.width &&
                resolutionsInfo.height == ManagementData.saveData.configurationsInfo.resolutionConfiguration.currentResolution.height)
            {
                dropdownResolution.value = i;
                break;
            }
        }
    }
    public void InitializeSliders()
    {
        FindSlider(TypeSound.Master).value = ManagementData.saveData.configurationsInfo.soundConfiguration.MASTERValue;
        FindSlider(TypeSound.BGM).value = ManagementData.saveData.configurationsInfo.soundConfiguration.BGMalue;
        FindSlider(TypeSound.SFX).value = ManagementData.saveData.configurationsInfo.soundConfiguration.SFXalue;
    }
    public void UnloadScene()
    {
        SceneManager.UnloadSceneAsync("OptionsScene");
    }
    public void ChangeSoundValue(int typeSound)
    {
        switch (typeSound)
        {
            case 0:
                ManagementData.saveData.configurationsInfo.soundConfiguration.MASTERValue = FindSlider(TypeSound.Master).value;
                break;
            case 1:
                ManagementData.saveData.configurationsInfo.soundConfiguration.BGMalue = FindSlider(TypeSound.BGM).value;
                break;
            case 2:
                ManagementData.saveData.configurationsInfo.soundConfiguration.SFXalue = FindSlider(TypeSound.SFX).value;
                break;
        }
        SetMixerValues();
    }
    public void SetMixerValues()
    {
        gameManagerHelper.SetAudioMixerData();
    }
    public void SetLenguage()
    {
        ManagementData.TypeLanguage selectedLenguage = (ManagementData.TypeLanguage)dropdownLanguage.value;
        ManagementData.saveData.configurationsInfo.currentLanguage = selectedLenguage;
        ManagementData.SaveGameData();
        var objects = FindObjectsByType<ManagementLanguage>(FindObjectsSortMode.None);
        foreach (var managementLanguage in objects)
        {
            managementLanguage.ValidateChangeText();
        }
    }
    public void SetFullScreen(){
        ManagementData.saveData.configurationsInfo.resolutionConfiguration.isFullScreen = !ManagementData.saveData.configurationsInfo.resolutionConfiguration.isFullScreen;
        fullScreenCheck.SetActive(ManagementData.saveData.configurationsInfo.resolutionConfiguration.isFullScreen);
        Screen.SetResolution(
            ManagementData.saveData.configurationsInfo.resolutionConfiguration.currentResolution.width,
            ManagementData.saveData.configurationsInfo.resolutionConfiguration.currentResolution.height,
            ManagementData.saveData.configurationsInfo.resolutionConfiguration.isFullScreen);
        ManagementData.SaveGameData();
    }
    public void SetResolution()
    {
        ManagementData.ResolutionsInfo currentResolution = GetCurrentResolution(dropdownResolution.options[dropdownResolution.value].text);
        ManagementData.saveData.configurationsInfo.resolutionConfiguration.currentResolution = currentResolution;
        Screen.SetResolution(
            currentResolution.width,
            currentResolution.height,
            ManagementData.saveData.configurationsInfo.resolutionConfiguration.isFullScreen);
        ManagementData.SaveGameData();
    }
    public void SetMute()
    {
        ManagementData.saveData.configurationsInfo.soundConfiguration.isMute = !ManagementData.saveData.configurationsInfo.soundConfiguration.isMute;
        muteCheck.SetActive(ManagementData.saveData.configurationsInfo.soundConfiguration.isMute);
        SetMixerValues();
        ManagementData.SaveGameData();
    }
    public ManagementData.ResolutionsInfo GetCurrentResolution(string resolution)
    {        
        int index = resolution.IndexOf("X");
        int width = int.Parse(resolution.Substring(0, index));
        int height = int.Parse(resolution.ToString().Substring(index + 1));
        return new ManagementData.ResolutionsInfo(width, height);
    }
    public Slider FindSlider(TypeSound typeSound)
    {
        foreach (var slider in soundInfo)
        {
            if (slider.typeSound == typeSound)
            {
                return slider.slider;
            }
        }
        return null;
    }
    [Serializable] public class SoundInfo
    {
        public TypeSound typeSound;
        public Slider slider;
    }
    public enum TypeSound
    {
        Master = 0,
        BGM = 1,
        SFX = 2
    }
}
