using Relays;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameLoop.Internal
{
    public class GLCoroutine : UpdatableObject, ICoroutine
    {
        private Action<float> _action;

        private float _stopAfterTime;
        private float _timer = 0.0f;
        private bool _automaticTermination = false;

        private ICoroutinePool _pool;

        public GLCoroutine(ICoroutinePool pool, IGameLoop loop, IRelay relay)
        {
            _pool = pool;
            Construct(loop, relay);
        }

        protected override bool DefaultSubscribe => false;

        public void Start(Action<float> action, float stopAfterTime)
        {
            _stopAfterTime = stopAfterTime;
            _timer = 0.0f;
            _automaticTermination = true;

            Start(action);
        }

        public void Start(Action<float> action)
        {
            _action = action;
            SubscribeLoop();
            _pool.HandleCoroutineStart(this);
        }

        public void Stop()
        {
            _action = null;
            _automaticTermination = false;
            UnsubscribeLoop();
            _pool.HandleCoroutineStop(this);
        }

        public override void OnUpdate(float deltaTime)
        {
            _action?.Invoke(deltaTime);

            AutomaticTermination(deltaTime);
        }

        protected void AutomaticTermination(float deltaTime)
        {
            if (_automaticTermination)
            {
                _timer += deltaTime;
                if (_timer >= _stopAfterTime)
                {
                    Stop();
                }
            }
        }
    }
}