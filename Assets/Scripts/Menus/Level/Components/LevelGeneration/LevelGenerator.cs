/// <summary>
/// This class uses level graphs made by graph generators to generate levels
/// </summary>
public class LevelGenerator
{
    private IGraphGenerable _graphGenerator;
    private Rooms _rooms;

    public LevelGenerator(
        IGraphGenerable graphGenerator,
        Rooms rooms)
    {
        _graphGenerator = graphGenerator;
        _rooms = rooms;
    }

    public void GenerateLevel()
    {
        LevelGraph levelGraph = _graphGenerator.GenerateLevelGraph();
        var vertices = levelGraph.nodes;

        // TODO MG : add monopool for rooms
        // TODO MG : iterate through vertices and spawn them 
        // spawn "bllockers" in rooms not connected to anything

    }

}