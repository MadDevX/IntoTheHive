using DarkRift;
using DarkRift.Client.Unity;

/// <summary>
/// Scene-local lobby only class used to send messages on that scene
/// </summary>
public class LobbyMessageSender
{
    private LobbyState _lobbyState;
    private UnityClient _client;
    private SceneMessageSender _sceneMessageSender;

    public LobbyMessageSender(
        UnityClient client,
        LobbyState lobbyState,
        SceneMessageSender sceneMessageSender)
    {
        _client = client;
        _lobbyState = lobbyState;
        _sceneMessageSender = sceneMessageSender;
    }

    /// <summary>
    /// Sends a LoadLobby message to a given player. 
    /// </summary>
    /// <param name="id"> Id of client who should change the scene </param>
    /// <param name="sceneIndex">Index of the scene which </param>
    public void SendLoadLobbyMessage(ushort id, ushort sceneIndex)
    {
        _sceneMessageSender.SendSceneChangedToPlayer(id,sceneIndex);
        // The response from client is RequestLobbyUpdate message
    }

    /// <summary>
    /// Sends a message with a list of clients and their ready status to provide visual list to the lobby.
    /// </summary>
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

    /// <summary>
    /// Sends a message with a request to receive a list of current players in the lobby along with their ready status.
    /// </summary>
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

    /// <summary>
    /// Sends a message with a an information wheter the client is ready or not.
    /// </summary>
    /// <param name="isReady">Is the sender ready</param>
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

