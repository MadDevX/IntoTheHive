using UnityEngine;

public static class Layers
{
    public static readonly int Default;
    public static readonly int Interactable;
    public static readonly int Projectile;

    static Layers()
    {
        SetupLayer(nameof(Default), ref Default);
        SetupLayer(nameof(Interactable), ref Interactable);
        SetupLayer(nameof(Projectile), ref Projectile);
    }

    private static void SetupLayer(string name, ref int value)
    {
        value = LayerMask.NameToLayer(name);
    }
}
