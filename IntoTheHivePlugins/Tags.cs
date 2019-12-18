public static class Tags
{
    public static readonly ushort SpawnCharacter = 0;
    public static readonly ushort DespawnCharacter = 1;
    
    public static readonly ushort UpdateCharacterState = 2;
    public static readonly ushort UpdateCharacterEquipment = 3;
    public static readonly ushort UpdateLobby = 4;
    public static readonly ushort RequestUpdateLobby = 6;

    public static readonly ushort ChangeScene = 5;
    public static readonly ushort GameStarted = 7;
    public static readonly ushort SceneReady = 8;
    public static readonly ushort ConnectionInfo = 9;

    public static readonly ushort PlayerJoined = 10;
    public static readonly ushort PlayerDisconnected = 11;
    public static readonly ushort IsPlayerReady = 12;
    public static readonly ushort LoadLobby = 13;
    public static readonly ushort ChangeSceneWithReply = 14;

    public static readonly ushort RequestSpawn = 15;
    public static readonly ushort LevelGraph = 16;

    public static readonly ushort ClientReady = 17;

    //Triggers
    public static readonly ushort EndLevelTrigger = 18;

    public static readonly ushort OpenDoorsMessage = 19;
    public static readonly ushort CloseDoorsMessage = 20;

    public static readonly ushort WeaponChanged = 21;
    
    //AI
    public static readonly ushort SpawnAI = 22;
    public static readonly ushort DespawnAI = 23;


}