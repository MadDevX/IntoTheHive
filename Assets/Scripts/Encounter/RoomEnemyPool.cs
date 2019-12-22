using System.Collections.Generic;

public class RoomEnemyInfoPool
{
    // make this an editable list that can have inserted values
    private List<RoomAISpawner> _roomAISpawners = new List<RoomAISpawner>();

    public RoomEnemyInfoPool()
    {
        // TODO MG: PLACEHOLDER IMPLEMENTATION
        _roomAISpawners.Add(new RoomAISpawner());
    }

    public List<AISpawnParameters> GetEnemySpawnInfos()
    {
        List<AISpawnParameters> spawnParameters = new List<AISpawnParameters>();
        _roomAISpawners.ForEach(roomSpawner => 
            { 
                spawnParameters.AddRange(roomSpawner.GetWaveData()); 
            });

        return spawnParameters;
    }
}