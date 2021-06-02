using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class OverlayManager : IInitializable
{
    private GameObject _overlay;
    private SceneGameplayProperties _properties;

    public OverlayManager([Inject(Id = Identifiers.Overlay)] GameObject overlay, SceneGameplayProperties properties)
    {
        _overlay = overlay;
        _properties = properties;
    }

    public void Initialize()
    {
        _overlay.SetActive(_properties.WeaponsEditable == false);
    }
}
