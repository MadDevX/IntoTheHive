using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

/// <summary>
/// This class generates basic level graphs
/// </summary>
public class LevelGraphGenerator : IGraphGenerable
{
    private LevelGraphState _levelGraph;
    //private Random _random;
    private Settings _settings;
    
    public LevelGraphGenerator(LevelGraphState levelGraph, Settings settings)
    {
        _levelGraph = levelGraph;
        _settings = settings;
    }

    private GraphDirection GetReverseDirection(GraphDirection direction)
    {
        return (GraphDirection)(3 - (int)direction);
    }

    private (int, int) GetRoomLocationAfterMove((int, int) currentLocation, GraphDirection direction)
    {
        switch(direction)
        {
            case GraphDirection.West:
                return (currentLocation.Item1 - 1, currentLocation.Item2);
            case GraphDirection.North:
                return (currentLocation.Item1, currentLocation.Item2 + 1);
            case GraphDirection.East:
                return (currentLocation.Item1 + 1, currentLocation.Item2);
            case GraphDirection.South:
                return(currentLocation.Item1, currentLocation.Item2 - 1);
            default:
                throw new ArgumentOutOfRangeException(nameof(direction), direction, null);
        }
    }
    private void Shuffle(GraphDirection[] array)
    {
        var length = array.Length;
        for (int t = 0; t < length; t++)
        {
            var tmp = array[t];
            int r = Random.Range(t, length);
            array[t] = array[r];
            array[r] = tmp;
        }
    }

