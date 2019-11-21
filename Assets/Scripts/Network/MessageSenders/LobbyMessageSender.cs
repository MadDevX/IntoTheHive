using System;
using DarkRift;
using DarkRift.Client.Unity;

// This class sends messages used only inside the lobby
// TODO MG : should this be split into playerLobbyMessageSender and HostLobbyMessageSender?
public class LobbyMessageSender
{
    private LobbyState _lobbyState;
    private UnityClient _client;

    public LobbyMessageSender(
        LobbyState lobbyState,
        UnityClient client)
    {
        _lobbyState = lobbyState;
        _client = client;
    }


    // host only - to change clients scene
    public void SendLoadLobbyMessage(ushort id, ushort sceneIndex)
    {
        using (DarkRiftWriter writer = DarkRiftWriter.Create())
        {
            writer.Write(id);
            writer.Write(sceneIndex);
            using (Message message = Message.Create(Tags.LoadLobby, writer))
            {
                _client.SendMessage(message, SendMode.Reliable);
            }
        }
    }


    // host only
    public void SendUpdateLobbyMessage()
    {
        using (DarkRiftWriter writer = DarkRiftWriter.Create())
        {
            foreach(ushort playerId in _lobbyState.PlayersReadyStatus.Keys)
            {
                writer.Write(playerId);
                writer.Write(_lobbyState.PlayersReadyStatus[playerId]);
            }

            using (Message message = Message.Create(Tags.UpdateLobby, writer))
            {
                _client.SendMessage(message, SendMode.Reliable);
            }
        }
    }

    public void SendRequestLobbyUpdate()
    {
        using (DarkRiftWriter writer = DarkRiftWriter.Create())
        {
            using (Message message = Message.Create(Tags.RequestUpdateLobby, writer))
            {
                _client.SendMessage(message, SendMode.Reliable);
            }
        }
    }

    public void SendIsPlayerReadyMessage(bool isReady)
    {
        using (DarkRiftWriter writer = DarkRiftWriter.Create())
        {
            writer.Write(_client.ID);
            writer.Write(isReady);

            using (Message message = Message.Create(Tags.IsPlayerReady, writer))
            {
                _client.SendMessage(message, SendMode.Reliable);
            }
        }
    }
}

