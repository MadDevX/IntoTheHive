using System.Collections.Generic;

public class DoorManager
{   
    private LevelSpawnParameters _levelSpawnParameters;
    private LevelGraphState _levelGraphState;
    private DoorFacade.Factory _doorFactory;
    /// <summary>
    /// The edges are only stored in one direction where From field is smaller than To.
    /// </summary>
    private Dictionary<int, List<EdgeStruct>> _edgesByRooms = new Dictionary<int, List<EdgeStruct>>(); 

    public DoorManager(
        LevelSpawnParameters levelSpawnParameters,
        LevelGraphState levelGraphState,
        DoorFacade.Factory doorFactory)
    {
        _levelSpawnParameters = levelSpawnParameters;
        _doorFactory = doorFactory;
        _levelGraphState = levelGraphState;
    }

    public void PrepareEdges()
    {
        //Initialize a list of edges for each present node
        for(int i=0; i<_levelGraphState.graph.nodes.Count; i++)
        {
            _edgesByRooms.Add(_levelGraphState.graph.nodes[i].ID, new List<EdgeStruct>());
        }

        _levelGraphState.graph.nodes.ForEach(node => CloseAllDoorsInRoom(node));
    }

    public void OpenAllDoorsInRoom(int id)
    {
        OpenAllDoorsInRoom(_levelGraphState.graph.nodes.Find(node => node.ID == id));
    }

    public void OpenAllDoorsInRoom(LevelGraphVertex node)
    {
        for (int i = 0; i < node.neighbours.Length; i++)
        {
            if (node.neighbours[i] >= 0)
            {
                if (Exists(node.ID, node.neighbours[i]))
                {
                    DeleteEdge(node.ID, node.neighbours[i]);
                }
            }
        }
    }

    public void CloseAllDoorsInRoom(int id)
    {
        CloseAllDoorsInRoom(_levelGraphState.graph.nodes.Find(node => node.ID == id));
    }

    public void CloseAllDoorsInRoom(LevelGraphVertex node)
    {
        // check which doors are closed and 
        for(int i=0; i<node.neighbours.Length; i++)
        {
            if(node.neighbours[i] >= 0)
            {
                if(Exists(node.ID, node.neighbours[i]) == false)
                {
                    AddEdge(node.ID, node.neighbours[i],
                        i == (int)GraphDirection.East || i == (int)GraphDirection.West);
                }
            }
        }
    }

    /// <summary>
    /// Gets an edge between two rooms
    /// </summary>
    /// <param name="from"></param>
    /// <param name="to"></param>
    /// <returns></returns>
    public EdgeStruct GetEdge(int from, int to)
    {
        if( from > to)
        {
            return GetEdge(to, from);
        }
        else
        {
            var edges = _edgesByRooms[from];
            return edges.Find(edge => edge.to == to);
        }
    }

    public bool Exists(int from, int to)
    {
        return GetEdge(from, to).facade != null;
    }

    public void AddEdge(int from, int to, bool isHorizontal)
    {
        if(from > to)
        {
            AddEdge(to, from, isHorizontal);
        }
        else
        {
            var spawnInfos = _levelSpawnParameters.spawnInfos;

            var fromInfo = spawnInfos.Find(info => info.ID == from);
            var toInfo = spawnInfos.Find(info => info.ID == to);

            var fromRoom = _levelGraphState.graph.nodes.Find(node => node.ID == from);
            var toRoom = _levelGraphState.graph.nodes.Find(node => node.ID == to);
            
            float x = (toInfo.X + fromInfo.X) / 2f;
            float y = (toInfo.Y + fromInfo.Y) / 2f;

            DoorSpawnParameters parameters = new DoorSpawnParameters(x, y, isHorizontal);

            EdgeStruct edge = new EdgeStruct
            {
                from = from,
                to = to,
                facade = _doorFactory.Create(parameters)
            };

            _edgesByRooms[from].Add(edge);

        }
    }

    public void DeleteEdge(int from, int to)
    {
        if(from > to)
        {
            DeleteEdge(to, from);
        }
        else
        {
            var edges = _edgesByRooms[from];
            var edgeToRemove = edges.Find(edge => edge.to == to);
            edgeToRemove.facade.Despawn();
            edges.Remove(edgeToRemove);
        }
    }

    public struct EdgeStruct
    {
        public int from;
        public int to;
        public DoorFacade facade;
    }

}