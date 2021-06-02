using UnityEngine;

public struct PickupSpawnParameters
{
    public short id;
    public ItemData item;
    public Vector2 position;

    public PickupSpawnParameters(short id, ItemData item, Vector2 position)
    {
        this.id = id;
        this.item = item;
        this.position = position;
    }
}

public struct PickupSpawnRequestParameters
{
    public short itemId;
    public Vector2 position;

    public PickupSpawnRequestParameters(short itemId, Vector2 position)
    {
        this.itemId = itemId;
        this.position = position;
    }
}