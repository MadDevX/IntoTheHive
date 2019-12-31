using DarkRift;
using DarkRift.Client.Unity;
using System;
using System.Collections.Generic;
using Zenject;

public class NetworkedCharacterSpawner: IInitializable, IDisposable
{
    public event Action<List<PlayerSpawnData>> OnDataPrepared;

    private GenericMessageWithResponseClient _messageWithResponse;
    private GlobalHostPlayerManager _globalHostPlayerManager;
    private CharacterSpawner _characterSpawner;
    private NetworkRelay _networkRelay;
    private UnityClient _client;

    public NetworkedCharacterSpawner(
        GenericMessageWithResponseClient messageWithResponse,
        GlobalHostPlayerManager globalHostPlayerManager,
        CharacterSpawner characterSpawner,
        NetworkRelay networkRelay,
        UnityClient client
        )
    {
        _globalHostPlayerManager = globalHostPlayerManager;
        _messageWithResponse = messageWithResponse;
        _characterSpawner = characterSpawner;
        _networkRelay = networkRelay;
        _client = client;
    }

    public void Initialize()
    {
        // TODO MG: SET TYPE OF CHARACTERS SPAWNED IN THIS WAY AI
        _networkRelay.Subscribe(Tags.SpawnAI, HandleSpawnAiNetworkedCharacter);
        _networkRelay.Subscribe(Tags.DespawnAI, HandleDespawn);

        _networkRelay.Subscribe(Tags.SpawnCharacter, HandleSpawnCharacter);
        _networkRelay.Subscribe(Tags.DespawnCharacter, HandleDespawn);
        _networkRelay.Subscribe(Tags.PlayerDisconnected, HandleDespawn);
    }

    public void Dispose()
    {
        _networkRelay.Unsubscribe(Tags.SpawnAI, HandleSpawnAiNetworkedCharacter);
        _networkRelay.Unsubscribe(Tags.DespawnAI, HandleDespawn);

        _networkRelay.Unsubscribe(Tags.SpawnCharacter, HandleSpawnCharacter);
        _networkRelay.Unsubscribe(Tags.DespawnCharacter, HandleDespawn);
        _networkRelay.Unsubscribe(Tags.PlayerDisconnected, HandleDespawn);
    }

    private void HandleDespawn(Message message)
    {
        using (DarkRiftReader reader = message.GetReader())
        {
            //TODO MG CHECKSIZE
            ushort clientID = reader.ReadUInt16();
            _characterSpawner.Despawn(clientID);
        }
    }

    private void HandleSpawnAiNetworkedCharacter(Message message)
    {
        using (DarkRiftReader reader = message.GetReader())
        {
            //TODO MG CHECKSIZE
            while (reader.Position < reader.Length)
            {
                ushort id = reader.ReadUInt16();
                float X = reader.ReadSingle();
                float Y = reader.ReadSingle();
                var itemCount = reader.ReadInt16();
                var items = new List<short>();
                for (short i = 0; i < itemCount; i++)
                {
                    items.Add(reader.ReadInt16());
                }
                var moduleCount = reader.ReadInt16();
                var modules = new List<short>();
                for (short i = 0; i < moduleCount; i++)
                {
                    modules.Add(reader.ReadInt16());
                }


                CharacterSpawnParameters spawnParameters = new CharacterSpawnParameters();
                spawnParameters.Id = id;
                spawnParameters.X = X;
                spawnParameters.Y = Y;
                spawnParameters.CharacterType = CharacterType.AICharacter;
                spawnParameters.IsLocal = false;
                spawnParameters.items = items;
                spawnParameters.modules = modules;
                _characterSpawner.Spawn(spawnParameters);
            }
            _messageWithResponse.SendClientReady();
        }
    }

