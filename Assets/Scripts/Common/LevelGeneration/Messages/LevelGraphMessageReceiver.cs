using System;
using DarkRift;
using Zenject;

public class LevelGraphMessageReceiver: IInitializable, IDisposable
{
    private NetworkRelay _relay;
    private LevelGraphState _graphState;
    private LevelSpawner _levelSpawner;
    private GenericMessageWithResponseClient _sender;


    public LevelGraphMessageReceiver(
        NetworkRelay relay,
        LevelGraphState graphState,
        GenericMessageWithResponseClient sender,
        LevelSpawner levelSpawner)
    {
        _relay = relay;
        _sender = sender;
        _levelSpawner = levelSpawner;
        _graphState = graphState;
    }

    public void Initialize()
    {
        _relay.Subscribe(Tags.LevelGraph, HandleLevelGraphChanged);
    }

    public void Dispose()
    {
        _relay.Unsubscribe(Tags.LevelGraph, HandleLevelGraphChanged);
    }

    private void HandleLevelGraphChanged(Message message)
    {
        using (DarkRiftReader reader = message.GetReader())
        {
            //TODO MG CHECKSIZE

            _graphState.graph.Reset();
            while(reader.Position < reader.Length)
            {
                short RoomId = reader.ReadInt16();
                short north = reader.ReadInt16();
                short west = reader.ReadInt16();
                short east = reader.ReadInt16();
                short south = reader.ReadInt16();
                _graphState.graph.AddVertex(RoomId, north, west, east, south);
            }
        }

        _levelSpawner.GenerateLevel();
        _sender.SendClientReady();
    }

}