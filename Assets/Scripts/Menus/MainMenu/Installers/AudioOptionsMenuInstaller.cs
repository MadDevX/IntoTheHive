using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using Zenject;

public class AudioOptionsMenuInstaller : MonoInstaller
{
    [SerializeField] private Slider _musicSlider;
    [SerializeField] private Slider _sfxSlider;

    public override void InstallBindings()
    {
        InstallLogic();
        InstallElements();
    }

    public void InstallLogic()
    {
        Container.BindInterfacesAndSelfTo<AudioOptionsMenuManager>().AsSingle();
    }
    public void InstallElements()
    {
        Container.BindInstance(_musicSlider).WithId(Identifiers.MusicSlider).AsCached();
        Container.BindInstance(_sfxSlider).WithId(Identifiers.SfxSlider).AsCached();
    }
}