    private void HandleSpawnCharacter(Message message)
    {  
        using (DarkRiftReader reader = message.GetReader())
        {
            //TODO MG CHECKSIZE
            while (reader.Position < reader.Length)
            {
                ushort id = reader.ReadUInt16();
                float X = reader.ReadSingle();
                float Y = reader.ReadSingle();
                bool isLocal = (id == _client.ID);
                var itemCount = reader.ReadInt16();
                var items = new List<short>();
                for (short i = 0; i < itemCount; i++)
                {
                    items.Add(reader.ReadInt16());
                }
                var moduleCount = reader.ReadInt16();
                var modules = new List<short>();
                for (short i = 0; i < moduleCount; i++)
                {
                    modules.Add(reader.ReadInt16());
                }


                CharacterSpawnParameters spawnParameters = new CharacterSpawnParameters();
                spawnParameters.Id = id;
                spawnParameters.X = X;
                spawnParameters.Y = Y;
                spawnParameters.CharacterType = CharacterType.Player;
                spawnParameters.IsLocal = isLocal;
                spawnParameters.items = items;
                spawnParameters.modules = modules;
                _characterSpawner.Spawn(spawnParameters);
            }
            _messageWithResponse.SendClientReady();
        }
    }

    public Message GenerateSpawnMessage()
    {
        var list = PrepareSpawnPositions();
        using (DarkRiftWriter writer = DarkRiftWriter.Create())
        {
            foreach (PlayerSpawnData spawnData in list)
            {
                //ID
                writer.Write(spawnData.Id);

                //Position
                writer.Write(spawnData.X);
                writer.Write(spawnData.Y);

                //Item count followed by item Id's
                var itemCount = (short)spawnData.ItemIds.Count;
                writer.Write(itemCount);
                foreach(var data in spawnData.ItemIds)
                {
                    writer.Write(data);
                }

                //Module count followed by module Id's
                var moduleCount = (short)spawnData.WeaponModuleIds.Count;
                writer.Write(moduleCount);
                foreach(var data in spawnData.WeaponModuleIds)
                {
                    writer.Write(data);
                }
            }
            
            return Message.Create(Tags.SpawnCharacter, writer);            
        }
    }

    public Message GenerateDespawnMessage(ushort playerID)
    {
        using (DarkRiftWriter writer = DarkRiftWriter.Create())
        {
            writer.Write(playerID);

            return Message.Create(Tags.DespawnCharacter, writer);
        }
    }

    private List<PlayerSpawnData> PrepareSpawnPositions()
    {

        //TODO MG: REMOVE ASAP: implement other method of determining positions.
        List<PlayerSpawnData> spawnPosisionsList = new List<PlayerSpawnData>();
        if (_globalHostPlayerManager.ConnectedPlayers.Count >= 1)
        {
            var spawnData = new PlayerSpawnData
            {
                Id = _globalHostPlayerManager.ConnectedPlayers[0].ID,
                X = 0.5f,
                Y = 0.5f,
                ItemIds = new List<short>(),
                WeaponModuleIds = new List<short>()
            };
            spawnPosisionsList.Add(spawnData);
        }
        if (_globalHostPlayerManager.ConnectedPlayers.Count >= 2)
        {
            var spawnData = new PlayerSpawnData
            {
                Id = _globalHostPlayerManager.ConnectedPlayers[1].ID,
                X = -0.5f,
                Y = 0.5f,
                ItemIds = new List<short>(),
                WeaponModuleIds = new List<short>()
            };
            spawnPosisionsList.Add(spawnData);
        }
        if (_globalHostPlayerManager.ConnectedPlayers.Count >= 3)
        {
            var spawnData = new PlayerSpawnData
            {
                Id = _globalHostPlayerManager.ConnectedPlayers[2].ID,
                X = 0.5f,
                Y = -0.5f,
                ItemIds = new List<short>(),
                WeaponModuleIds = new List<short>()
            };
            spawnPosisionsList.Add(spawnData);
        }
        if (_globalHostPlayerManager.ConnectedPlayers.Count >= 4)
        {
            var spawnData = new PlayerSpawnData
            {
                Id = _globalHostPlayerManager.ConnectedPlayers[3].ID,
                X = -0.5f,
                Y = -0.5f,
                ItemIds = new List<short>(),
                WeaponModuleIds = new List<short>()
            };
            spawnPosisionsList.Add(spawnData);
        }

        OnDataPrepared?.Invoke(spawnPosisionsList);

        return spawnPosisionsList;
    }


}
