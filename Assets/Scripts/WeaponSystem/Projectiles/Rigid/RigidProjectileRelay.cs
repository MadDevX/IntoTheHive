using Relays.Internal;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RigidProjectileRelay : MonoRelay
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        RaiseOnTrigger2DEnterEvt(collision);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        RaiseOnCollision2DEnterEvt(collision);
    }
}
