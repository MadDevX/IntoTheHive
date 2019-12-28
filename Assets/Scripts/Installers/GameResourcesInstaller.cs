using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

[CreateAssetMenu(menuName = "GameResources/Installer")]
public class GameResourcesInstaller : ScriptableObjectInstaller
{
    [SerializeField] private ItemDatabase _database;
    public override void InstallBindings()
    {
        Container.BindInstance(_database).AsSingle();
    }
}
