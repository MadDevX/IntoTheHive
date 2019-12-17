using UnityEngine;
/// <summary>
/// This class uses level graphs made by graph generators to generate levels
/// </summary>
public class LevelSpawner
{
    private SpawnParametersGenerator _levelGraphTranslator;
    private RoomFacade.Factory _levelRoomFactory;
    private TriggerFacade.Factory _triggerFactory;
    private LevelGraphState _levelGraph;

    public LevelSpawner(
        SpawnParametersGenerator levelGraphTranslator,
        RoomFacade.Factory levelRoomFactory,
        TriggerFacade.Factory triggerFactory,
        LevelGraphState levelGraph)
    {
        _levelGraphTranslator = levelGraphTranslator;
        _levelRoomFactory = levelRoomFactory;
        _triggerFactory = triggerFactory;
        _levelGraph = levelGraph;
    }

    /// <summary>
    /// Spawn the level based on LevelGraphState class.
    /// </summary>
    public void GenerateLevel()
    {        
        var spawnInfo = _levelGraphTranslator.TranslateLevelGraph();
        
        foreach(RoomSpawnParameters spawnParameters in spawnInfo.spawnInfos)
        {
            Debug.Log("roomSpawned");
            _levelRoomFactory.Create(spawnParameters);
            if (spawnParameters.ID == _levelGraph.graph.EndLevelRoomId)
            {
                Debug.Log("SpawnTrigger hit");
                var triggerParams = new TriggerSpawnParameters(_levelGraph.graph.TriggerId, spawnParameters.X, spawnParameters.Y);
                _triggerFactory.Create(triggerParams);
            }
        }

        foreach(RoomSpawnParameters spawnParameters in spawnInfo.doorSpawnInfos)
        {
            _levelRoomFactory.Create(spawnParameters);
        }

    }
    
}