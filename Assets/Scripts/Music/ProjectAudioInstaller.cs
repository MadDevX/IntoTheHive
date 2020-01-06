using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using Zenject;

public class ProjectAudioInstaller : MonoInstaller
{
    [SerializeField] private AudioMixer _mixer;
    [SerializeField] private AudioManager _audioManager;
    public override void InstallBindings()
    {
        Container.BindInterfacesAndSelfTo<AudioMixer>().FromInstance(_mixer);
        Container.BindInterfacesAndSelfTo<AudioManager>().FromInstance(_audioManager);
    }
}



