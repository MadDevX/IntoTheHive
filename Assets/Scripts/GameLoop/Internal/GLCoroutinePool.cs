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

    public class GLCoroutinePool : ICoroutinePool
    {
        private IGameLoop _gameLoop;
        private IRelay _relay;

        private QuickList<GLCoroutine> _activeCoroutines = new QuickList<GLCoroutine>(20);
        private QuickList<GLCoroutine> _pendingCoroutines = new QuickList<GLCoroutine>(20);

        public GLCoroutinePool(IGameLoop gameLoop, IRelay relay)
        {
            _gameLoop = gameLoop;
            _relay = relay;
        }

        public GLCoroutine StartCoroutine(Action<float> action)
        {
            var cor = GetPendingCoroutine();
            cor.Start(action);
            return cor;
        }

        public GLCoroutine StartCoroutine(Action<float> action, float stopAfterTime)
        {
            var cor = GetPendingCoroutine();
            cor.Start(action, stopAfterTime);
            return cor;
        }

        private GLCoroutine GetPendingCoroutine()
        {
            if (_pendingCoroutines.Count > 0)
            {
                var idx = _pendingCoroutines.Count - 1;
                var cor = _pendingCoroutines[idx];
                _pendingCoroutines.RemoveAt(idx);
                return cor;
            }
            else
            {
                var cor = new GLCoroutine(this, _gameLoop, _relay);
                return cor;
            }
        }

        public void HandleCoroutineStart(GLCoroutine cor)
        {
            _activeCoroutines.Add(cor);
        }

        public void HandleCoroutineStop(GLCoroutine cor)
        {
            _activeCoroutines.Remove(cor);
        }
    
    }
}