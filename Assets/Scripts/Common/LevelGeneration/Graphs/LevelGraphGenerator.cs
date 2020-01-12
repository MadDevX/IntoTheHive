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

    /// <summary>
    /// Function for generating level graph.
    /// Uses minimal number of rooms, maximal number of rooms and chance to connect neighbor rooms from settings
    /// Returns graph of one level floor
    /// </summary>
    public void GenerateLevelGraph()
    {
        //Possible directions in which room can be connected to other ones
        GraphDirection[] directions = { GraphDirection.East, GraphDirection.West, GraphDirection.North, GraphDirection.South};

        LevelGraph graph = _levelGraph.graph;
        //number of rooms between given room and starting room
        Dictionary<int, int> distanceFromStart = new Dictionary<int, int>();
        //Rooms that can have more neighbors (number of neighbors is less than 4)
        List<int> possibleNeighborRooms = new List<int>();
        //Number of neighbors of a given room
        Dictionary<int, int> numberOfNeighbors = new Dictionary<int, int>();

        //Bidirectional dictionaries mapping rooms and locations of them on a xy plane
        Dictionary<int, (int, int)> roomToLocation = new Dictionary<int, (int, int)>();
        Dictionary<(int, int), int> locationToRoom = new Dictionary<(int, int), int>();

        //Number of rooms in the currently generated levelGraph
        int numberOfRooms = Random.Range(_settings.MinNumberOfRooms, _settings.MaxNumberOfRooms);

        //Add first room [Starting one] in (0, 0)
        graph.AddVertex(Rooms.GetStartingRoom());

        roomToLocation[0] = (0, 0);
        locationToRoom[(0, 0)] = 0;

        distanceFromStart[0] = 0;
        numberOfNeighbors[0] = 0;
        possibleNeighborRooms.Add(0);
        
        
        for (int newRoomId = 1; newRoomId < numberOfRooms; newRoomId++)
        {
            //Shuffle the possible directions in which we try to add the new room
            Shuffle(directions);
            graph.AddVertex(Rooms.GetRandomRoom());
            
            //Choose an existing room to which you can connect the newly created one
            var roomToConnect = possibleNeighborRooms[Random.Range(0, possibleNeighborRooms.Count - 1)];
            var location = roomToLocation[roomToConnect];
            var distanceToStart = Int32.MaxValue;
            //Go in the random direction in which you could add the room
            for (int j = 0; j < directions.Length; j++)
            {
                var potentialRoom = GetRoomLocationAfterMove(location, directions[j]);

                //If there is a room already there, just continue in another direction
                if (locationToRoom.ContainsKey(potentialRoom)) continue;

                //Set room in free spot
                roomToLocation[newRoomId] = potentialRoom;
                locationToRoom[potentialRoom] = newRoomId;
                numberOfNeighbors[newRoomId] = 0;

                graph.AddEdge(roomToConnect, newRoomId, directions[j]);
                distanceToStart = distanceFromStart[roomToConnect] + 1;

                for (var k = 0; k < directions.Length; k++)
                {
                    var potentialNeighbor = GetRoomLocationAfterMove(potentialRoom, directions[k]);
                    
                    if (locationToRoom.ContainsKey(potentialNeighbor))
                    {
                        var roomNumber = locationToRoom[potentialNeighbor];
                        numberOfNeighbors[roomNumber]++;
                        numberOfNeighbors[newRoomId]++;

                        // Our selected room has no free spot for a new rooms
                        if (numberOfNeighbors[roomNumber] == 4)
                        {
                            possibleNeighborRooms.Remove(roomNumber);
                        }

                        //if we check the room we chose to be connected to, we know there will be an edge
                        //No need to check if we want to connect it
                        if (roomNumber == roomToConnect)
                        {
                            continue;
                        }

                        //Check if we add a connection between those rooms 
                        if (Random.Range(1, 100) <= _settings.PercentageChanceOfConnectingExistingRoom)
                        {
                            //Debug.Log($"Added a connection from room {newRoomId} to {roomNumber} with chance = {_settings.PercentageChanceOfConnectingExistingRoom}");
                            graph.AddEdge(newRoomId, roomNumber, directions[k]);
                            distanceToStart = Math.Min(distanceToStart, distanceFromStart[roomNumber] + 1);
                        }
                    }
                }

                break;
            }
            //In case we add a room in a space enclosed from all 4 sides
            //        X -- X
            //        |
            //        X    N -- X
            //        |         |
            //        X -- X -- X
            // X - existing rooms
            // N - new room we just added
            //In this case, new room can't be a potential room, as it already has 4 neighbors
            if(numberOfNeighbors[newRoomId] != 4)
                possibleNeighborRooms.Add(newRoomId);
            distanceFromStart[newRoomId] = distanceToStart;
        }


        var roomsForExit = numberOfNeighbors.Where(x => x.Value == 1).Select(x => x.Key)
            .OrderByDescending(x => distanceFromStart[x]);

        //there is a room with exactly one neighbor -> will become the exit room
        if (roomsForExit.Any())
        {
            var exitRoom = roomsForExit.First();
            graph.nodes[exitRoom].SetRoomId(Rooms.GetExitRoom());
        }
        else
        {
            Shuffle(directions);
            //if there is no room with exactly one neighbor, create a new one and place it furthest from spawn
            var roomId = graph.AddVertex(Rooms.GetExitRoom());
            
            var roomNumber = numberOfNeighbors.Where(x => x.Value != 4).OrderByDescending(x => distanceFromStart[x.Key])
                .First().Key;
            var location = roomToLocation[roomNumber];
            //Check where we can place the new room
            for (int i = 0; i < directions.Length; i++)
            {
                var potentialSpace = GetRoomLocationAfterMove(location, directions[i]);

                if (!locationToRoom.ContainsKey(potentialSpace))
                {
                    graph.AddEdge(roomNumber, roomId, directions[i]);
                    return;
                }

                //Don't try to add more connections to the exit room, we want to have exactly one entrance
                //Last room, so no need to keep the dictionaries updated
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