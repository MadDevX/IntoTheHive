using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IHealth : IDamageable //TODO: maybe inheritance is not a good idea, but for now it works
{
    event Action OnDeath;
    float MaxHealth { get; }
    float Health { get; }
}
