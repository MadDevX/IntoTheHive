using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameLoop.Internal
{
    public class ExecuteAfterDelay : BaseCoroutine
    {
        private Action _action;
        private float _delay;

        public ExecuteAfterDelay(ICoroutinePool pool, IGameLoop loop) : base(pool, loop)
        {
        }

        public void Start(Action action, float delay)
        {
            _action = action;
            _delay = delay;
            Start();
        }

        public override void OnUpdate(float deltaTime)
        {
            _timer += deltaTime;
            if(_timer >= _delay)
            {
                _action?.Invoke();
                Stop();
            }
        }
    }
}