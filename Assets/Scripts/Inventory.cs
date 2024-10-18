using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    private Dictionary<Item, int>   items = new Dictionary<Item, int>();

    public void Add(Item item)
    {
        if (items.ContainsKey(item))
        {
            items[item]++;
        }
        else
        {
            items.Add(item, 1);
        }
    }
}
