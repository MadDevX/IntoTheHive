/// <summary>
/// This class uses level graphs made by graph generators to generate levels
/// </summary>
public class LevelSpawner
{
    private LevelGraphState _graphState;
    private LevelGraphTranslator _levelGraphTranslator;
    private Rooms _rooms;

    public LevelSpawner(
        LevelGraphTranslator levelGraphTranslator,
        LevelGraphState graphState,
        Rooms rooms)
    {
        _graphState = graphState;
        _levelGraphTranslator = levelGraphTranslator;
        _rooms = rooms;
    }

    public void GenerateLevel()
    {        
        var vertices = _graphState.graph.nodes;
        var spawnInfo = _levelGraphTranslator.TraverseLevelGraph();

        // TODO MG : iterate through vertices and spawn them 
        // spawn "bllockers" in rooms not connected to anything
    }
    
}