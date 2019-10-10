using System;
using UnityEngine;

namespace Relays
{
    public interface IRelay
    {
        bool IsActive { get; }
        event Action OnAwakeEvt;
        event Action OnStartEvt;
        event Action OnEnableEvt;
        event Action OnDisableEvt;
        event Action OnDestroyEvt;
        event Action<Collider> OnTriggerEnterEvt;
        event Action<Collider> OnTriggerStayEvt;
        event Action<Collider> OnTriggerExitEvt;
        event Action<Collider2D> OnTrigger2DEnterEvt;
        event Action<Collider2D> OnTrigger2DStayEvt;
        event Action<Collider2D> OnTrigger2DExitEvt;
        event Action<Collision> OnCollisionEnterEvt;
        event Action<Collision> OnCollisionStayEvt;
        event Action<Collision> OnCollisionExitEvt;
        event Action<Collision2D> OnCollision2DEnterEvt;
        event Action<Collision2D> OnCollision2DStayEvt;
        event Action<Collision2D> OnCollision2DExitEvt;
    }
}
