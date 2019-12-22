using System;
using System.Collections.Generic;
/// <summary>
/// Placeholder
/// </summary>
public class RoomAISpawner
{
    // WaveSpawnParameters list

    public List<AISpawnParameters> GetWaveData()
    {

        //TODO MG : PLACEHOLDER IMPLEMENTATION
        List<AISpawnParameters> list = new List<AISpawnParameters>();
        var param = new AISpawnParameters();
        param.EnemyPrefabId = 0;
        param.SpawnParameters = new CharacterSpawnParameters()
        {
            CharacterType = CharacterType.AICharacter,
            X = 0f,
            Y = 2f
        };

        list.Add(param);
        return list;
    }
}