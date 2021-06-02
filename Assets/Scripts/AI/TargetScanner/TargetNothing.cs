using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetNothing : ITargetUpdatable
{
    public Transform GetTarget(Collider2D[] hits)
    {
        return null;
    }
}
