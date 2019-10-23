using UnityEngine;

public interface IWeapon
{
    bool Shoot(Vector2 position, float rotation, Vector2 offset);
    void ReleaseTrigger();
    void Reload();
}
