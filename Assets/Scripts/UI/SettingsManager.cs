using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using System.Collections.Generic;
using System.Linq;
using TMPro;

public class SettingsManager : MonoBehaviour
{
    [Header("Audio Settings")]
    public AudioMixer masterMixer;
    public Slider volumeSlider;

    [Header("Graphics Settings")]
    public TMP_Dropdown resolutionDropdown;

    private Resolution[] resolutions;
    private List<Resolution> filteredResolutions;
    private int currentResolutionIndex = 0;

    const string MASTER_VOLUME_KEY = "MasterVolume";
    const string RESOLUTION_PREF_KEY_WIDTH = "ResolutionWidth";
    const string RESOLUTION_PREF_KEY_HEIGHT = "ResolutionHeight";
    const string RESOLUTION_PREF_KEY_REFRESH_RATE = "ResolutionRefreshRate";
    const string FULLSCREEN_PREF_KEY = "FullscreenPreference";

    // Inicializace nastavení zvuku a rozlišení
    void Start()
    {
        if (volumeSlider != null && masterMixer != null)
        {
            float savedVolume = PlayerPrefs.GetFloat(MASTER_VOLUME_KEY, 0.75f);
            SetMasterVolume(savedVolume);
            volumeSlider.value = savedVolume;
            volumeSlider.onValueChanged.AddListener(SetMasterVolume);
        }

        if (resolutionDropdown != null)
        {
            resolutions = Screen.resolutions;
            filteredResolutions = new List<Resolution>();

            resolutionDropdown.ClearOptions();
            List<string> options = new List<string>();
            HashSet<string> uniqueResolutions = new HashSet<string>();

            for (int i = resolutions.Length -1; i >= 0; i--)
            {
                string resolutionOption = resolutions[i].width + " x " + resolutions[i].height + " @ " + resolutions[i].refreshRateRatio.ToString() + " Hz";
                if(uniqueResolutions.Add(resolutions[i].width + "x" + resolutions[i].height))
                {
                    filteredResolutions.Add(resolutions[i]);
                    options.Add(resolutionOption);
                }
            }
            filteredResolutions.Reverse();
            options.Reverse();

            resolutionDropdown.AddOptions(options);
            LoadResolutionSettings();
            resolutionDropdown.onValueChanged.AddListener(delegate { SetSelectedResolution(resolutionDropdown.value); });
        }
    }

    // Nastaví hlavní hlasitost ve hře
    public void SetMasterVolume(float volume)
    {
        if (masterMixer != null)
        {
            masterMixer.SetFloat("MasterVolume", Mathf.Log10(Mathf.Max(volume, 0.0001f)) * 20);
            PlayerPrefs.SetFloat(MASTER_VOLUME_KEY, volume);
            PlayerPrefs.Save();
        }
    }

    // Načte uložené nastavení rozlišení obrazovky
    void LoadResolutionSettings()
    {
        if (filteredResolutions == null || filteredResolutions.Count == 0) return;

        int savedWidth = PlayerPrefs.GetInt(RESOLUTION_PREF_KEY_WIDTH, Screen.currentResolution.width);
        int savedHeight = PlayerPrefs.GetInt(RESOLUTION_PREF_KEY_HEIGHT, Screen.currentResolution.height);
        float savedRefreshRate = PlayerPrefs.GetFloat(RESOLUTION_PREF_KEY_REFRESH_RATE, (float)Screen.currentResolution.refreshRateRatio.value);

        bool foundSavedResolution = false;
        for (int i = 0; i < filteredResolutions.Count; i++)
        {
            if (filteredResolutions[i].width == savedWidth &&
                filteredResolutions[i].height == savedHeight &&
                Mathf.Approximately((float)filteredResolutions[i].refreshRateRatio.value, savedRefreshRate))
            {
                currentResolutionIndex = i;
                foundSavedResolution = true;
                break;
            }
        }

        if (!foundSavedResolution)
        {
            for (int i = 0; i < filteredResolutions.Count; i++)
            {
                 if (filteredResolutions[i].width == savedWidth && filteredResolutions[i].height == savedHeight)
                 {
                    currentResolutionIndex = i;
                    foundSavedResolution = true;
                    break;
                 }
            }
        }

        if (!foundSavedResolution)
        {
            for (int i = 0; i < filteredResolutions.Count; i++)
            {
                if (filteredResolutions[i].width == Screen.currentResolution.width &&
                    filteredResolutions[i].height == Screen.currentResolution.height)
                {
                    currentResolutionIndex = i;
                    foundSavedResolution = true;
                    break;
                }
            }
        }

        if (!foundSavedResolution && filteredResolutions.Count > 0)
        {
            currentResolutionIndex = filteredResolutions.Count - 1;
        }

        resolutionDropdown.value = currentResolutionIndex;
        resolutionDropdown.RefreshShownValue();
    }

    // Nastaví vybrané rozlišení obrazovky
    public void SetSelectedResolution(int resolutionIndex)
    {
        currentResolutionIndex = resolutionIndex;
        ApplyResolution();
    }

    public void ApplyResolution()
    {
        if (filteredResolutions != null && currentResolutionIndex >= 0 && currentResolutionIndex < filteredResolutions.Count)
        {
            Resolution selectedResolution = filteredResolutions[currentResolutionIndex];
            
            FullScreenMode screenMode;
            bool isFullscreenDesired = PlayerPrefs.GetInt(FULLSCREEN_PREF_KEY, Screen.fullScreen ? 1 : 0) == 1;

            if (isFullscreenDesired)
            {
                screenMode = FullScreenMode.FullScreenWindow;
            }
            else
            {
                screenMode = FullScreenMode.Windowed;
            }

            Screen.SetResolution(selectedResolution.width, selectedResolution.height, screenMode, new RefreshRate { numerator = (uint)selectedResolution.refreshRateRatio.numerator, denominator = (uint)selectedResolution.refreshRateRatio.denominator });
            
            PlayerPrefs.SetInt(RESOLUTION_PREF_KEY_WIDTH, selectedResolution.width);
            PlayerPrefs.SetInt(RESOLUTION_PREF_KEY_HEIGHT, selectedResolution.height);
            PlayerPrefs.SetFloat(RESOLUTION_PREF_KEY_REFRESH_RATE, (float)selectedResolution.refreshRateRatio.value);
            PlayerPrefs.Save();
        }
    }

    public void SetFullscreen(bool isFullscreen)
    {
        PlayerPrefs.SetInt(FULLSCREEN_PREF_KEY, isFullscreen ? 1 : 0);
        PlayerPrefs.Save();
        ApplyResolution(); 
    }
}