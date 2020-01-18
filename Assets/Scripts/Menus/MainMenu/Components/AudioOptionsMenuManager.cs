using Assets.Scripts.Music;
using GameLoop;
using System;
using System.Media;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using Zenject;

public class AudioOptionsMenuManager : UpdatableObject
{
    public AudioMixer _mixer;
    public Slider _musicSlider;
    public Slider _sfxSlider;

    private AudioManager _audioManager;

    protected override bool DefaultSubscribe => false;

    public AudioOptionsMenuManager(AudioMixer mixer, [Inject(Id = Identifiers.MusicSlider)] Slider musicSlider,
        [Inject(Id = Identifiers.SfxSlider)] Slider sfxSlider,
        AudioManager audioManager)
    {
        _mixer = mixer;
        _musicSlider = musicSlider;
        _sfxSlider = sfxSlider;
        _audioManager = audioManager;
    }

    public override void Initialize()
    {
        base.Initialize();
        SetSlidersValue();
        _musicSlider.onValueChanged.AddListener(SetMusicLevel);
        _sfxSlider.onValueChanged.AddListener(SetSFXLevel);
    }

    public override void Dispose()
    {
        base.Dispose();
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

    public void SetSlidersValue()
    {
        _mixer.GetFloat("SFXVolume", out var SFXVolume);
        _mixer.GetFloat("MusicVolume", out var MusicVolume);
        _sfxSlider.value = CalculateSliderValueFromVolume(SFXVolume);
        _musicSlider.value = CalculateSliderValueFromVolume(MusicVolume);
    }


    public float CalculateSliderValueFromVolume(float volume)
    {
        return Mathf.Pow(10, volume / 20);
    }
    
    public void SetSFXLevel(float sliderValue)
    {
        _mixer.SetFloat("SFXVolume",CalculateSoundVolume(sliderValue));
        SubscribeLoop();
    }
    public void SetMusicLevel(float sliderValue)
    {
        _mixer.SetFloat("MusicVolume", CalculateSoundVolume(sliderValue));
    }

    public override void OnUpdate(float deltaTime)
    {
        if(Input.GetMouseButtonUp(0))
        {
            _audioManager.Play(Sound.BulletShoot);
            UnsubscribeLoop();
        }
    }
}




