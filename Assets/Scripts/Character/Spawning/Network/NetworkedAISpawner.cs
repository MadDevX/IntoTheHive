using DarkRift;
using DarkRift.Client.Unity;
using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class NetworkedAISpawner
{      
    public Message GenerateSpawnMessage(List<AISpawnParameters> data)
    {
        using (DarkRiftWriter writer = DarkRiftWriter.Create())
        {
            foreach (AISpawnParameters spawnData in data)
            {
                writer.Write(spawnData.SpawnParameters.Id);
                writer.Write(spawnData.SpawnParameters.X);
                writer.Write(spawnData.SpawnParameters.Y);
            }
            
            return Message.Create(Tags.SpawnAI, writer);            
        }
    }   

    public Message GenerateDespawnMessage(ushort playerID)
    {
        using (DarkRiftWriter writer = DarkRiftWriter.Create())
        {
            writer.Write(playerID);

            return Message.Create(Tags.DespawnAI, writer);
        }
    }
}
