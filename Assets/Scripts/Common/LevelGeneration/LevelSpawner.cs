/// <summary>
/// This class uses level graphs made by graph generators to generate levels
/// </summary>
public class LevelSpawner
{
    private SpawnParametersGenerator _levelGraphTranslator;
    private RoomFacade.Factory _levelRoomFactory;

    public LevelSpawner(
        SpawnParametersGenerator levelGraphTranslator,
        RoomFacade.Factory levelRoomFactory)
    {
        _levelRoomFactory = levelRoomFactory;
        _levelGraphTranslator = levelGraphTranslator;
    }

    /// <summary>
    /// Spawn the level based on LevelGraphState class.
    /// </summary>
    public void GenerateLevel()
    {        
        var spawnInfo = _levelGraphTranslator.TranslateLevelGraph();
        
        foreach(RoomSpawnParameters spawnParameters in spawnInfo.spawnInfos)
        {
            _levelRoomFactory.Create(spawnParameters);
        }

        foreach(RoomSpawnParameters spawnParameters in spawnInfo.doorSpawnInfos)
        {
            _levelRoomFactory.Create(spawnParameters);
        }

    }
    
}