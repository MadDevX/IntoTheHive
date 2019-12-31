using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageManager
{
    public bool NegateDamage => _settings.negateDamage;
    private Settings _settings;

    public DamageManager(Settings settings)
    {
        _settings = settings;
    }

    [System.Serializable]
    public class Settings
    {
        public bool negateDamage;
    }
}
