using System;
using System.Collections.Generic;
/// <summary>
/// This class uses level graphs made by graph generators to generate levels
/// </summary>
public class LevelSpawner
{
    private IGraphGenerable _graphGenerator;
    private LevelGraphTranslator _levelGraphTranslator;
    private Rooms _rooms;

    public LevelSpawner(
        LevelGraphTranslator levelGraphTranslator,
        IGraphGenerable graphGenerator,
        Rooms rooms)
    {
        _graphGenerator = graphGenerator;
        _levelGraphTranslator = levelGraphTranslator;
        _rooms = rooms;
    }

    public void GenerateLevel()
    {
        //Delete this and instead call this in NetworkedLEvel spawner and then send an appropriate message
        LevelGraph levelGraph = _graphGenerator.GenerateLevelGraph();
        var vertices = levelGraph.nodes;

        //Move to another class - return list of (x,y,roomId)
        //Maybe use Data Component to do so in a network friendly way

        //This thing will happen only on host - the (x,y, roomId data will be sent)?? or not because of floats
        _levelGraphTranslator.TraverseLevelGraph();

        // TODO MG : iterate through vertices and spawn them 
        // spawn "bllockers" in rooms not connected to anything

    }
    
}