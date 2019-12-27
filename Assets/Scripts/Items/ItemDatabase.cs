using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Items/ItemDatabase")]
public class ItemDatabase : ScriptableObject
{
    public List<ItemData> dataList;
}
