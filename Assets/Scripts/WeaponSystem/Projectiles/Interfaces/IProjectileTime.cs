using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IProjectileTime
{
    float TravelTime { get; }
    event Action<float> OnUpdateEvt;
}
