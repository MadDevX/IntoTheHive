using DarkRift.Client.Unity;

public class ClientInfo
{
    // TODO MG: UnityClient is probably unnecessary here
    public UnityClient Client;
    public ClientStatus Status = ClientStatus.None;

    public ClientInfo(
        UnityClient client)
    {
        Client = client;
    }
}