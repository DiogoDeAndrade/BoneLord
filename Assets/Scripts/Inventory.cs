using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    [SerializeField] private int maxSlots = 25;

    public delegate void OnChange();

    public event OnChange onChange;

    class Slot
    {
        public Item item;
        public int  count;
    }

    private Slot[] items;

    public void Awake()
    {
        items = new Slot[maxSlots];
    }

    public void Add(Item item)
    {
        foreach (var slot in items)
        {
            if (slot != null)
            {
                if (slot.item == item)
                {
                    slot.count++;
                    if (onChange != null) onChange();
                    return;
                }
            }
        }

        var newSlot = GetEmptySlot();
        if (newSlot == null)
        {
            Debug.LogWarning("Inventory is full!");
            return;
        }
        newSlot.item = item;
        newSlot.count = 1;

        if (onChange != null) onChange();
    }

    public bool isFull
    {
        get
        {
            foreach (var item in items)
            {
                if ((item == null) || (item.item == null)) return false;
            }

            return true;
        }
    }

    Slot GetEmptySlot()
    {
        for (int i = 0; i < items.Length; i++)
        {
            if (items[i] == null)
            {
                items[i] = new Slot();
                return items[i];
            }
            else if (items[i].item == null)
            {
                return items[i];
            }
        }

        return null;
    }
      
    public Item GetItem(int index)
    {
        if (items[index] == null) return null;

        return items[index].item;
    }

    public int GetItemCount(Item item)
    {
        if (item == null) return 0;

        foreach (var slot in items)
        {
            if (slot.item == item) return slot.count;
        }

        return 0;
    }
}
