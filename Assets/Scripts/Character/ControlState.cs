using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlState
{
    /// <summary>
    /// State of the vertical movement axis, takes values between -1 (down) and 1 (up).
    /// </summary>
    public float Vertical { get; set; } = 0.0f;

    /// <summary>
    /// State of the horizontal movement axis, takes values between -1 (left) and 1 (right).
    /// </summary>
    public float Horizontal { get; set; } = 0.0f;

    /// <summary>
    /// Boolean representing whether or not the character should execute its primary action.
    /// </summary>
    public bool PrimaryAction { get; set; } = false;

    /// <summary>
    /// Boolean representing whether or not the character should execute its secondary action.
    /// </summary>
    public bool SecondaryAction { get; set; } = false;

    /// <summary>
    /// Vector representing the direction which character should be facing.
    /// </summary>
    public Vector2 Direction { get; set; } = Vector2.up;
    public Vector2 Position { get; set; } = Vector2.zero;
}
