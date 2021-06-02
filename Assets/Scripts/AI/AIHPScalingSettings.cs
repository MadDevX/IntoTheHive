using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIHPScalingSettings : IHealthSettings
{
    public float MaxHealth
    {
        get
        {
            return _settings.baseMaxHealth * _winManager.CurrentLevel;
        }
    }

    private WinManager _winManager;
    private Settings _settings;

    public AIHPScalingSettings(WinManager winManager, Settings settings)
    {
        _winManager = winManager;
        _settings = settings;
    }

    [System.Serializable]
    public class Settings
    {
        public float baseMaxHealth;
    }
}
