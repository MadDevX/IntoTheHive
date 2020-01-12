using DarkRift;
using DarkRift.Client.Unity;

class EquipmentMessageSender
{
    private UnityClient _networkManager;
    private CharacterFacade _characterFacade;

    public EquipmentMessageSender(
        UnityClient client,
        CharacterFacade characterFacade
        )
    {
        _networkManager = client;
        _characterFacade = characterFacade;
    }

    public void SendWeaponChangedMessage(IWeapon newWeapon)
    {
        using(DarkRiftWriter writer = DarkRiftWriter.Create())
        {
            writer.Write(_characterFacade.Id);
            
            using (Message message = Message.Create(Tags.UpdateCharacterEquipment, writer))
            {
                _networkManager.SendMessage(message,SendMode.Reliable);
            }
        }
    }
}
