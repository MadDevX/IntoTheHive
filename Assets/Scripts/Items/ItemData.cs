using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "Items/Item")]
public class ItemData : ScriptableObject
{
    public ItemTypes type;
    public short itemId;
    public Sprite icon;

    public ItemInstance CreateItem(ItemFactory factory)
    {
        return new ItemInstance(factory.Create(type, itemId), this);
    }
}