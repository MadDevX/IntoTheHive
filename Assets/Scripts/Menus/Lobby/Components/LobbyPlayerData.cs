public class LobbyPlayerData
{
    public string Nickname;
    public bool Ready = false;

    public LobbyPlayerData(string nickname, bool ready)
    {
        Nickname = nickname;
        Ready = ready;
    }
}