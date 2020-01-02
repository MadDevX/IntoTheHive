using GameLoop;
using UnityEngine;

public class CharacterSpriteFixedRotation : LateUpdatableObject
{

    private SpriteRenderer _characterSprite;
    public CharacterSpriteFixedRotation(
        SpriteRenderer characterSprite)
    {
        _characterSprite = characterSprite;
    }

    public override void OnLateUpdate(float deltaTime)
    {
        _characterSprite.transform.rotation = Quaternion.identity;
    }
}