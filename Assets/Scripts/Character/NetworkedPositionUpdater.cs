using GameLoop;
using UnityEngine;

public class NetworkedPositionUpdater : FixedUpdatableObject
{
    private Rigidbody2D _rb;
    private ControlState _controlState;

    public NetworkedPositionUpdater(
        Rigidbody2D rigidbody,
        ControlState controlState)
    {
        _rb = rigidbody;
        _controlState = controlState;
    }

    public override void OnFixedUpdate(float deltaTime)
    {
        _controlState.Position = _rb.position;
    }
}