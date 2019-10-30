/// <summary>
/// This class generates basic level graphs
/// </summary>
public class BasicLevelGraphGenerator : IGraphGenerable
{
    private LevelGraphState _levelGraph;
    // change this to BasicLevelGraphFactory?
    public BasicLevelGraphGenerator(LevelGraphState levelGraph)
    {
        _levelGraph = levelGraph;
    }

    public void GenerateLevelGraph()
    {
        LevelGraph graph = _levelGraph.graph;

        graph.AddVertex(0);
        graph.AddVertex(0);
        graph.AddVertex(0);
        graph.AddVertex(0);
        graph.AddVertex(0);
        graph.AddVertex(0);

        graph.AddEdge(0, 1, GraphDirection.North);
        graph.AddEdge(1, 2, GraphDirection.West);
        graph.AddEdge(2, 3, GraphDirection.North);
        graph.AddEdge(1, 4, GraphDirection.East);
        graph.AddEdge(4, 5, GraphDirection.North);
    }
}