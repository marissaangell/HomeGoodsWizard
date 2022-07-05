using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class OptionsMenu : MonoBehaviour
{
    [Header("Graphics Fields")]
    public Toggle fullscreenToggle;
    public Toggle vsyncToggle;
    public TMP_Dropdown resolutionDropdown;
    private Resolution[] resolutions;

    [Header("Sound Fields")]
    public AudioMixer audioMixer;
    public TMP_Text volumeLabelMaster, volumeLabelMusic, volumeLabelSFX, volumeLabelUI;
    public Slider volumeSliderMaster, volumeSliderMusic, volumeSliderSFX, volumeSliderUI;

    //private SoundManager sound;

    // Initializations
    void Start()
    {
        //sound = FindObjectOfType<SoundManager>();

        // Graphics settings init
        fullscreenToggle.isOn = Screen.fullScreen;
        vsyncToggle.isOn = QualitySettings.vSyncCount == 0 ? false : true;
        InitResolutionsDropdown();

        // Sound settings init
        /*audioMixer.GetFloat("MasterVolume", out float masterVol);
        volumeSliderMaster.value = InvLogVolume(masterVol);
        volumeLabelMaster.text = Mathf.RoundToInt(InvLogVolume(masterVol) * 100).ToString();

        audioMixer.GetFloat("MusicVolume", out float musicVol);
        volumeSliderMusic.value = InvLogVolume(musicVol);
        volumeLabelMusic.text = Mathf.RoundToInt(InvLogVolume(musicVol) * 100).ToString();

        audioMixer.GetFloat("SFXVolume", out float sfxVol);
        volumeSliderSFX.value = InvLogVolume(sfxVol);
        volumeLabelSFX.text = Mathf.RoundToInt(InvLogVolume(sfxVol) * 100).ToString();

        audioMixer.GetFloat("UIVolume", out float uiVol);
        volumeSliderUI.value = InvLogVolume(uiVol);
        volumeLabelUI.text = Mathf.RoundToInt(InvLogVolume(uiVol) * 100).ToString();*/
    }


    public void ApplyGraphics()
    {
        QualitySettings.vSyncCount = vsyncToggle.isOn ? 1 : 0;
        SetResolution(resolutionDropdown.value);

        //sound.Play("UI_Select");
    }

    // Master Volume
    public void SetMasterVolume()
    {
        volumeLabelMaster.text = Mathf.RoundToInt(volumeSliderMaster.value * 100).ToString();

        float logVolume = LogVolume(volumeSliderMaster.value);
        audioMixer.SetFloat("MasterVolume", logVolume);
        PlayerPrefs.SetFloat("MasterVolume", logVolume);
    }

    // Music
    public void SetMusicVolume()
    {
        volumeLabelMusic.text = Mathf.RoundToInt(volumeSliderMusic.value * 100).ToString();

        float logVolume = LogVolume(volumeSliderMusic.value);
        audioMixer.SetFloat("MusicVolume", logVolume);
        PlayerPrefs.SetFloat("MusicVolume", logVolume);
    }

    // SFX
    public void SetSFXVolume()
    {
        volumeLabelSFX.text = Mathf.RoundToInt(volumeSliderSFX.value * 100).ToString();

        float logVolume = LogVolume(volumeSliderSFX.value);
        audioMixer.SetFloat("SFXVolume", logVolume);
        PlayerPrefs.SetFloat("SFXVolume", logVolume);
    }

    // UI
    public void SetUIVolume()
    {
        volumeLabelUI.text = Mathf.RoundToInt(volumeSliderUI.value * 100).ToString();

        float logVolume = LogVolume(volumeSliderUI.value);
        audioMixer.SetFloat("UIVolume", logVolume);
        PlayerPrefs.SetFloat("UIVolume", logVolume);
    }

    /*
     * Private Helpers
     */

    // Reference for logarithmic conversion:
    // https://johnleonardfrench.com/the-right-way-to-make-a-volume-slider-in-unity-using-logarithmic-conversion/
    private float LogVolume(float value)
    {
        return Mathf.Log10(value) * 20;
    }

    private float InvLogVolume(float logVolume)
    {
        return Mathf.Pow(10f, (logVolume / 20.0f));
    }



    private void InitResolutionsDropdown()
    {
        // Prevent duplicate resolutions (no refresh rates): https://answers.unity.com/questions/1463609/screenresolutions-returning-duplicates.html
        resolutions = Screen.resolutions.Select(resolution => new Resolution { width = resolution.width, height = resolution.height }).Distinct().ToArray<Resolution>();

        List<string> options = new List<string>();
        int currResolutionIdx = 0;

        for (int i = 0; i < resolutions.Length; i++)
        {
            string resOptionStr = resolutions[i].width + " x " + resolutions[i].height;
            options.Add(resOptionStr);

            if (resolutions[i].width == Screen.width &&
                resolutions[i].height == Screen.height)
            {
                currResolutionIdx = i;
            }
        }

        resolutionDropdown.ClearOptions();
        resolutionDropdown.AddOptions(options);

        resolutionDropdown.value = currResolutionIdx;
        resolutionDropdown.RefreshShownValue();
    }

    private void SetResolution(int resolutionIdx)
    {
        Resolution newResolution = resolutions[resolutionIdx];
        Screen.SetResolution(newResolution.width, newResolution.height, fullscreenToggle.isOn);
    }
}
