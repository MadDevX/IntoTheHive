using GameLoop;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RigidProjectileTimer : UpdatableObject, IProjectileTime
{
    public float TravelTime { get; private set; }

    public event Action<float> OnUpdateEvt;

    private ProjectilePhasePipeline _pipeline;

    protected override bool DefaultSubscribe => false;

    public RigidProjectileTimer(ProjectilePhasePipeline pipeline)
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
    public override void OnUpdate(float deltaTime)
    {
        OnUpdateEvt?.Invoke(TravelTime);
        TravelTime += deltaTime;
    }

    private void StartTimer(ProjectilePipelineParameters parameters)
    {
        TravelTime = 0.0f;
        SubscribeLoop();
    }

    private void StopTimer(ProjectilePipelineParameters parameters)
    {
        UnsubscribeLoop();
    }
}
