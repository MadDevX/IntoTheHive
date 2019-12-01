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

    public void AddVertex(ushort roomid, ushort north, ushort west, ushort east, ushort south)
    {
        var vertex = new LevelGraphVertex(roomid);
        vertex.AddNeighbour(north, GraphDirection.North);
        vertex.AddNeighbour(west, GraphDirection.West);
        vertex.AddNeighbour(east, GraphDirection.East);
        vertex.AddNeighbour(south, GraphDirection.South);
        nodes.Add(vertex);
    }

    public void Reset()
    {
        nodes.Clear();
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
        GraphDirection reversedDirection = (GraphDirection)(3 - (int)direction);
        to.AddNeighbour(fromId, reversedDirection);
    }

    public List<ushort> GetSendableFormat()
    {
        List<ushort> data = new List<ushort>();

        nodes.ForEach(node =>
        {
            data.Add(node.RoomId);
            for(int i=0; i<node.neighbours.Length; i++)
            {
                data.Add((ushort)node.neighbours[i]);
            }
        });

        return data;
    }

}


