using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlState
{
    public float Vertical { get; set; } = 0.0f;
    public float Horizontal { get; set; } = 0.0f;
    public bool PrimaryAction { get; set; } = false;
    public bool SecondaryAction { get; set; } = false;
    public Vector2 Direction { get; set; } = Vector2.up;
    public Vector2 Position { get; set; } = Vector2.zero;
}
