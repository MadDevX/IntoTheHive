using System.Collections.Generic;
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
        var numberOfSkippedResolutions = 0;
        var filteredResolutions = new List<Resolution>();
        for (int i = 0; i < resolutions.Length; i++)
        {
            if (resolutions[i].refreshRate < 50)
            {
                numberOfSkippedResolutions++;
                continue;
            }
            options.Add($"{resolutions[i].width} x {resolutions[i].height} @{resolutions[i].refreshRate}Hz");

            if (resolutions[i].width == Screen.width && resolutions[i].height == Screen.height &&
                resolutions[i].refreshRate == Screen.currentResolution.refreshRate)
            {
                currentResolution = i - numberOfSkippedResolutions;
            }
            filteredResolutions.Add(resolutions[i]);
        }

        _resolutionDropdown.AddOptions(options);
        _resolutionDropdown.value = currentResolution;
        resolutions = filteredResolutions.ToArray();
        _resolutionDropdown.RefreshShownValue();
    }

    public void SetResolution(int resolutionIndex)
    {
        var chosenResolution = resolutions[resolutionIndex];
        Screen.SetResolution(chosenResolution.width, chosenResolution.height, Screen.fullScreen);
    }
}