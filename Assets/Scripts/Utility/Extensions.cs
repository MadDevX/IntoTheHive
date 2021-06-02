using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Extensions 
{
    public static Vector2 Rotate(this Vector2 v, float degrees)
    {
        float sin = Mathf.Sin(degrees * Mathf.Deg2Rad);
        float cos = Mathf.Cos(degrees * Mathf.Deg2Rad);

        float tx = v.x;
        float ty = v.y;
        v.x = (cos * tx) - (sin * ty);
        v.y = (sin * tx) + (cos * ty);
        return v;
    }

    public static float Rotation(this Vector2 v)
    {
        return Vector2.SignedAngle(Vector2.up, v);
    }

    public static Vector2 ReflectionVector(Vector2 towardsProjectile, Vector2 surfaceNormal)
    {
        var angle = Vector2.SignedAngle(towardsProjectile, surfaceNormal);
        return towardsProjectile.Rotate(2.0f * angle - 180.0f);
    }

    public static int ToMask(this int i)
    {
        return 1 << i;
    }
}
