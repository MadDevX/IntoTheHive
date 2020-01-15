using Relays;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameLoop.Internal
{
    public abstract class BaseCoroutine : UpdatableObject, ICoroutine
    {
        protected float _timer = 0.0f;

        private ICoroutinePool _pool;

        protected override bool DefaultSubscribe => false;

        public BaseCoroutine(ICoroutinePool pool, IGameLoop loop)
        {
            _pool = pool;
            Construct(loop, null);
        }

        /// <summary>
        /// Attaches coroutine to game loop and informs coroutine pool that coroutine has started.
        /// </summary>
        protected void Start()
        {
            _timer = 0.0f;
            SubscribeLoop();
            _pool.HandleCoroutineStart(this);
        }


        /// <summary>
        /// Detaches coroutine from game loop and informs coroutine pool that coroutine has stopped.
        /// </summary>
        public void Stop()
        {
            UnsubscribeLoop();
            _pool.HandleCoroutineStop(this);
        }
    }
}