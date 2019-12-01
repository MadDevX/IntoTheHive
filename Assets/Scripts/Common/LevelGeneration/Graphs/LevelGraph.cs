using System.Collections.Generic;
/// <summary>
/// Class representing levels as planar graphs with four children, each representing one direction
/// </summary>
public class LevelGraph
{
    public List<LevelGraphVertex> nodes;

    public LevelGraph()
    {
        nodes = new List<LevelGraphVertex>();
    }

    public int AddVertex(ushort roomId)
    {
        var vertex = new LevelGraphVertex(roomId);
        nodes.Add(vertex);
        vertex.ID = nodes.IndexOf(vertex);
        return vertex.ID;
    }

    public void RemoveVertex(int Id)
    {
        nodes.RemoveAt(Id);
    }

    public void AddEdge(int fromId, int toId, GraphDirection direction)
    {
        var from = nodes[fromId];
        var to = nodes[toId];
        from.AddNeighbour(toId, direction);
        GraphDirection reversedDirection = (GraphDirection) (3 - (int)direction);
        to.AddNeighbour(fromId, reversedDirection);
    }
}


