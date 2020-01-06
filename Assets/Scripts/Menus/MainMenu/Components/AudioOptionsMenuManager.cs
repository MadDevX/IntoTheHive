using System;
using System.Media;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using Zenject;

public class AudioOptionsMenuManager : IInitializable
{
    public AudioMixer _mixer;
    public Slider _musicSlider;
    public Slider _sfxSlider;

    public AudioOptionsMenuManager(AudioMixer mixer, [Inject(Id = Identifiers.MusicSlider)] Slider musicSlider,
        [Inject(Id = Identifiers.SfxSlider)] Slider sfxSlider)
    {
        _mixer = mixer;
        _musicSlider = musicSlider;
        _sfxSlider = sfxSlider;

    }

    public void Initialize()
    {
        _musicSlider.onValueChanged.AddListener(SetMusicLevel);
        _sfxSlider.onValueChanged.AddListener(SetSFXLevel);
    }

    public void Dispose()
    {
        _musicSlider.onValueChanged.RemoveListener(SetMusicLevel);
        _sfxSlider.onValueChanged.RemoveListener(SetSFXLevel);
    }

    public float CalculateSoundVolume(float sliderValue)
    {
        // Sound volume is logarithmic, so slider is from 0.0001 to 1
        // log10(0.0001) = -4 , log10(1) = 0
        // This lets us map it to values from -80 to 0
        return Mathf.Log10(sliderValue) * 20;
    }
    
    public void SetSFXLevel(float sliderValue)
    {
        _mixer.SetFloat("SFXVolume",CalculateSoundVolume(sliderValue));
    }
    public void SetMusicLevel(float sliderValue)
    {
        _mixer.SetFloat("MusicVolume", CalculateSoundVolume(sliderValue));
    }

}




