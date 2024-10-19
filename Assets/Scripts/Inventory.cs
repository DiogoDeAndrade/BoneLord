using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    [SerializeField] private int maxSlots = 25;

    [SerializeField] private Item[] debugItems;
    [SerializeField] private int    debugInitialGold;
    [SerializeField] private int    debugInitialSouls;

    public delegate void OnChange();

    public event OnChange onChange;

    class Slot
    {
        public Item item;
        public int  count;
    }

    private Slot[] items;
    private int    _gold;
    private int    _souls;

    public int gold => _gold;
    public int souls => _souls;

    public void Awake()
    {
        items = new Slot[maxSlots];

#if UNITY_EDITOR
        foreach (var item in debugItems)
        {
            Add(item);
        }
        _gold = debugInitialGold;
        _souls = debugInitialSouls;
#endif
    }

    public void ChangeGold(int delta)
    {
        _gold += delta;
    }

    public void ChangeSouls(int delta)
    {
        _souls += delta;
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
