using System.Collections.Generic;
/// <summary>
/// Class representing levels as planar graphs with four children, each representing one direction
/// </summary>
public class LevelGraph
{
    public List<LevelGraphVertex> nodes;
    public int EndLevelRoomId { get; set; } = -1;
    public int TriggerId { get; set; } = -1;

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

    public void AddVertex(short roomid, short north, short west, short east, short south)
    {
        var vertex = new LevelGraphVertex((ushort)roomid);
        vertex.AddNeighbour(north, GraphDirection.North);
        vertex.AddNeighbour(west, GraphDirection.West);
        vertex.AddNeighbour(east, GraphDirection.East);
        vertex.AddNeighbour(south, GraphDirection.South);
        nodes.Add(vertex);
        vertex.ID = nodes.IndexOf(vertex);
    }

    public void Reset()
    {
        nodes.Clear();
        EndLevelRoomId = -1;
        TriggerId = -1;
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

    public List<short> GetSendableFormat()
    {
        List<short> data = new List<short>();

        nodes.ForEach(node =>
        {
            data.Add((short)node.RoomId);
            for(int i=0; i<node.neighbours.Length; i++)
            {
                data.Add((short)node.neighbours[i]);
            }
        });

        return data;
    }

}


