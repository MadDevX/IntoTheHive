using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RayProjectileCollisionHandler : IProjectileCollision, IProjectileCollisionHandler
{
    public bool IsPiercing { get; set; }

    public event Action<Collider2D> OnCollisionEnter;

    public void HandleCollision(Collider2D col)
    {
        OnCollisionEnter?.Invoke(col);
    }

}
