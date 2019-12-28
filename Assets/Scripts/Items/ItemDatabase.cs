using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Items/ItemDatabase")]
public class ItemDatabase : ScriptableObject
{
    public List<ItemData> dataList;

    public ItemData GetData(ItemType type, short id)
    {
        foreach (var data in dataList)
        {
            if (data.itemId == id && data.type == type)
            {
                return data;
            }
        }
        throw new ArgumentException($"Tried to get data that does not exist. Type: {type} | ID: {id}");
    }
}
