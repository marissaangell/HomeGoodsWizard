using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Events;
using UnityEngine.Audio;

public class PreferenceLoader : MonoBehaviour
{
    [SerializeField] public InputActionAsset actions;
    [SerializeField] public AudioMixer audioMixer;

    public void OnEnable()
    {
        var rebinds = PlayerPrefs.GetString("rebinds");
        if (!string.IsNullOrEmpty(rebinds))
        {
            actions.LoadBindingOverridesFromJson(rebinds);
        }

    }

    public void OnDisable()
    {
        var rebinds = actions.SaveBindingOverridesAsJson();
        PlayerPrefs.SetString("rebinds", rebinds);
    }

    

    /*
     * Load saved mixer volume levels
     */
    void Start()
    {
        // Master volume
        float masterVolume = PlayerPrefs.HasKey("MasterVolume") ? PlayerPrefs.GetFloat("MasterVolume") : -6;
        audioMixer.SetFloat("MasterVolume", masterVolume);

        // Music
        float musicVolume = PlayerPrefs.HasKey("MusicVolume") ? PlayerPrefs.GetFloat("MusicVolume") : -6;
        audioMixer.SetFloat("MusicVolume", musicVolume);

        // SFX
        float sfxVolume = PlayerPrefs.HasKey("SFXVolume") ? PlayerPrefs.GetFloat("SFXVolume") : -6;
        audioMixer.SetFloat("SFXVolume", sfxVolume);

        // UI 
        float uiVolume = PlayerPrefs.HasKey("UIVolume") ? PlayerPrefs.GetFloat("UIVolume") : -6;
        audioMixer.SetFloat("UIVolume", uiVolume);
    }
}
