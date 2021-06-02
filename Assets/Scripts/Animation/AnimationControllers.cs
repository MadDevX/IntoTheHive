using UnityEngine;
using UnityEngine.Animations;
/// <summary>
/// This class holds animator controllers
/// </summary>
[CreateAssetMenu(menuName = "GameResources/AnimatorControllers")]
public class AnimationControllers : ScriptableObject
{
    [SerializeField] public Object Human;
    [SerializeField] public Object Wasp;
}