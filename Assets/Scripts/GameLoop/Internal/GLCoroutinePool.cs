using Relays;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameLoop.Internal
{
    public interface ICoroutinePool
    {
        void HandleCoroutineStart(GLCoroutine cor);
        void HandleCoroutineStop(GLCoroutine cor);
    }

    public class GLCoroutinePool : ICoroutinePool, ICoroutineManager
    {
        private IGameLoop _gameLoop;
        private IRelay _relay;

        private QuickList<GLCoroutine> _activeCoroutines = new QuickList<GLCoroutine>(20);
        private QuickList<GLCoroutine> _pendingCoroutines = new QuickList<GLCoroutine>(20);

        public GLCoroutinePool(IGameLoop gameLoop)
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

        public void HandleCoroutineStart(GLCoroutine cor)
        {
            _pendingCoroutines.Remove(cor);
            _activeCoroutines.Add(cor);
        }

        public void HandleCoroutineStop(GLCoroutine cor)
        {
            _activeCoroutines.Remove(cor);
            _pendingCoroutines.Add(cor);
        }

        private GLCoroutine GetPendingCoroutine()
        {
            if (_pendingCoroutines.Count > 0)
            {
                return _pendingCoroutines[0];
            }
            else
            {
                var cor = new GLCoroutine(this, _gameLoop, null);
                return cor;
            }
        }


    }
}