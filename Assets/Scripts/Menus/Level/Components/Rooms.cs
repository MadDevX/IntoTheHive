
using UnityEngine;
/// <summary>
/// This class holds all rooms' prefabs
/// </summary>
[CreateAssetMenu(menuName = "GameResources/Rooms")]
public class Rooms: ScriptableObject
{
    [SerializeField] public Object BasicRoom;
    //[SerializeField] public Object Door;

    public Object GetRoomById(ushort roomId)
    {
        if (roomId == 0) return BasicRoom;
      //  if (roomId == 1) return Door;
        else return BasicRoom;
    }
}