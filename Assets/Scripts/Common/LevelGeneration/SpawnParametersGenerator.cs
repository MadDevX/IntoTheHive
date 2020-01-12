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
    private LevelSpawnParameters _levelSpawnParameters;

    public SpawnParametersGenerator(
        LevelGraphState levelGraphState,
        LevelSpawnParameters levelSpawnParameters,
        Settings settings)
    {
        _settings = settings;
        _levelGraphState = levelGraphState;
        _levelSpawnParameters = levelSpawnParameters;
    }

    /// <summary>
    /// Translates levelGraph contained in LevelGraphState to a list of RoomSpawnParameters.
    /// </summary>
    /// <returns>List of RoomSpawnParameters with complete information to spawn the level</returns>
    public void TranslateLevelGraph()
    {
        var vertices = _levelGraphState.graph.nodes;
        CalculateNeighboursPosition(vertices, _levelSpawnParameters.roomSpawnInfos, _levelSpawnParameters.doorSpawnInfos);
        ////clear(0, 0) rooms - when calculate positions is changed to bfs, all separate rooms will be deleted here
        //var first = levelSpawnParameters.spawnInfos[0];
        //levelSpawnParameters.spawnInfos.RemoveAll(info => info.X == 0 && info.Y == 0);
        //levelSpawnParameters.spawnInfos.Add(first);
    }

    /// <summary>
    /// Iterates through all vertices and determines neighbours positions
    /// </summary>
    /// <param name="vertices">A list of LevelGraph vertices</param>
    /// <param name="roomSpawnInfo">A list of empty SpawnParameters which will be filled in this method</param>
    private void CalculateNeighboursPosition(List<LevelGraphVertex> vertices, List<RoomSpawnParameters> roomSpawnInfo, List<DoorSpawnParameters> doorSpawnInfo)
    {
        bool[] visited = new bool[vertices.Count];
        Queue<LevelGraphVertex> queue = new Queue<LevelGraphVertex>();       
        
        vertices.ForEach(vertex => roomSpawnInfo.Add(new RoomSpawnParameters(0, 0, 0, vertex.RoomId)));
        //BFS through the graph
        queue.Enqueue(vertices[0]);

        while(queue.Count > 0)
        {              
            var currentVertex = queue.Dequeue();
            int id = currentVertex.ID;
            roomSpawnInfo[id].ID = id;
            if (id == 0)
            {
                roomSpawnInfo[id].X = 0;
                roomSpawnInfo[id].Y = 0;
            }

            for (int dir = 0; dir < currentVertex.neighbours.Length; dir++)
            {
                int neighbourIndex = currentVertex.neighbours[dir];
                if (neighbourIndex >= 0)
                {
                    if(visited[neighbourIndex] == false )
                    {
                        Setposition(id , vertices[neighbourIndex], (GraphDirection)dir, roomSpawnInfo);
                        queue.Enqueue(vertices[neighbourIndex]);
                    }
                    
                }
                else
                {
                    SetDoorPosition(id, (GraphDirection)dir, roomSpawnInfo, doorSpawnInfo);
                }
            }
            visited[id] = true;
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
    /// <param name="doorSpawnInfo">List of RoomSpawnParameters</param>
    private void SetDoorPosition(int vertexIndex, GraphDirection direction, List<RoomSpawnParameters> roomSpawnInfo, List<DoorSpawnParameters> doorSpawnInfo)
    {
        float X = 0, Y = 0;
        bool isHorizontal = true;
        switch (direction)
        {
            case GraphDirection.North:
                X = roomSpawnInfo[vertexIndex].X;
                Y = roomSpawnInfo[vertexIndex].Y + _settings.roomSize / 2f - 0.5f;
                isHorizontal = false;
                break;
            case GraphDirection.South:
                X = roomSpawnInfo[vertexIndex].X;
                Y = roomSpawnInfo[vertexIndex].Y - _settings.roomSize / 2f + 0.5f;
                isHorizontal = false;
                break;
            case GraphDirection.East:
                X = roomSpawnInfo[vertexIndex].X + _settings.roomSize / 2f - 0.5f;
                Y = roomSpawnInfo[vertexIndex].Y;
                isHorizontal = true;
                break;
            case GraphDirection.West:
                X = roomSpawnInfo[vertexIndex].X - _settings.roomSize / 2f + 0.5f;
                Y = roomSpawnInfo[vertexIndex].Y;
                isHorizontal = true;
                break;
        }

        doorSpawnInfo.Add(new DoorSpawnParameters(X, Y, isHorizontal));
    }

    [System.Serializable]
    public class Settings
    {
        public float roomSize;
    }
}