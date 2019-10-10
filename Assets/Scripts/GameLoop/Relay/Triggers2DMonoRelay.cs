using UnityEngine;

namespace Relays.Internal
{
    public class Triggers2DMonoRelay : MonoRelay
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