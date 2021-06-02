using GameLoop;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RigidProjectileFixedTimer : FixedUpdatableObject, IProjectileFixedTime
{
    public float FixedTravelTime { get; private set; }

    public event Action<float> OnFixedUpdateEvt;

    private ProjectilePhasePipeline _pipeline;

    protected override bool DefaultSubscribe => false;

    public RigidProjectileFixedTimer(ProjectilePhasePipeline pipeline)
    {
        _pipeline = pipeline;
        PreInitialize();
    }

    private void PreInitialize()
    {
        _pipeline.SubscribeToInit(ProjectilePhases.Created, StartTimer);
        _pipeline.SubscribeToInit(ProjectilePhases.Destroyed, StopTimer);
    }

    public override void Dispose()
    {
        base.Dispose();
        _pipeline.UnsubscribeFromInit(ProjectilePhases.Created, StartTimer);
        _pipeline.UnsubscribeFromInit(ProjectilePhases.Destroyed, StopTimer);

    }
    public override void OnFixedUpdate(float deltaTime)
    {
        OnFixedUpdateEvt?.Invoke(FixedTravelTime);
        FixedTravelTime += deltaTime;
    }

    private void StartTimer(ProjectilePipelineParameters parameters)
    {
        FixedTravelTime = 0.0f;
        SubscribeLoop();
    }

    private void StopTimer(ProjectilePipelineParameters parameters)
    {
        UnsubscribeLoop();
    }
}
