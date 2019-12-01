/// <summary>
/// This class represents a room in levelgraph
/// </summary>
public class LevelGraphVertex
{
    public int ID { get; set; }
    public ushort RoomId;
    public int[] neighbours;

    public LevelGraphVertex(ushort RoomId)
    {
        neighbours = new int[4];
        for (int i = 0; i < neighbours.Length; i++)
        {
            neighbours[i] = -1;
        }
    }

    /// <summary>
    /// Add neighbours in a given direction
    /// </summary>
    /// <param name="id">Node id of the neighbour</param>
    /// <param name="direction">Direction in which the neighbour will be located</param>
    public void AddNeighbour(int id, GraphDirection direction)
    {
        int dir = (int)direction;
        neighbours[dir] = id;
    }
}


