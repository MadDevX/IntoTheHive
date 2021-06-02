using GameLoop;
using Zenject;

public class TriggerFacade: MonoUpdatableObject
{
    public override void OnUpdate(float deltaTime)
    {
    }

    public class Factory : PlaceholderFactory<TriggerSpawnParameters, TriggerFacade>
    {

    }
}