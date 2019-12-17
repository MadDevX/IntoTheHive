using DarkRift;
using UnityEngine;
/// <summary>
/// Sends messages which will be handled by ClientDoorManager
/// </summary>
public class HostDoorManager
{    
    public Message PrepareOpenDoorsMessage(ushort roomId)
    {
        Debug.Log("PreparedOpenDoorsMessage");
        using (DarkRiftWriter writer = DarkRiftWriter.Create())
        {
            writer.Write(roomId);
            return Message.Create(Tags.OpenDoorsMessage, writer);
        }
    }

    public Message PrepareCloseDoorsMessage(ushort roomId)
    {
        using (DarkRiftWriter writer = DarkRiftWriter.Create())
        {
            writer.Write(roomId);
            return Message.Create(Tags.CloseDoorsMessage, writer);
        }
    }
}