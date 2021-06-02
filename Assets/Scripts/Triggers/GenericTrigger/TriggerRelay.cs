using Relays.Internal;
using UnityEngine;

public class TriggerRelay: MonoRelay
{
    public void OnTriggerEnter2D(Collider2D collision)
    {
        RaiseOnTrigger2DEnterEvt(collision);
    }

    public void OnTriggerExit2D(Collider2D collision)
    {
        RaiseOnTrigger2DExitEvt(collision);
    }
}