using UnityEngine;

public struct ItemSpawnParameters
{
    public short id;
    public ItemInstance item;
    public Vector2 position;

    public ItemSpawnParameters(short id, ItemInstance item, Vector2 position)
    {
        this.id = id;
        this.item = item;
        this.position = position;
    }
}

public struct ItemSpawnRequestParameters
{
    public ItemData data;
    public Vector2 position;

    public ItemSpawnRequestParameters(ItemData data, Vector2 position)
    {
        this.data = data;
        this.position = position;
    }
}