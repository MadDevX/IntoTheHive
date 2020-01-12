public static class Tags
{
    //Tag 0 is not used to not worry about uninitialized Messages
    //Character 
    public static readonly ushort SpawnCharacter = 1;
    public static readonly ushort DespawnCharacter = 2;
    public static readonly ushort DisposeCharacter = 3;
    public static readonly ushort UpdateCharacterState = 4;
    public static readonly ushort UpdateCharacterEquipment = 5;
    public static readonly ushort WeaponChanged = 6;

    //SceneManagment
    public static readonly ushort UpdateLobby = 7;
    public static readonly ushort RequestUpdateLobby = 8;
    public static readonly ushort ChangeScene = 9;
    public static readonly ushort GameStarted = 10;
    public static readonly ushort SceneReady = 11;

    //Connections
    public static readonly ushort ConnectionInfo = 12;
    public static readonly ushort PlayerJoined = 13;
    public static readonly ushort PlayerDisconnected = 14;
    public static readonly ushort IsPlayerReady = 15;
    public static readonly ushort LoadLobby = 16;
    public static readonly ushort ChangeSceneWithReply = 17;

    //Level
    public static readonly ushort RequestSpawn = 18;
    public static readonly ushort LevelGraph = 19;
    public static readonly ushort ClientReady = 20;
    public static readonly ushort SpawnProjectile = 21;
    public static readonly ushort UpdateGameState = 22;

    //Triggers
    public static readonly ushort EndLevelTrigger = 23;
    public static readonly ushort OpenDoorsMessage = 24;
    public static readonly ushort CloseDoorsMessage = 25;
    
    //AI
    public static readonly ushort SpawnAI = 26;
    public static readonly ushort DespawnAI = 27;
    
    //Damage
    public static readonly ushort TakeDamage = 28;
    public static readonly ushort UpdateHealth = 29;
    public static readonly ushort DeathRequest = 30;
    
    //Pickups
    public static readonly ushort SpawnPickup = 31;
    public static readonly ushort DespawnPickup = 32;
    public static readonly ushort RequestSpawnPickup = 33;
    public static readonly ushort AssignItem = 34;
    public static readonly ushort RemoveItem = 35;

}