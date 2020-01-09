using System;
using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Music;
using UnityEngine;
using UnityEngine.Audio;
using Zenject;

public class AudioManager : MonoBehaviour, IInitializable
{
    private AudioMixer _mixer;
    public SoundClip[] sounds;

    [Inject]
    public void Construct(AudioMixer audioMixer)
    {
        _mixer = audioMixer;
        
    }
    public void Initialize()
    {
        
        var musicGroup = _mixer.FindMatchingGroups("Master/Music")[0];
        var sfxGroup = _mixer.FindMatchingGroups("Master/SFX")[0];
        
        for (int i = 0; i < sounds.Length; i++)
        {
            sounds[i].Source = gameObject.AddComponent<AudioSource>();
            sounds[i].Source.clip = sounds[i].Clip;
            sounds[i].Source.loop = sounds[i].Loop;
            sounds[i].Source.outputAudioMixerGroup =
                sounds[i].SoundType == SoundType.Music ? musicGroup : sfxGroup;
        }
    }


    public void Play(Sound soundName)
    {
        SoundClip s = Array.Find(sounds, item => item.Name == soundName);
        if (s == null)
        {
            Debug.LogWarning("Sound: " + soundName + " not found!");
            return;
        }
        s.Source.Play();
        
    }

    public void PlayIfNotPlaying(Sound soundName)
    {
        SoundClip s = Array.Find(sounds, item => item.Name == soundName);
        if (s == null)
        {
            Debug.LogWarning("Sound: " + soundName + " not found!");
            return;
        }
        if(!s.Source.isPlaying)
            s.Source.Play();
    }

    public void StopAll(SoundType soundType)
    {
        for(int i = 0; i < sounds.Length; i++)
        {
            if (sounds[i].SoundType == soundType)
                sounds[i].Source.Stop();
        }
    }
    public void PauseAll(SoundType soundType)
    {
        for (int i = 0; i < sounds.Length; i++)
        {
            if (sounds[i].SoundType == soundType)
                sounds[i].Source.Pause();
        }
    }
    public void UnPauseAll(SoundType soundType)
    {
        for (int i = 0; i < sounds.Length; i++)
        {
            if (sounds[i].SoundType == soundType)
                sounds[i].Source.UnPause();
        }
    }
    public void StopAll()
    {
        for (int i = 0; i < sounds.Length; i++)
        {
            sounds[i].Source.Stop();
        }
    }
    public void PauseAll()
    {
        for (int i = 0; i < sounds.Length; i++)
        {
            sounds[i].Source.Pause();
        }
    }
    public void UnPauseAll()
    {
        for (int i = 0; i < sounds.Length; i++)
        {
            sounds[i].Source.UnPause();
        }
    }

    public void Stop(Sound soundName)
    {
        SoundClip s = Array.Find(sounds, item => item.Name == soundName);
        if (s == null)
        {
            Debug.LogWarning("Sound: " + soundName + " not found!");
            return;
        }
        s.Source.Stop();
    }

}


