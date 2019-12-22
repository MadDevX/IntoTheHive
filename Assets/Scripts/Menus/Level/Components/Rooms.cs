
using System;
using UnityEngine;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;

/// <summary>
/// This class holds all rooms' prefabs
/// </summary>
[CreateAssetMenu(menuName = "GameResources/Rooms")]
public class Rooms: ScriptableObject
{
    [SerializeField] public Object BasicRoom;
    [SerializeField] public Object Door;
    [SerializeField] public Object ExitRoom;
    [SerializeField] public Object Room1;
    [SerializeField] public Object Room2;
    [SerializeField] public Object Room3;
    [SerializeField] public Object Room4;
    [SerializeField] public Object Room5;
    [SerializeField] public Object Room6;
    [SerializeField] public Object Room7;
    [SerializeField] public Object Room8;
    [SerializeField] public Object Room9;
    [SerializeField] public Object Room10;
    [SerializeField] public Object Room11;
    [SerializeField] public Object Room12;
    [SerializeField] public Object Room13;
    [SerializeField] public Object Room14;
    [SerializeField] public Object Room15;
    

    public Object GetRoomById(ushort roomId)
    {
        if (roomId == 0) return BasicRoom;
        if (roomId == 1) return Door;
        if (roomId == 2) return ExitRoom;
        if (roomId == 3) return Room1;
        if (roomId == 4) return Room2;
        if (roomId == 5) return Room3;
        if (roomId == 6) return Room4;
        if (roomId == 7) return Room5;
        if (roomId == 8) return Room6;
        if (roomId == 9) return Room7;
        if (roomId == 10) return Room8;
        if (roomId == 11) return Room9;
        if (roomId == 12) return Room10;
        if (roomId == 13) return Room11;
        if (roomId == 14) return Room12;
        if (roomId == 15) return Room13;
        if (roomId == 16) return Room14;
        if (roomId == 17) return Room15;
        else return BasicRoom;
    }

    public static ushort GetExitRoom() => 2;
    public static ushort GetStartingRoom() => 0;
    public static ushort GetRandomRoom() => Convert.ToUInt16(Random.Range(3, 17));

}