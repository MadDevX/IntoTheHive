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
    public ItemData data;
    public Vector2 position;

    public PickupSpawnRequestParameters(ItemData data, Vector2 position)
    {
        this.data = data;
        this.position = position;
    }
}