using DarkRift;
using DarkRift.Client.Unity;

public class LevelGraphMessageSender
{
    private UnityClient _client;
    private LevelGraphState _levelGraphState;
    private IGraphGenerable _graphGenerator;

    public LevelGraphMessageSender(
        LevelGraphState levelGraphState,
        IGraphGenerable graphGenerator,
        UnityClient client)
    {
        _client = client;
        _graphGenerator = graphGenerator;
        _levelGraphState = levelGraphState;
    }

    public void SendLevelGraph()
    {
        _graphGenerator.GenerateLevelGraph();
        //TODO MG: add some kind of validation to generated graph
        var rawGraphData = _levelGraphState.graph.GetSendableFormat();
        using (DarkRiftWriter writer = DarkRiftWriter.Create())
        {
            for (int i = 0; i < rawGraphData.Count; i++)
            {
                writer.Write(rawGraphData[i]);
            }

            using (Message levelGraphMessage = Message.Create(Tags.LevelGraph, writer))
            {
                _client.SendMessage(levelGraphMessage, SendMode.Reliable);
            }
        }
    }
}

