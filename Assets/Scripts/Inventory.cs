using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public delegate void OnChange();

    public event OnChange onChange;

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

        if (onChange != null) onChange();
    }

    public Item GetItem(int index)
    {
        if (index < items.Count)
        {
            return items.ElementAt(index).Key;
        }

        return null;
    }

    public int GetItemCount(Item item)
    {
        if (item == null) return 0;

        if (items.ContainsKey(item))
        {
            return items[item];
        }

        return 0;
    }
}
