using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Relays.Internal
{
    public class FullMonoRelay : Collisions2DMonoRelay
    {
        private void OnTriggerEnter2D(Collider2D collision)
        {
            RaiseOnTrigger2DEnterEvt(collision);
        }

        private void OnTriggerStay2D(Collider2D collision)
        {
            RaiseOnTrigger2DStayEvt(collision);
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            RaiseOnTrigger2DExitEvt(collision);
        }
    }
}