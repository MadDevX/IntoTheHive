using System;
using DarkRift;
using Zenject;

public class LevelGraphMessageReceiver: IInitializable, IDisposable
{
    private NetworkRelay _relay;
    private LevelGraphState _graphState;
    private LevelSpawner _levelSpawner;

    public LevelGraphMessageReceiver(
        NetworkRelay relay,
        LevelGraphState graphState,
        LevelSpawner levelSpawner)
    {
        _relay = relay;
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
                ushort RoomId = reader.ReadUInt16();
                ushort north = reader.ReadUInt16();
                ushort west = reader.ReadUInt16();
                ushort east = reader.ReadUInt16();
                ushort south = reader.ReadUInt16();
                _graphState.graph.AddVertex(RoomId, north, west, east, south);
            }
        }

        _levelSpawner.GenerateLevel();
    }

}