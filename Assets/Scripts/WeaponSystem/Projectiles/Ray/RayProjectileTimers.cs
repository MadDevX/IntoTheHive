using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RayProjectileTimers : IProjectileTime, IProjectileFixedTime
{
    public float FixedTravelTime => 0.0f;

    public float TravelTime => 0.0f;

    public event Action<float> OnFixedUpdateEvt;
    public event Action<float> OnUpdateEvt;
}
