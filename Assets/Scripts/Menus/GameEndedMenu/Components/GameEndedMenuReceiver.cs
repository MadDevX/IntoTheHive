using DarkRift;
using System;
using TMPro;
using UnityEngine;
using Zenject;

/// <summary>
/// Updates Win/Lose text based on appropriate messages.
/// </summary>
public class GameEndedMenuReceiver: IInitializable, IDisposable
{
    private NetworkRelay _networkRelay;
    private TextMeshProUGUI _winText;
    private TextMeshProUGUI _loseText;

    public GameEndedMenuReceiver(
        NetworkRelay relay,
        [Inject(Id = Identifiers.GameEndedWinText)]
        TextMeshProUGUI winText,
        [Inject(Id = Identifiers.GameEndedLoseText)]
        TextMeshProUGUI loseText)
    {
        _networkRelay = relay;
        _winText = winText;
        _loseText = loseText;
    }

    public void Initialize()
    {
        _networkRelay.Subscribe(Tags.UpdateGameState, UpdateGameEndMessage);
    }

    public void Dispose()
    {
        _networkRelay.Unsubscribe(Tags.UpdateGameState, UpdateGameEndMessage);
    }    

    /// <summary>
    /// Updates the texts in the scene based on the message that is received.
    /// </summary>
    /// <param name="updateGameStateMessage"></param>
    private void UpdateGameEndMessage(Message updateGameStateMessage)
    {
        using(DarkRiftReader reader = updateGameStateMessage.GetReader())
        {
            bool isWin = reader.ReadBoolean();
            _winText.enabled = isWin;
            _loseText.enabled = !isWin;
        }
    }
}
