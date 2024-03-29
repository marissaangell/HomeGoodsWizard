using System;
using UnityEngine;
using UnityEngine.Audio;

// Written using Brackeys' "Introduction to Audio in Unity" video as reference
// https://www.youtube.com/watch?v=6OT43pvUyfY
public class SoundManager : MonoBehaviour
{
    [SerializeField] private SoundDatabase soundDatabase;
    [SerializeField] private AudioSource oneShotSFXSource;

    // Singleton instance
    public static SoundManager instance;

    void Awake()
    {
        // Singleton check
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
        DontDestroyOnLoad(this); //persist through scene changes

        // Instantiate an audiosource component for each sound in the sounds array
        foreach (Sound s in soundDatabase.sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.outputAudioMixerGroup = s.mixerGroup;
            s.source.playOnAwake = false;

            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;
        }
    }

    public static void Play(string name)
    {
        if (string.IsNullOrEmpty(name)) { return; }

        if (instance != null)
        {
            Sound s = instance.GetSound(name);
            s?.source?.Play();
        }
    }

    /*public void PlayIfNotAlready(string name)
    {
        if (string.IsNullOrEmpty(name)) { return; }

        Sound s = GetSound(name);
        if (s != null && s.source != null && !s.source.isPlaying)
        {
            s.source.Play();
        }
    }

    public void PlayOneShot(string name)
    {
        if (string.IsNullOrEmpty(name)) { return; }

        Sound s = GetSound(name);
        s?.source?.PlayOneShot(s.source.clip);
    }*/

    public static void Stop(string name)
    {
        if (string.IsNullOrEmpty(name)) { return; }

        if (instance != null)
        {
            Sound s = instance.GetSound(name);
            s?.source?.Stop();
        }
    }

    private Sound GetSound(string name)
    {
        //Sound s = Array.Find(instance.sounds, sound => sound.name == name);
        Sound s = Array.Find(instance.soundDatabase.sounds, sound => sound.name == name);

        if (s == null || s.source == null)
        {
            Debug.Log("Sound " + name + " not found");
        }

        return s;
    }

}

[System.Serializable]
public class Sound
{
    public string name;

    public AudioClip clip;
    public AudioMixerGroup mixerGroup;

    [Range(0, 1)]
    public float volume = 1;

    [Range(.1f, 3)]
    public float pitch = 1;

    public bool loop;

    [HideInInspector] public AudioSource source;
}