using System;
using UnityEngine;

public interface IProjectileCollision
{
    event Action<Collider2D> OnCollisionEnter;
    bool IsPiercing { get; set; }
}