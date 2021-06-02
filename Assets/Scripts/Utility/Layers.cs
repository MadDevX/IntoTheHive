using UnityEngine;

public static class Layers
{
    public static readonly int Default;
    public static readonly int Interactable;
    public static readonly int Projectile;
    public static readonly int Environment;

    static Layers()
    {
        SetupLayer(nameof(Default), ref Default);
        SetupLayer(nameof(Interactable), ref Interactable);
        SetupLayer(nameof(Projectile), ref Projectile);
        SetupLayer(nameof(Environment), ref Environment);
    }

    private static void SetupLayer(string name, ref int value)
    {
        value = LayerMask.NameToLayer(name);
    }
}
