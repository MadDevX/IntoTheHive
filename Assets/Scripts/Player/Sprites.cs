using UnityEngine;

/// <summary>
/// This class holds all doors' prefabs
/// </summary>
[CreateAssetMenu(menuName = "GameResources/Sprites")]
public class Sprites : ScriptableObject
{
    [SerializeField] public Sprite Human;
    [SerializeField] public Sprite Wasp;
}