using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class VideoOptionsMenuInstaller : MonoInstaller
{
    [SerializeField] private Toggle FullscreenToggle;
    [SerializeField] private TMP_Dropdown ResolutionsDropdown;
    public override void InstallBindings()
    {
        InstallLogic();
        InstallElements();
    }

    private void InstallElements()
    {
        Container.BindInterfacesAndSelfTo<VideoOptionsMenuManager>().AsSingle();
    }
    
    private void InstallLogic()
    {
        Container.BindInstance(FullscreenToggle).WithId(Identifiers.Fullscreen).AsSingle();
        Container.BindInstance(ResolutionsDropdown).WithId(Identifiers.ResolutionsDropdown).AsSingle();
    }
}
