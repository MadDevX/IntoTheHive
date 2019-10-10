using GameLoop.Internal;

namespace GameLoop
{
    public abstract class UpdatableObject : GlobalLoopedObject, IUpdatable
    {
        protected override void SubscribeLoopInternal() => _gameLoop.Subscribe(this);
        protected override void UnsubscribeLoopInternal() => _gameLoop.Unsubscribe(this);
        public abstract void OnUpdate(float deltaTime);
    }

    public abstract class FixedUpdatableObject : GlobalLoopedObject, IFixedUpdatable
    {
        protected override void SubscribeLoopInternal() => _gameLoop.Subscribe(this);
        protected override void UnsubscribeLoopInternal() => _gameLoop.Unsubscribe(this);
        public abstract void OnFixedUpdate(float deltaTime);
    }

    public abstract class LateUpdatableObject : GlobalLoopedObject, ILateUpdatable
    {
        protected override void SubscribeLoopInternal() => _gameLoop.Subscribe(this);
        protected override void UnsubscribeLoopInternal() => _gameLoop.Unsubscribe(this);
        public abstract void OnLateUpdate(float deltaTime);
    }
}
