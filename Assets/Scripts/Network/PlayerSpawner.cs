using DarkRift;
using DarkRift.Client;
using DarkRift.Client.Unity;
using UnityEngine;

public class PlayerSpawner : MonoBehaviour
{
    [SerializeField]
    [Tooltip("The DarkRift client to communicate on.")]
    UnityClient client;

    //Should be Character.Factory
    //private Projectile.Factory _projectileFactory;
    

    void Awake()
    {
        client.MessageReceived += HandleMessage;
    }

    private void HandleMessage(object sender, MessageReceivedEventArgs e)
    {
        using (Message message = e.GetMessage())
        {
            if (message.Tag == Tags.SpawnCharacter) HandleSpawn(sender,e);
        }
    }

    private void HandleSpawn(object sender, MessageReceivedEventArgs e)
    {
        using (Message message = e.GetMessage())
        {
            using (DarkRiftReader reader = message.GetReader())
            {
                //logic for SpawnMessage handling
            }
        }
    }
}
