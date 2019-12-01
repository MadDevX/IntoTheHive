using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// transforms a list of levelgraphvertexes into roomspawninfo
/// </summary>
public class LevelGraphTranslator
{
    private Settings _settings;
    private LevelGraphState _levelGraphState;

    public LevelGraphTranslator(
        LevelGraphState levelGraphState,
        Settings settings)
    {
        _settings = settings;
    }

    public List<RoomSpawnInfo> TraverseLevelGraph()
    {
        var vertices = _levelGraphState.graph.nodes;
        bool[] positionSet = new bool[vertices.Count];
        List<RoomSpawnInfo> spawnInfos = new List<RoomSpawnInfo>();
        vertices.ForEach(vertex => spawnInfos.Add(new RoomSpawnInfo(0, 0, 0)));

        CalculateNeighboursPosition(vertices, positionSet, spawnInfos);

        //Clear (0,0) Rooms - when calculate positions is changed to BFS, all separate rooms will be deleted here
        var first = spawnInfos[0];
        spawnInfos.RemoveAll(info => info.X == 0 && info.Y == 0);
        spawnInfos.Add(first);

        return spawnInfos;
    }

    private void CalculateNeighboursPosition(List<LevelGraphVertex> vertices, bool[] positionSet, List<RoomSpawnInfo> roomSpawnInfo)
    {
        Queue<LevelGraphVertex> queue = new Queue<LevelGraphVertex>();       
        queue.Enqueue(vertices[0]);

        while(queue.Count > 0)
        {
            var currentVertex = queue.Dequeue();
            if (currentVertex.ID == 0)
            {
                roomSpawnInfo[0].X = 0;
                roomSpawnInfo[0].Y = 0;
            }

            positionSet[currentVertex.ID] = true;

            for (int dir = 0; dir < currentVertex.neighbours.Length; dir++)
            {
                int neighbourIndex = currentVertex.neighbours[dir];
                if (neighbourIndex >= 0)
                {
                    if(positionSet[neighbourIndex] == false )
                    {
                        Setposition(currentVertex.ID , vertices[neighbourIndex], (GraphDirection)dir, roomSpawnInfo);
                    }
                    positionSet[neighbourIndex] = true;
                    queue.Enqueue(vertices[neighbourIndex]);
                }
                else
                {
                    // TODO MG: somehow spawn closed doors in a given direction
                }
            }
        }
    }

    private void Setposition(int vertexIndex, LevelGraphVertex neighbourVertex, GraphDirection direction, List<RoomSpawnInfo> roomSpawnInfo)
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

    [SerializeField]
    public class Settings
    {
        public int roomSize;
    }
}