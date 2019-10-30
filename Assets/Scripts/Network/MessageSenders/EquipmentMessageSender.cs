using DarkRift;
using DarkRift.Client.Unity;

class EquipmentMessageSender
{
    private CharacterEquipment _characterEquipment;
    private UnityClient _networkManager;
    private CharacterFacade _characterFacade;

    public EquipmentMessageSender(
        UnityClient client,
        CharacterEquipment characterEquipment,
        CharacterFacade characterFacade
        )
    {
        _networkManager = client;
        _characterEquipment = characterEquipment;
        _characterFacade = characterFacade;
        _characterEquipment.OnWeaponChanged += SendWeaponChangedMessage;
    }

    public void SendWeaponChangedMessage(IWeapon newWeapon)
    {
        using(DarkRiftWriter writer = DarkRiftWriter.Create())
        {
            writer.Write(_characterFacade.Id);
            // Write message contents
            // TODO MG: decide how should the modules be "serialized"
            // 5 ushorts with '0' for empty slot?

            using (Message message = Message.Create(Tags.UpdateCharacterEquipment, writer))
            {
                _networkManager.SendMessage(message,SendMode.Reliable);
            }
        }
    }
}
