using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IProjectileFixedTime
{
    float FixedTravelTime { get; }

    event Action<float> OnFixedUpdateEvt;
}
