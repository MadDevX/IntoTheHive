using UnityEngine;
using System;

namespace Relays.Internal
{
    public class MonoRelay : MonoBehaviour, IRelay
    {
        public bool IsActive => gameObject.activeInHierarchy;

        public event Action OnAwakeEvt;
        public event Action OnStartEvt;
        public event Action OnEnableEvt;
        public event Action OnDisableEvt;
        public event Action OnDestroyEvt;
        public event Action<Collider> OnTriggerEnterEvt;
        public event Action<Collider> OnTriggerStayEvt;
        public event Action<Collider> OnTriggerExitEvt;
        public event Action<Collider2D> OnTrigger2DEnterEvt;
        public event Action<Collider2D> OnTrigger2DStayEvt;
        public event Action<Collider2D> OnTrigger2DExitEvt;
        public event Action<Collision> OnCollisionEnterEvt;
        public event Action<Collision> OnCollisionStayEvt;
        public event Action<Collision> OnCollisionExitEvt;
        public event Action<Collision2D> OnCollision2DEnterEvt;
        public event Action<Collision2D> OnCollision2DStayEvt;
        public event Action<Collision2D> OnCollision2DExitEvt;

        protected void RaiseOnTriggerEnterEvt(Collider col) => OnTriggerEnterEvt?.Invoke(col);
        protected void RaiseOnTriggerStayEvt(Collider col) => OnTriggerStayEvt?.Invoke(col);
        protected void RaiseOnTriggerExitEvt(Collider col) => OnTriggerExitEvt?.Invoke(col);
        protected void RaiseOnTrigger2DEnterEvt(Collider2D col) => OnTrigger2DEnterEvt?.Invoke(col);
        protected void RaiseOnTrigger2DStayEvt(Collider2D col) => OnTrigger2DStayEvt?.Invoke(col);
        protected void RaiseOnTrigger2DExitEvt(Collider2D col) => OnTrigger2DExitEvt?.Invoke(col);
        protected void RaiseOnCollisionEnterEvt(Collision col) => OnCollisionEnterEvt?.Invoke(col);
        protected void RaiseOnCollisionStayEvt(Collision col) => OnCollisionStayEvt?.Invoke(col);
        protected void RaiseOnCollisionExitEvt(Collision col) => OnCollisionExitEvt?.Invoke(col);
        protected void RaiseOnCollision2DEnterEvt(Collision2D col) => OnCollision2DEnterEvt?.Invoke(col);
        protected void RaiseOnCollision2DStayEvt(Collision2D col) => OnCollision2DStayEvt?.Invoke(col);
        protected void RaiseOnCollision2DExitEvt(Collision2D col) => OnCollision2DExitEvt?.Invoke(col);


        private void Awake()
        {
            OnAwakeEvt?.Invoke();
        }

        private void Start()
        {
            OnStartEvt?.Invoke();
        }

        private void OnEnable()
        {
            OnEnableEvt?.Invoke();
        }

        private void OnDisable()
        {
            OnDisableEvt?.Invoke();
        }

        private void OnDestroy()
        {
            OnDestroyEvt?.Invoke();
        }
    }
}