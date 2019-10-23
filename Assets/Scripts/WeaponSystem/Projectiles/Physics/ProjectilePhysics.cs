using GameLoop;
using System;
using UnityEngine;

//C# class - requires GameObjectContext (hell no) and appropriate installer 
//thus - if projectile will have context either way, then Physics can be pure C#, if Context will not be required - make it MonoBehaviour
//MonoUpdatables will come in handy either way, so why not both
public class ProjectilePhysics : MonoFixedUpdatableObject
{
    public event Action<Collider2D> OnCollisionEnter;
    public Vector2 Velocity { get; set; } = new Vector2(40.0f, 40.0f);
    public float Radius { get; set; } = 0.125f;

    public bool IsPiercing { get; set; } = false;

    private RaycastHit2D[] _hits = new RaycastHit2D[1]; //what about piercing AND fast projectiles? TODO: check hit count
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
        Vector2 basePos = transform.position;
        var count = Physics2D.CircleCastNonAlloc(basePos, Radius, Velocity, _hits, velMag * deltaTime, _hitMask);

        float remainingDist = velMag * deltaTime;

        if (count == 0) //Reset prev collision (enables tidy implementation of bouncing/piercing bullets)
        {
            _prevCol = null;
        }
        else if(_prevCol != _hits[0].collider) //Only OnCollisionEnter should be useful gameplay-wise
        {
            var hit = _hits[0];
            var col = hit.collider;
            _prevCol = col;
            OnCollisionEnter?.Invoke(col);
            
            if(IsPiercing == false)
            {
                var posDiff = (basePos - hit.point).magnitude;
                var correctionDist = posDiff - Radius;
                remainingDist = Math.Max(remainingDist - correctionDist, 0.0f);
                Velocity = ReflectionVector(Velocity, hit.normal);
                basePos = Vector3.MoveTowards(basePos, hit.point, correctionDist);
            }
        }
        Vector3 endPos = basePos + Vector2.ClampMagnitude((Velocity * deltaTime), remainingDist); //Reflections ideally should be made in while loop
        transform.position = endPos;
    }

    private Vector2 ReflectionVector(Vector2 towardsProjectile, Vector2 surfaceNormal)
    {
        var angle = Vector2.SignedAngle(towardsProjectile, surfaceNormal);
        return towardsProjectile.Rotate(180.0f - 2.0f * angle);
    }
}
