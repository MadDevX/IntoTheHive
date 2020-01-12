public static class Tags
{
    //Character 
    public static readonly ushort SpawnCharacter = 0;
    public static readonly ushort DespawnCharacter = 1;
    public static readonly ushort DisposeCharacter = 2;
    public static readonly ushort UpdateCharacterState = 3;
    public static readonly ushort UpdateCharacterEquipment = 4;
    public static readonly ushort WeaponChanged = 5;

    //SceneManagment
    public static readonly ushort UpdateLobby = 6;
    public static readonly ushort RequestUpdateLobby = 7;
    public static readonly ushort ChangeScene = 8;
    public static readonly ushort GameStarted = 9;
    public static readonly ushort SceneReady = 10;

    //Connections
    public static readonly ushort ConnectionInfo = 11;
    public static readonly ushort PlayerJoined = 12;
    public static readonly ushort PlayerDisconnected = 13;
    public static readonly ushort IsPlayerReady = 14;
    public static readonly ushort LoadLobby = 15;
    public static readonly ushort ChangeSceneWithReply = 16;

    //Level
    public static readonly ushort RequestSpawn = 17;
    public static readonly ushort LevelGraph = 18;
    public static readonly ushort ClientReady = 19;
    public static readonly ushort SpawnProjectile = 20;
    public static readonly ushort UpdateGameState = 21;

    //Triggers
    public static readonly ushort EndLevelTrigger = 22;
    public static readonly ushort OpenDoorsMessage = 23;
    public static readonly ushort CloseDoorsMessage = 24;
    
    //AI
    public static readonly ushort SpawnAI = 25;
    public static readonly ushort DespawnAI = 26;
    
    //Damage
    public static readonly ushort TakeDamage = 27;
    public static readonly ushort UpdateHealth = 28;
    public static readonly ushort DeathRequest = 29;
    
    //Pickups
    public static readonly ushort SpawnPickup = 30;
    public static readonly ushort DespawnPickup = 31;
    public static readonly ushort RequestSpawnPickup = 32;
    public static readonly ushort AssignItem = 33;
    public static readonly ushort RemoveItem = 34;

}