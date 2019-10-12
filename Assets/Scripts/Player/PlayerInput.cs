using GameLoop;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : UpdatableObject
{
    private ControlState _controlState;
    private Transform _transform;
    private Camera _mainCamera;

    public PlayerInput(ControlState controlState, Transform transform, Camera mainCamera)
    {
        _controlState = controlState;
        _transform = transform;
        _mainCamera = mainCamera;
    }

    public override void OnUpdate(float deltaTime)
    {
        _controlState.Horizontal = Input.GetAxis("Horizontal");
        _controlState.Vertical = Input.GetAxis("Vertical");
        _controlState.PrimaryAction = Input.GetButton("PrimaryAction");
        _controlState.Direction = (_mainCamera.ScreenToWorldPoint(Input.mousePosition) - _transform.position).normalized;
    }
}
