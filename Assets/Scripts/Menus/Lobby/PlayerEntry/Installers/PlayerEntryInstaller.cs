using UnityEngine;
using UnityEngine.UI;
using Zenject;

/// <summary>
/// Installs components necessary to add, hide and handle player entry in lobby.
/// </summary>
public class PlayerEntryInstaller: MonoInstaller
{
    [SerializeField] private Text _nameText;
    [SerializeField] private Text _readyStatusText;
    [SerializeField] private Text _notReadyStatusText;

    public override void InstallBindings()
    {
        InstallTextFields();
    }

    public void InstallTextFields()
    {
        Container.BindInstance(_nameText).WithId(Identifiers.PlayerEntryName);
        Container.BindInstance(_readyStatusText).WithId(Identifiers.PlayerEntryReady);
        Container.BindInstance(_notReadyStatusText).WithId(Identifiers.PlayerEntryNotReady);
    }    
}