    public void GenerateLevelGraph()
    {
        //All possible room connection options - every room can have 4 exits: top, bottom, left and right
        GraphDirection[] directions = { GraphDirection.East, GraphDirection.West, GraphDirection.North, GraphDirection.South};

        LevelGraph graph = _levelGraph.graph;
        graph.Reset();
        //Random number of rooms between min and max
        int numberOfRooms = Random.Range(_settings.MinNumberOfRooms, _settings.MaxNumberOfRooms);

        //Distance of a room from (0,0) - Manhattan distance
        Dictionary<int, int> distanceFromStart = new Dictionary<int, int>();
        //List of rooms without 4 neighbors - newly added room can be connected to any of those rooms
        List<int> possibleRooms = new List<int>();
        //Number of neighbors of each room to know when a given room has 4 neighbors, that means he can't have more of them 
        Dictionary<int, int> numberOfNeighbors = new Dictionary<int, int>();
        var realNumberOfNeighbors = new Dictionary<int, int>(); // <- this can be removed, only used for debug
        //room number to his location (x, y)
        Dictionary<int, (int, int)> roomToLocation = new Dictionary<int, (int, int)>();
        //location (x, y) to a room number that occupies it
        Dictionary<(int, int), int> locationToRoom = new Dictionary<(int, int), int>();

        //Add first room [Starting one], its location is (0, 0)
        Debug.Log($"Added room number 0");
        graph.AddVertex(Rooms.GetStartingRoom());

        roomToLocation[0] = (0, 0);
        locationToRoom[(0, 0)] = 0;

        distanceFromStart[0] = 0;
        numberOfNeighbors[0] = 0;
        realNumberOfNeighbors[0] = 0;
        possibleRooms.Add(0);
        
        //Add room one by one
        for (int newRoomId = 1; newRoomId < numberOfRooms; newRoomId++)
        {
            Debug.Log($"Added room number {newRoomId}");

            Shuffle(directions);
            graph.AddVertex(Rooms.GetRandomRoom());
            //Choose an existing room to which you can connect the newly created one
            var roomToConnect = possibleRooms[Random.Range(0, possibleRooms.Count - 1)];
            Debug.Log($"Found room of id {roomToConnect}");

            var location = roomToLocation[roomToConnect];
            var distanceToStart = int.MaxValue;
            //Go in the random direction in which you could add the room
            for (var j = 0; j < directions.Length; j++)
            {
                var potentialRoom = GetRoomLocationAfterMove(location, directions[j]);

                //If there is a room already there, just continue in another direction
                if (locationToRoom.ContainsKey(potentialRoom)) continue;

                //Set room in free spot
                roomToLocation[newRoomId] = potentialRoom;
                locationToRoom[potentialRoom] = newRoomId;
                numberOfNeighbors[newRoomId] = 0;
                realNumberOfNeighbors[newRoomId] = 0;

                graph.AddEdge(roomToConnect, newRoomId, directions[j]);
                distanceToStart = distanceFromStart[roomToConnect] + 1;

                for (var k = 0; k < directions.Length; k++)
                {
                    var potentialNeighbor = GetRoomLocationAfterMove(potentialRoom, directions[k]);
                    //if the newly added room has a neighbor in any of the direction
                    if (locationToRoom.ContainsKey(potentialNeighbor))
                    {
                        //increase number of neighbours of both rooms
                        var roomNumber = locationToRoom[potentialNeighbor];
                        numberOfNeighbors[roomNumber]++;
                        numberOfNeighbors[newRoomId]++;
                        // If we were the last possible neighbour of a new room
                        if (numberOfNeighbors[roomNumber] == 4)
                        {
                            possibleRooms.Remove(roomNumber);
                        }

                        //if we check the room we chose to be connected to, we know there will be a connection
                        if (roomNumber == roomToConnect)
                        {
                            realNumberOfNeighbors[roomNumber]++;
                            realNumberOfNeighbors[newRoomId]++;
                            continue;
                        }

                        //If we want to add the connection (random chance) 
                        if (Random.Range(1, 100) <= _settings.PercentageChanceOfConnectingExistingRoom)
                        {
                            Debug.Log($"Added a connection from room {newRoomId} to {roomNumber} with chance = {_settings.PercentageChanceOfConnectingExistingRoom}");
                            graph.AddEdge(newRoomId, roomNumber, directions[k]);
                            distanceToStart = Math.Min(distanceToStart, distanceFromStart[roomNumber] + 1);
                            realNumberOfNeighbors[roomNumber]++;
                            realNumberOfNeighbors[newRoomId]++;
                        }
                    }
                }
            }
            //In case we add a room in a space enclosed from all 4 sides
            //        X -- X
            //        |
            //        X    N -- X
            //        |         |
            //        X -- X -- X
            // X - existing rooms
            // N - new room we just added
            //In this case, our new room can't be a potential room, as it already has 4 neighbors
            if(numberOfNeighbors[newRoomId] != 4)
                possibleRooms.Add(newRoomId);
            distanceFromStart[newRoomId] = distanceToStart;
        }

        foreach (var room in numberOfNeighbors.Keys)
        {
            Debug.Log($"Room {room} has {realNumberOfNeighbors[room]} neighbors");
        }

        var roomsForExit = numberOfNeighbors.Where(x => x.Value == 1).Select(x => x.Key)
            .OrderByDescending(x => distanceFromStart[x]).ToList();

        //there is a room with exactly one neighbour -> will become the exit room
        if (roomsForExit.Any())
        {
            var exitRoom = roomsForExit.First();
            graph.nodes[exitRoom].SetRoomId(Rooms.GetExitRoom());
        }
        else
        {
            //Otherwise just add one more room add connect it to the furthest room from the start
            var roomId = graph.AddVertex(Rooms.GetExitRoom());
            //Impossible for all rooms to have 4 neighbors. The proof is left as an exercise to the reader.
            var roomNumber = numberOfNeighbors.Where(x => x.Value != 4).OrderByDescending(x => distanceFromStart[x.Key])
                .First().Key;
            var location = roomToLocation[roomNumber];
            for (int i = 0; i < directions.Length; i++)
            {
                var potentialSpace = GetRoomLocationAfterMove(location, directions[i]);

                //Check if there is a place for a new room after moving in given direction 
                if (!locationToRoom.ContainsKey(potentialSpace))
                {
                    graph.AddEdge(roomNumber, roomId, directions[i]);
                    break;
                }
            }
        }

    }
    [Serializable]
    public class Settings
    {
        public int MinNumberOfRooms;
        public int MaxNumberOfRooms;
        public int PercentageChanceOfConnectingExistingRoom;
    }
}