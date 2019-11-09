using DarkRift;
using DarkRift.Client.Unity;

public class NetworkedCharacterEquipment
{
    //Discuss How to handle creation on non damaging bullets
    //New Factories

    //Inject Weapon Factory 
    //Dictionary <ushort, WeaponComponents>

    private IWeapon Weapon { get; set; }
    private UnityClient _client;
    private CharacterSpawner _characterSpawner;
    private CharacterFacade _characterFacade;
    private NetworkRelay _networkRelay;

    public NetworkedCharacterEquipment(
        UnityClient client,
        CharacterSpawner characterSpawner,
        CharacterFacade characterFacade,
        NetworkRelay networkRelay)
    {
        _characterFacade = characterFacade;
        _characterSpawner = characterSpawner;
        _client = client;
        _networkRelay = networkRelay;
        _networkRelay.Subscribe(Tags.UpdateCharacterEquipment, ParseMessage);
    }

    public void ParseMessage(Message message)
    {

        using (DarkRiftReader reader = message.GetReader())
        {

            ushort incomingClientId = reader.ReadUInt16();
            if (incomingClientId == _characterFacade.Id)
            {
                // TODO How to store data about weapons
                // do something with NetworkedCharacterEquipment
                // read content
                object arguments = null;
                //Weapon.CreateWeapon(arguments);
            }
        }
    }
}