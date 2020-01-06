﻿using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class VideoOptionsMenuManager : IInitializable
{
    public TMP_Dropdown _resolutionDropdown;
    public Toggle _fullscreenToggle;
    private Resolution[] resolutions;
    public VideoOptionsMenuManager([Inject(Id = Identifiers.Fullscreen)] Toggle fullscreenToggle, [Inject(Id = Identifiers.ResolutionsDropdown)] TMP_Dropdown resolutionDropdown)
    {
        _resolutionDropdown = resolutionDropdown;
        _fullscreenToggle = fullscreenToggle;
        resolutions = Screen.resolutions;
    }

    public void Initialize()
    {
        PrepareResolutionDropdown();
        _fullscreenToggle.isOn = Screen.fullScreen;
        _fullscreenToggle.onValueChanged.AddListener(SetFullscreen);
        _resolutionDropdown.onValueChanged.AddListener(SetResolution);
    }

    public void Dispose()
    {
        _fullscreenToggle.onValueChanged.RemoveListener(SetFullscreen);
        _resolutionDropdown.onValueChanged.RemoveListener(SetResolution);
    }

    public void SetFullscreen(bool isFullscreen)
    {
        Screen.fullScreen = isFullscreen;
    }

    public void PrepareResolutionDropdown()
    {
        _resolutionDropdown.ClearOptions();
        var options = new List<string>();
        var currentResolution = 0;
        for (int i = 0; i < resolutions.Length; i++)
        {
            options.Add($"{resolutions[i].width} x {resolutions[i].height}");

            if (resolutions[i].width == Screen.width && resolutions[i].height == Screen.height)
                currentResolution = i;
        }
        _resolutionDropdown.AddOptions(options);
        _resolutionDropdown.value = currentResolution;
        _resolutionDropdown.RefreshShownValue();
    }

    public void SetResolution(int resolutionIndex)
    {
        var chosenResolution = resolutions[resolutionIndex];
        Screen.SetResolution(chosenResolution.width, chosenResolution.height, Screen.fullScreen);
    }
}