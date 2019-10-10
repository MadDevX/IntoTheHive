using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Relays.Internal
{
    public class Collisions2DMonoRelay : MonoRelay
    {
        private void OnCollisionEnter2D(Collision2D collision)
        {
            RaiseOnCollision2DEnterEvt(collision);
        }

        private void OnCollisionStay2D(Collision2D collision)
        {
            RaiseOnCollision2DStayEvt(collision);
        }

        private void OnCollisionExit2D(Collision2D collision)
        {
            RaiseOnCollision2DExitEvt(collision);
        }
    }
}