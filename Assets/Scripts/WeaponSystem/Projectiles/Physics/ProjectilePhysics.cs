using GameLoop;
using System;
using UnityEngine;

//C# class - requires GameObjectContext (hell no) and appropriate installer 
//thus - if projectile will have context either way, then Physics can be pure C#, if Context will not be required - make it MonoBehaviour
//MonoUpdatables will come in handy either way, so why not both
public class ProjectilePhysics : MonoFixedUpdatableObject
{
    public event Action<Collider2D> OnCollisionEnter;
    public Vector2 Velocity { get; set; }
    public Vector2 Position { get => transform.position; set => transform.position = value; }
    public float Radius { get; set; } = 0.125f;

    public bool IsPiercing { get; set; } = false;

    private RaycastHit2D[] _hits = new RaycastHit2D[2]; //what about piercing AND fast projectiles? TODO: check hit count
    private Collider2D _prevCol = null;
    private Vector3 _translateVector = Vector3.zero;
    private int _hitMask;

    private void Awake()
    {
        _hitMask = Layers.Interactable.ToMask();
    }

    //TODO: more properties to define more specific bullet behaviours, such as drag, gravity, etc.

    public override void OnFixedUpdate(float deltaTime)
    {
        var velMag = Velocity.magnitude;
        float remainingDist = velMag * deltaTime;

        Vector2 currentPos = transform.position;

        bool canStop = false;
        int i = 0;

        //TODO: upgrade collision algorithm or strip it off unnecessary calculations
        while (canStop == false && i < 10)
        {
            i++;
            if (i == 5) Debug.LogError("Probably endless loop");
            var count = Physics2D.CircleCastNonAlloc(currentPos, Radius, Velocity, _hits, velMag * deltaTime, _hitMask);

            if (count == 0) //Reset prev collision (enables tidy implementation of bouncing/piercing bullets)
            {
                _prevCol = null;
                canStop = true;
            }
            else if (_prevCol != _hits[0].collider || (count == 2 && _prevCol != _hits[1].collider)) //Only OnCollisionEnter should be useful gameplay-wise
            {
                int j = _prevCol != _hits[0].collider ? 0 : 1;
                if (j == 1) Debug.Log("gotem");
                var hit = _hits[j];
                var col = hit.collider;
                _prevCol = col;
                OnCollisionEnter?.Invoke(col);

                if (IsPiercing == false)
                {
                    var posDiff = (currentPos - hit.point).magnitude;
                    var correctionDist = posDiff - Radius;
                    remainingDist = Math.Max(remainingDist - correctionDist, 0.0f);
                    Velocity = ReflectionVector(Velocity, hit.normal);
                    currentPos = Vector3.MoveTowards(currentPos, hit.point, correctionDist);
                }
            }
            else
            {
                canStop = true;
            }
        }
        transform.position = currentPos + Vector2.ClampMagnitude((Velocity * deltaTime), remainingDist);
    }

    private Vector2 ReflectionVector(Vector2 towardsProjectile, Vector2 surfaceNormal)
    {
        var angle = Vector2.SignedAngle(towardsProjectile, surfaceNormal);
        return towardsProjectile.Rotate(2.0f * angle - 180.0f);
    }
}
