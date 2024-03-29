﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IProjectile
{
    event Action<IProjectile, float> OnUpdateEvt;
    event Action<IProjectile, float> OnFixedUpdateEvt;
    event Action<IProjectile, Collider2D, int> OnCollisionEnter;

    ModifierList DamageModifiers { get; }

    ProjectilePhasePipeline Pipeline { get; }
    Vector2 Position { get; }
    Vector2 Velocity { get; set; }
    float TravelTime { get; }
    float FixedTravelTime { get; }
    bool IsPiercing { get; set; }
    bool IsDummy { get; }
    int CollisionLimit { get; set; }

    void Destroy();
}
