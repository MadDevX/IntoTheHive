using GameLoop;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

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
        _controlState.Horizontal = Input.GetAxis(nameof(_controlState.Horizontal));
        _controlState.Vertical = Input.GetAxis(nameof(_controlState.Vertical));
        if (EventSystem.current.IsPointerOverGameObject() == false)
        {
            _controlState.PrimaryAction = Input.GetButton(nameof(_controlState.PrimaryAction));
            _controlState.SecondaryAction = Input.GetButton(nameof(_controlState.SecondaryAction));
        }
        _controlState.Direction = (_mainCamera.ScreenToWorldPoint(Input.mousePosition) - _transform.position).normalized;
    }
}
