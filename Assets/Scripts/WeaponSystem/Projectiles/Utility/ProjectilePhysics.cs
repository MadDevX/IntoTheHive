using GameLoop;
using System;
using UnityEngine;

//C# class - requires GameObjectContext (hell no) and appropriate installer 
//thus - if projectile will have context either way, then Physics can be pure C#, if Context will not be required - make it MonoBehaviour
//MonoUpdatables will come in handy either way, so why not both
public class ProjectilePhysics : MonoFixedUpdatableObject
{
    [SerializeField] private Projectile _projectile;

    public event Action<Projectile, RaycastHit2D, int> OnCollisionEnter;
    public Vector2 velocity { get; set; }
    public Vector2 position { get => transform.position; set => transform.position = value; }
    public float Radius { get; set; } = 0.125f;

    public bool isPiercing = false;
    public int RemainingCollisions { get; set; } = 0;

    private RaycastHit2D[] _hits = new RaycastHit2D[5]; //what about piercing AND fast projectiles? TODO: check hit count
    private Collider2D _prevCol = null;
    private Vector2 _zeroVector = Vector2.zero;
    private int _hitMask;

    private void Awake()
    {
        _hitMask = Layers.Interactable.ToMask();
    }

    //TODO: more properties to define more specific bullet behaviours, such as drag, gravity, etc.

    public override void OnFixedUpdate(float deltaTime)
    {
        if (isPiercing)
        {
            TriggerBehaviour(deltaTime);
        }
        else
        {
            CollisionBehaviour(deltaTime);
        }
    }

    private void CollisionBehaviour(float deltaTime)
    {
        var velMag = velocity.magnitude;
        float remainingDist = velMag * deltaTime;

        Vector2 currentPos = transform.position;

        bool canStop = false;
        int i = 0;

        //TODO: upgrade collision algorithm or strip it off unnecessary calculations
        while (canStop == false && i < 10)
        {
            i++;
            if (i == 10) Debug.LogError("Probably endless loop");
            var count = Physics2D.CircleCastNonAlloc(currentPos, Radius, velocity, _hits, velMag * deltaTime, _hitMask);

            if (count == 0) //Reset prev collision (enables tidy implementation of bouncing/piercing bullets)
            {
                _prevCol = null;
                canStop = true;
            }
            else if (_prevCol != _hits[0].collider || (count >= 2)) //Only OnCollisionEnter should be useful gameplay-wise
            {
                int j;
                for(j = 0; j < count; j++)
                {
                    if (Vector2.Dot(_hits[j].normal, velocity) <= 0.0f && _prevCol != _hits[j].collider) break;
                }
                var hit = _hits[j];
                var col = hit.collider;
                _prevCol = col;
                var posDiff = (currentPos - hit.point).magnitude;
                var correctionDist = posDiff - Radius - Constants.COLLISION_CORRECTION_EPS; //so that previous collisions won't be taken into consideration by raycast (eliminate slight overlap)
                remainingDist = Math.Max(remainingDist - correctionDist, 0.0f);
                var hitRb = hit.rigidbody;
                var baseVel = _zeroVector;
                if (hitRb != null) baseVel = hitRb.velocity;
                velocity = Extensions.ReflectionVector((velocity - baseVel), hit.normal) + baseVel;
                currentPos = Vector3.MoveTowards(currentPos, hit.point, correctionDist);
                RemainingCollisions--;
                transform.position = currentPos;
                OnCollisionEnter?.Invoke(_projectile, hit, RemainingCollisions);
            }
            else
            {
                canStop = true;
            }
        }
        transform.position = currentPos + Vector2.ClampMagnitude((velocity * deltaTime), remainingDist);
    }

    private void TriggerBehaviour(float deltaTime)
    {
        var velMag = velocity.magnitude;
        float remainingDist = velMag * deltaTime;

        Vector2 currentPos = transform.position;

        var count = Physics2D.CircleCastNonAlloc(currentPos, Radius, velocity, _hits, remainingDist, _hitMask);
        if(count == 0)
        {
            _prevCol = null;
        }
        else if (_hits[0].collider != _prevCol)
        {
            _prevCol = _hits[0].collider;
            for (int i = 0; i < count; i++)
            {
                RemainingCollisions--;
                OnCollisionEnter?.Invoke(_projectile, _hits[i], RemainingCollisions); //For visual effects maybe forward RaycastHit instead of Collider?
                Debug.Log(_hits[i].collider.name);
            }
        }
        transform.position = currentPos + Vector2.ClampMagnitude((velocity * deltaTime), remainingDist);

    }
}
