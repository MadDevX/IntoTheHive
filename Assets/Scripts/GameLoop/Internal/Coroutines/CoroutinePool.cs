using Relays;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameLoop.Internal
{
    public interface ICoroutinePool
    {
        void HandleCoroutineStart(BaseCoroutine cor);
        void HandleCoroutineStop(BaseCoroutine cor);
    }

    public class CoroutinePool : ICoroutinePool, ICoroutineManager
    {
        private IGameLoop _gameLoop;
        private IRelay _relay;

        private QuickList<Coroutine> _activeCoroutines = new QuickList<Coroutine>(20);
        private QuickList<Coroutine> _pendingCoroutines = new QuickList<Coroutine>(20);

        private QuickList<ExecuteAfterDelay> _activeDelayedExecutes = new QuickList<ExecuteAfterDelay>(20);
        private QuickList<ExecuteAfterDelay> _pendingDelayedExecutes = new QuickList<ExecuteAfterDelay>(20);

        public CoroutinePool(IGameLoop gameLoop)
        {
            _gameLoop = gameLoop;
        }

        public ICoroutine StartCoroutine(Action<float> action, float stopAfterTime)
        {
            var cor = GetPendingCoroutine();
            cor.Start(action, stopAfterTime);
            return cor;
        }

        public ICoroutine StartCoroutine(Action<float> action)
        {
            var cor = GetPendingCoroutine();
            cor.Start(action);
            return cor;
        }

        public ICoroutine ExecuteAfterDelay(Action action, float delay)
        {
            var exec = GetPendingExecuteAfterDelay();
            exec.Start(action, delay);
            return exec;
        }

        #region Pool interface

        public void HandleCoroutineStart(BaseCoroutine baseCor)
        {
            if (baseCor is Coroutine)
            {
                var cor = baseCor as Coroutine;
                _pendingCoroutines.Remove(cor);
                _activeCoroutines.Add(cor);
            }
            else if(baseCor is ExecuteAfterDelay)
            {
                var exec = baseCor as ExecuteAfterDelay;
                _pendingDelayedExecutes.Remove(exec);
                _activeDelayedExecutes.Add(exec);
            }
        }

        public void HandleCoroutineStop(BaseCoroutine baseCor)
        {
            if (baseCor is Coroutine)
            {
                var cor = baseCor as Coroutine;
                _activeCoroutines.Remove(cor);
                _pendingCoroutines.Add(cor);
            }
            else if (baseCor is ExecuteAfterDelay)
            {
                var exec = baseCor as ExecuteAfterDelay;
                _activeDelayedExecutes.Remove(exec);
                _pendingDelayedExecutes.Add(exec);
            }
        }

        #endregion

        #region GetPendingCoroutine

        private Coroutine GetPendingCoroutine()
        {
            if (_pendingCoroutines.Count > 0)
            {
                return _pendingCoroutines[0];
            }
            else
            {
                var cor = new Coroutine(this, _gameLoop);
                return cor;
            }
        }

        private ExecuteAfterDelay GetPendingExecuteAfterDelay()
        {
            if(_pendingDelayedExecutes.Count > 0)
            {
                return _pendingDelayedExecutes[0];
            }
            else
            {
                var exec = new ExecuteAfterDelay(this, _gameLoop);
                return exec;
            }
        }

        #endregion
    }
}