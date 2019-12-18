using DarkRift;
using DarkRift.Client.Unity;

public class LevelGraphMessageSender
{   
    private LevelGraphState _levelGraphState;
    private IGraphGenerable _graphGenerator;

    public LevelGraphMessageSender(
        LevelGraphState levelGraphState,
        IGraphGenerable graphGenerator)
    {
        _graphGenerator = graphGenerator;
        _levelGraphState = levelGraphState;
    }

    public Message GenerateLevelGraphMessage()
    {
        _graphGenerator.GenerateLevelGraph();
        //TODO MG: add some kind of validation to generated graph        
        var rawGraphData = _levelGraphState.graph.GetSendableFormat();
        using (DarkRiftWriter writer = DarkRiftWriter.Create())
        {
            writer.Write((ushort)_levelGraphState.graph.TriggerId);
            writer.Write((ushort)_levelGraphState.graph.EndLevelRoomId);

            for (int i = 0; i < rawGraphData.Count; i++)
            {
                writer.Write(rawGraphData[i]);
            }

            return Message.Create(Tags.LevelGraph, writer);            
        }
    }
}

