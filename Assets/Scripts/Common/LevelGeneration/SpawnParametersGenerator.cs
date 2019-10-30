using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// transforms a list of levelgraphvertexes into roomspawninfo
/// </summary>
public class SpawnParametersGenerator
{
    private Settings _settings;
    private LevelGraphState _levelGraphState;

    public SpawnParametersGenerator(
        LevelGraphState levelGraphState,
        Settings settings)
    {
        _settings = settings;
        _levelGraphState = levelGraphState;
    }

    /// <summary>
    /// Translates levelGraph contained in LevelGraphState to a list of RoomSpawnParameters.
    /// </summary>
    /// <returns>List of RoomSpawnParameters with complete information to spawn the level</returns>
    public List<RoomSpawnParameters> TranslateLevelGraph()
    {
        var vertices = _levelGraphState.graph.nodes;
        
        List<RoomSpawnParameters> spawnInfos = new List<RoomSpawnParameters>();

        CalculateNeighboursPosition(vertices, spawnInfos);

        //clear(0, 0) rooms - when calculate positions is changed to bfs, all separate rooms will be deleted here
        var first = spawnInfos[0];
        spawnInfos.RemoveAll(info => info.X == 0 && info.Y == 0);
        spawnInfos.Add(first);

        return spawnInfos;
    }

    /// <summary>
    /// Iterates through all vertices and determines neighbours positions
    /// </summary>
    /// <param name="vertices">A list of LevelGraph vertices</param>
    /// <param name="roomSpawnInfo">A list of empty SpawnParameters which will be filled in this method</param>
    private void CalculateNeighboursPosition(List<LevelGraphVertex> vertices, List<RoomSpawnParameters> roomSpawnInfo)
    {
        bool[] visited = new bool[vertices.Count];
        Queue<LevelGraphVertex> queue = new Queue<LevelGraphVertex>();       
        
        vertices.ForEach(vertex => roomSpawnInfo.Add(new RoomSpawnParameters(0, 0, 0)));
        //BFS through the graph
        queue.Enqueue(vertices[0]);

        while(queue.Count > 0)
        {              
            var currentVertex = queue.Dequeue();
            if (currentVertex.ID == 0)
            {
                roomSpawnInfo[0].X = 0;
                roomSpawnInfo[0].Y = 0;
            }

            for (int dir = 0; dir < currentVertex.neighbours.Length; dir++)
            {
                int neighbourIndex = currentVertex.neighbours[dir];
                if (neighbourIndex >= 0)
                {
                    if(visited[neighbourIndex] == false )
                    {
                        Setposition(currentVertex.ID , vertices[neighbourIndex], (GraphDirection)dir, roomSpawnInfo);
                        queue.Enqueue(vertices[neighbourIndex]);
                        visited[neighbourIndex] = true;
                    }
                    
                }
                else
                {
                    //TODO MG : Figure out how to set doors properly
                    //SetDoorPosition(currentVertex.ID, (GraphDirection)dir, roomSpawnInfo);
                }
            }
        }
    }

    /// <summary>
    /// Sets the position fo the neighbouting vertex based on the direction in which it lies
    /// </summary>
    /// <param name="vertexIndex"> Index of current vertex</param>
    /// <param name="neighbourVertex"> Neighbouring LevelGraphVertex object</param>
    /// <param name="direction"> Direction in which the neighbouring vertex lies in reference to current vertex </param>
    /// <param name="roomSpawnInfo">List of RoomSpawnParameters</param>
    private void Setposition(int vertexIndex, LevelGraphVertex neighbourVertex, GraphDirection direction, List<RoomSpawnParameters> roomSpawnInfo)
    {
        roomSpawnInfo[neighbourVertex.ID].RoomId = neighbourVertex.RoomId;

        switch (direction)
        {
            case GraphDirection.North:
                roomSpawnInfo[neighbourVertex.ID].X = roomSpawnInfo[vertexIndex].X;
                roomSpawnInfo[neighbourVertex.ID].Y = roomSpawnInfo[vertexIndex].Y + _settings.roomSize;
                break;
            case GraphDirection.South:
                roomSpawnInfo[neighbourVertex.ID].X = roomSpawnInfo[vertexIndex].X;
                roomSpawnInfo[neighbourVertex.ID].Y = roomSpawnInfo[vertexIndex].Y - _settings.roomSize;
                break;
            case GraphDirection.East:
                roomSpawnInfo[neighbourVertex.ID].X = roomSpawnInfo[vertexIndex].X + _settings.roomSize;
                roomSpawnInfo[neighbourVertex.ID].Y = roomSpawnInfo[vertexIndex].Y;
                break;
            case GraphDirection.West:
                roomSpawnInfo[neighbourVertex.ID].X = roomSpawnInfo[vertexIndex].X - _settings.roomSize;
                roomSpawnInfo[neighbourVertex.ID].Y = roomSpawnInfo[vertexIndex].Y;
                break;
        }
    }

    /// <summary>
    /// Sets the position of the doors to block unconnected exits. This method adds new item to the SpawnParametersList.
    /// </summary>
    /// <param name="vertexIndex"> Index of current vertex</param>
    /// <param name="direction"> Direction in which the neighbouring vertex lies in reference to current vertex </param>
    /// <param name="roomSpawnInfo">List of RoomSpawnParameters</param>
    private void SetDoorPosition(int vertexIndex, GraphDirection direction, List<RoomSpawnParameters> roomSpawnInfo)
    {
        float X = 0, Y = 0;
        bool isHorizontal = true;
        switch (direction)
        {
            case GraphDirection.North:
                X = roomSpawnInfo[vertexIndex].X;
                Y = roomSpawnInfo[vertexIndex].Y + _settings.roomSize / 2;
                break;
            case GraphDirection.South:
                X = roomSpawnInfo[vertexIndex].X;
                Y = roomSpawnInfo[vertexIndex].Y - _settings.roomSize / 2;
                break;
            case GraphDirection.East:
                X = roomSpawnInfo[vertexIndex].X + _settings.roomSize / 2;
                Y = roomSpawnInfo[vertexIndex].Y;
                isHorizontal = false;
                break;
            case GraphDirection.West:
                X = roomSpawnInfo[vertexIndex].X - _settings.roomSize / 2;
                Y = roomSpawnInfo[vertexIndex].Y;
                isHorizontal = false;
                break;
        }

        //TODO MG: DELETE THIS HARDCODED DOOR "ROOMID" AND MAKE A BETTER SOLUTION FOR THIS
        roomSpawnInfo.Add(new RoomSpawnParameters(X, Y, 1, isHorizontal));
    }

    [System.Serializable]
    public class Settings
    {
        public float roomSize;
    }
}