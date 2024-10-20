using UnityEngine;

public class Inventory : MonoBehaviour
{
    [SerializeField] private int maxSlots = 25;

    [SerializeField] private ItemDef[] debugItems;
    [SerializeField] private int    debugInitialGold;
    [SerializeField] private int    debugInitialSouls;

    public delegate void OnChange();

    public event OnChange onChange;

    class Slot
    {
        public ItemDef item;
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
        LoadDebugItems();
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

    public void Add(ItemDef item)
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

        onChange?.Invoke();
    }

    public void Remove(ItemDef item)
    {
        foreach (var slot in items)
        {
            if (slot != null)
            {
                if (slot.item == item)
                {
                    slot.count--;
                    if (slot.count == 0) slot.item = null;

                    onChange?.Invoke();
                    return;
                }
            }
        }
    }

    public void RemoveAt(int index)
    {
        if (items[index] == null) return;
        if (items[index].item == null) return;

        items[index].count--;

        if (items[index].count == 0)
        {
            items[index].item = null;
        }

        onChange?.Invoke();
        return;
    }

    public void Set(int slot, ItemDef item, int count)
    {
        if (items[slot] == null) items[slot] = new Slot();

        items[slot].item = item;
        items[slot].count = count;

        onChange?.Invoke();
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
      
    public ItemDef GetItem(int index)
    {
        if (items[index] == null) return null;

        return items[index].item;
    }
    public int GetItemCount(int index)
    {
        if (items[index] == null) return 0;

        return items[index].count;
    }

    public int GetItemCount(ItemDef item)
    {
        if (item == null) return 0;

        foreach (var slot in items)
        {
            if (slot == null) continue;
            if (slot.item == item) return slot.count;
        }

        return 0;
    }

    void LoadDebugItems()
    {
        if (debugItems == null) return;
        foreach (var item in debugItems)
        {
            Add(item);
        }
        _gold = debugInitialGold;
        _souls = debugInitialSouls;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F5))
        {
            LoadDebugItems();
        }
    }

}
