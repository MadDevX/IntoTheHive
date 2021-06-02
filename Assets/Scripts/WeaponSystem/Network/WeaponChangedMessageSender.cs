using DarkRift;
using DarkRift.Client.Unity;
using System;
using System.Collections.Generic;
using Zenject;

public class WeaponChangedMessageSender: IDisposable
{
    private IWeapon _weapon;
    private UnityClient _unityClient;
    private CharacterFacade _characterFacade;
    public WeaponChangedMessageSender(
        IWeapon weapon,
        UnityClient unityClient,
        CharacterFacade characterFacade)
    {
        _weapon = weapon;
        _unityClient = unityClient;
        _characterFacade = characterFacade;
    }

    [Inject]
    public void Initialize()
    {
        _weapon.OnWeaponRefreshed += SendWeaponChanged;
    }

    public void Dispose()
    {
        _weapon.OnWeaponRefreshed -= SendWeaponChanged;
    }

    private void SendWeaponChanged(List<IModule> modules)
    {
        using (DarkRiftWriter writer = DarkRiftWriter.Create())
        {
            //write message
            writer.Write(_characterFacade.Id);
            modules.ForEach(module => writer.Write(module.Id));

            using(Message message = Message.Create(Tags.WeaponChanged, writer))
            {
                _unityClient.SendMessage(message, SendMode.Reliable);
            }
        }
    }
}