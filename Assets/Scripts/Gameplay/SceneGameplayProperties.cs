using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneGameplayProperties
{
    public bool NegateDamage => _settings.negateDamage;
    public bool WeaponsEditable => _settings.weaponsEditable;
    private Settings _settings;

    public SceneGameplayProperties(Settings settings)
    {
        _settings = settings;
    }

    [System.Serializable]
    public class Settings
    {
        public bool negateDamage;
        public bool weaponsEditable;
    }
}
