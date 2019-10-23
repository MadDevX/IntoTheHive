using GameLoop.Internal;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameLoop
{
    public abstract class MonoUpdatableObject : MonoGlobalLoopedObject, IUpdatable
    {
        protected override void SubscribeLoopInternal() => _gameLoop.Subscribe(this);
        protected override void UnsubscribeLoopInternal() => _gameLoop.Unsubscribe(this);
        public abstract void OnUpdate(float deltaTime);
    }

    public abstract class MonoFixedUpdatableObject : MonoGlobalLoopedObject, IFixedUpdatable
    {
        protected override void SubscribeLoopInternal() => _gameLoop.Subscribe(this);
        protected override void UnsubscribeLoopInternal() => _gameLoop.Unsubscribe(this);
        public abstract void OnFixedUpdate(float deltaTime);
    }

    public abstract class MonoLateUpdatableObject : MonoGlobalLoopedObject, ILateUpdatable
    {
        protected override void SubscribeLoopInternal() => _gameLoop.Subscribe(this);
        protected override void UnsubscribeLoopInternal() => _gameLoop.Unsubscribe(this);
        public abstract void OnLateUpdate(float deltaTime);
    }
}