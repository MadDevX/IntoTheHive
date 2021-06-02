using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameLoop.Internal
{
    public class Coroutine : BaseCoroutine
    {
        private Action<float> _action;
        private float _stopAfterTime;
        private bool _automaticTermination = false;

        public Coroutine(ICoroutinePool pool, IGameLoop loop) : base(pool, loop)
        {
        }

        public void Start(Action<float> action, float stopAfterTime)
        {
            StartInternal(action, true, stopAfterTime);
        }

        public void Start(Action<float> action)
        {
            StartInternal(action, false);
        }

        private void StartInternal(Action<float> action, bool automaticTermination, float stopAfterTime = 0.0f)
        {
            _action = action;
            _automaticTermination = automaticTermination;
            _stopAfterTime = stopAfterTime;
            Start();
        }

        public override void OnUpdate(float deltaTime)
        {
            _action?.Invoke(deltaTime);

            AutomaticTermination(deltaTime);
        }

        private void AutomaticTermination(float deltaTime)
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