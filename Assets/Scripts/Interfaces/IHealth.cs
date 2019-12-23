using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IHealth
{
    float MaxHealth { get; }
    float Health { get; }
}

public interface IHealthSetter : IHealth
{
    new float Health { get; set; }
}