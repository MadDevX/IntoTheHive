using System;
using UnityEngine;

public interface IProjectileCollision
{

    event Action<Collider2D> OnCollisionEnter;
    /// <summary>
    /// Used by observers that could adversely influence logic of observers attached to OnCollisionEnter
    /// </summary>
    event Action AfterCollisionEnter;
    bool IsPiercing { get; set; }
}