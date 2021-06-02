using System;
using UnityEngine;
/// <summary>
/// This class uses level graphs made by graph generators to generate levels
/// </summary>
public class LevelSpawner
{
    private SpawnParametersGenerator _levelGraphTranslator;
    private RoomFacade.Factory _levelRoomFactory;
    private DoorFacade.Factory _doorFactory;
    private TriggerFacade.Factory _triggerFactory;
    private LevelGraphState _levelGraph;
    private LevelSpawnParameters _levelSpawnParameters;
    private DoorManager _doorManager;
    private AIGraphSpawner _aIGraphSpawner;

    public LevelSpawner(
        SpawnParametersGenerator levelGraphTranslator,
        RoomFacade.Factory levelRoomFactory,
        TriggerFacade.Factory triggerFactory,
        DoorFacade.Factory doorFactory,
        LevelGraphState levelGraph,
        LevelSpawnParameters levelSpawnParameters,
        DoorManager doorManager,
        AIGraphSpawner aIGraphSpawner)
    {
        _levelGraphTranslator = levelGraphTranslator;
        _levelRoomFactory = levelRoomFactory;
        _triggerFactory = triggerFactory;
        _doorFactory = doorFactory;
        _levelGraph = levelGraph;
        _levelSpawnParameters = levelSpawnParameters;
        _doorManager = doorManager;
        _aIGraphSpawner = aIGraphSpawner;
    }

    /// <summary>
    /// Spawn the level based on LevelGraphState class.
    /// </summary>
    public void GenerateLevel()
    {   
        //Updates LevelSpawnParameters
        _levelGraphTranslator.TranslateLevelGraph();

        foreach (RoomSpawnParameters spawnParameters in _levelSpawnParameters.roomSpawnInfos)
        {
            //Debug.Log("roomSpawned");
            _levelRoomFactory.Create(spawnParameters);
            if (spawnParameters.ID == _levelGraph.graph.EndLevelRoomId)
            {
                //Debug.Log("SpawnTrigger hit");
                var triggerParams = new TriggerSpawnParameters(_levelGraph.graph.TriggerId, spawnParameters.X, spawnParameters.Y);
                _triggerFactory.Create(triggerParams);
            }
        }

        foreach(DoorSpawnParameters spawnParameters in _levelSpawnParameters.doorSpawnInfos)
        {
            spawnParameters.isBasic = false;
            _doorFactory.Create(spawnParameters);
        }
        _doorManager.PrepareEdges();
        _aIGraphSpawner.AddLevelGraphs();
        // open spawn roomso that players can exit
        //_doorManager.OpenAllDoorsInRoom(0);
        //_doorManager.OpenAllDoorsInRoom(2);
    }
    
}