using System.Collections.Generic;
using UnityEngine;

public class RoomEnemyInfoPool: MonoBehaviour
{
    // make this an editable list that can have inserted values
    public  List<RoomAISpawner> _roomAISpawners = new List<RoomAISpawner>();

    public List<AISpawnParameters> GetEnemySpawnInfos()
    {
        List<AISpawnParameters> spawnParameters = new List<AISpawnParameters>();
        _roomAISpawners.ForEach(roomSpawner =>
            {
                var wave = roomSpawner.GetWaveData();
                if (wave != null)
                    spawnParameters.AddRange(wave);
            });

        return spawnParameters;
    }
}