using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct WaveSpawnParameters
{
    [SerializeField] public List<int> enemyPrefabsIds;
}