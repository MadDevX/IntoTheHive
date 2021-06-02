using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "Items/Item")]
public class ItemData : ScriptableObject
{
    public ItemType type;
    public short itemId;
    public Sprite icon;
}