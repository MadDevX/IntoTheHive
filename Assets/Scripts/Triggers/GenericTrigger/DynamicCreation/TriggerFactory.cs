using UnityEngine;
using Zenject;

public class TriggerFactory : IFactory<TriggerSpawnParameters, TriggerFacade>
{
    private Triggers _triggers;
    private DiContainer _container;

    public TriggerFactory(
        Triggers triggers,
        DiContainer container
        )
    {
        _container = container;
        _triggers = triggers;
    }

    public TriggerFacade Create(TriggerSpawnParameters param)
    {
        var triggerPrefab = _triggers.GetPrefabByID(param.TriggerId);
        var triggerGO = _container.InstantiatePrefab(triggerPrefab);

        Transform transform = triggerGO.transform;
        transform.SetPositionAndRotation(new Vector3(param.X, param.Y, transform.position.z), transform.rotation);

        return triggerGO.GetComponent<TriggerFacade>();
    }
}