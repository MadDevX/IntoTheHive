using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ITargetUpdatable
{
    Transform GetTarget(Collider2D[] hits);
}