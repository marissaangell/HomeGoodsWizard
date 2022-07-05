using System;
using UnityEngine;
using UnityEngine.Audio;

// Used Brackeys' "Introduction to Audio in Unity" video as reference
// https://www.youtube.com/watch?v=6OT43pvUyfY
public class SoundManager : MonoBehaviour
{
    public Sound[] sounds;
    public AudioSource oneShotSFXSource;
    public AudioMixerGroup musicMixerGroup;

    [Header("Player Footsteps")]
    public AudioClip[] footsteps;
    public float footstepVolumeScale = 0.5f;

    private System.Random random;

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
        foreach (Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.outputAudioMixerGroup = s.mixerGroup;
            s.source.playOnAwake = false;

            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;
        }

        random = new System.Random();
    }

    public void Play(string name)
    {
        Sound s = GetSound(name);
        s?.source?.Play();
    }

    public void PlayIfNotAlready(string name)
    {
        Sound s = GetSound(name);

        if (s.source != null && !s.source.isPlaying)
        {
            s?.source?.Play();
        }
    }

    public void PlayOneShot(string name)
    {
        if (name == "FOOTSTEP")
        {
            if (random == null) { random = new System.Random(); }
            int clipIdx = random.Next(0, footsteps.Length - 1);
            instance.oneShotSFXSource.PlayOneShot(instance.footsteps[clipIdx], instance.footstepVolumeScale);
        }
        else
        {
            Sound s = GetSound(name);
            s?.source?.PlayOneShot(s.source.clip);
        }
    }

    public void Stop(string name)
    {
        Sound s = GetSound(name);
        s?.source?.Stop();
    }

    public void StopAllMusic()
    {
        foreach (Sound s in instance.sounds)
        {
            if (s.source.outputAudioMixerGroup != null && s.source.outputAudioMixerGroup.Equals(instance.musicMixerGroup))
            {
                s.source.Stop();
            }
        }
    }

    private Sound GetSound(string name)
    {
        Sound s = Array.Find(instance.sounds, sound => sound.name == name);

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
    public float volume;

    [Range(.1f, 3)]
    public float pitch = 1;

    public bool loop;

    [HideInInspector] public AudioSource source;
}