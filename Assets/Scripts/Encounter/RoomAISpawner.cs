using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Placeholder
/// </summary>
[System.Serializable]
public class RoomAISpawner : MonoBehaviour
{
    // WaveSpawnParameters list
    [SerializeField] public List<WaveSpawnParameters> waves = new List<WaveSpawnParameters>();

    private int _waveCounter = 0;

    public List<AISpawnParameters> GetWaveData()
    {

        if (_waveCounter < waves.Count)
        {
            List<AISpawnParameters> list = new List<AISpawnParameters>();
            var wave = waves[_waveCounter];
            int positionCounter = 0;
            // for each enemy in the wave, add him to the spawned characters
            wave.enemyPrefabsIds.ForEach(
                (enemy) =>
                {
                    // generate spawn parameters for the enemy
                    var param = new AISpawnParameters();
                    var position = GetSpawnPosition(positionCounter);
                    positionCounter++;

                    param.EnemyPrefabId = enemy;
                    param.SpawnParameters = new CharacterSpawnParameters()
                    {
                        CharacterType = CharacterType.AICharacter,
                        X = position.x,
                        Y = position.y
                    };
                    list.Add(param);
                });
            _waveCounter++;
            return list;
        }
        else return null;
            
    }

    private (float x, float y) GetSpawnPosition(int counter)
    {
        float X = this.transform.position.x;
        float Y = this.transform.position.y;

        switch (counter)
        {
            case 0:
                X += 0.5f;
                Y += 0.5f;
                break;
            case 1:
                X -= 0.5f;
                Y += 0.5f;
                break;
            case 2:
                X += 0.5f;
                Y -= 0.5f;
                break;
            case 3:
                X -= 0.5f;
                Y -= 0.5f;
                break;
            default:
                break;
        }
    
        return (X, Y);
    }
}