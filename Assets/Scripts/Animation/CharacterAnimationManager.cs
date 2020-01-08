using GameLoop;
using UnityEngine;

/// <summary>
/// Updates Animation Parameters and flips sprites if necessary
/// </summary>
public class CharacterAnimationManager : FixedUpdatableObject
{
    private Animator _animator;
    private SpriteRenderer _characterRenderer;
    private Rigidbody2D _rb;
    private ControlState _controlState;
    public CharacterAnimationManager(
        Animator animator,
        SpriteRenderer renderer,
        Rigidbody2D rb,
        ControlState controlState
        )
    {
        _animator = animator;
        _characterRenderer = renderer;
        _rb = rb;
        _controlState = controlState;
    }

    public override void OnFixedUpdate(float deltaTime)
    {
        _animator?.SetFloat("Speed", _rb.velocity.magnitude);
        _characterRenderer.flipX = _controlState.Direction.x > 0;

    }

}