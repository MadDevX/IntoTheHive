using UnityEngine;

public interface IProjectileCollisionHandler
{
    void HandleCollision(Collider2D col);
}