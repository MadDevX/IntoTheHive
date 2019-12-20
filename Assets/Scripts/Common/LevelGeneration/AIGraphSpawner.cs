using Pathfinding;
using System;
using UnityEngine;

public class AIGraphSpawner
{
    private LevelSpawnParameters _levelSpawnParameters;
    private SpawnParametersGenerator.Settings _roomSettings;
    private Settings _settings;
    private AstarPath _aStar;

    public AIGraphSpawner(
        LevelSpawnParameters spawnParameters,
        SpawnParametersGenerator.Settings roomSettings,
        Settings settings,
        AstarPath aStar)
    {
        _levelSpawnParameters = spawnParameters;
        _roomSettings = roomSettings;
        _settings = settings;
        _aStar = aStar;
    }

    public void AddLevelGraphs()
    {
        var roomInfos = _levelSpawnParameters.roomSpawnInfos;

        // Spawn an AStar GridGraph in every room
        foreach(RoomSpawnParameters info in roomInfos)
        {
            float X = _roomSettings.roomSize / _settings.nodeSize;
            float Y = _roomSettings.roomSize / _settings.nodeSize;
            // Add a graph to the graph list 
            GridGraph roomGraph = _aStar.data.AddGraph(typeof(GridGraph)) as GridGraph;
            // Modify the graph to represent one room
            roomGraph.name = "Room " + info.ID;
            roomGraph.center.Set(info.X, info.Y, roomGraph.center.z);
            roomGraph.SetDimensions((int)X, (int)Y, _settings.nodeSize);
            // This line makes the 2D checkbox, under Shape field, enabled
            roomGraph.rotation = new Vector3(-90, 0, 0);
            // Represents the 3rd "paragraph" in AStar editor view
            var collisionSettings = roomGraph.collision;
            { 
                collisionSettings.use2D = true;
                collisionSettings.collisionCheck = true;
                //TODO MG : figure out something more... clean
                collisionSettings.mask = LayerMask.GetMask("Environment");
                collisionSettings.diameter = _settings.diameter;
            }
        }
        // Scans all active graphs to make them usable
        AstarPath.active.Scan();
        
    }

    [System.Serializable]
    public class Settings
    {
        public float nodeSize = 0.5f;
        public float diameter = 2f;
    }

}