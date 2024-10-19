using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DisplayInventory : UIPanel
{
    public delegate void OnClick(DisplayItem item);
    public event OnClick onClick;

    [SerializeField] TextMeshProUGUI goldText;
    [SerializeField] TextMeshProUGUI soulText;
    [SerializeField] bool            interactionEnable = false;

    Inventory       _inventory;
    DisplayItem[]   items;
    Camera          mainCamera;

    protected override void Start()
    {
        base.Start();

        items = GetComponentsInChildren<DisplayItem>();

        Canvas canvas = GetComponentInParent<Canvas>();
        mainCamera = canvas.worldCamera;
    }

    public void SetInventory(Inventory inventory)
    {
        if (_inventory)
        {
            _inventory.onChange -= Refresh;
        }
        _inventory = inventory;
        if (_inventory)
        {
            _inventory.onChange += Refresh;
        }
    }

    public override void Open()
    {
        Refresh();

        base.Open();
    }

    public void Refresh()
    {
        for (int i = 0; i < items.Length; i++)
        {
            Item item = _inventory.GetItem(i);
            int  count = _inventory.GetItemCount(i);
            
            items[i].SetItem(i, item, count);
        }

        if (goldText) goldText.text = $"x{_inventory.gold}";
        if (soulText) soulText.text = $"x{_inventory.souls}";
    }

    protected override void Update()
    {
        base.Update();

        if ((!interactionEnable) || (!isOpen)) return;

        if (Input.GetMouseButtonDown(0))
        {
            Vector2 mousePos = Input.mousePosition;

            foreach (var item in items)
            {
                if (item.Overlaps(mousePos, mainCamera))
                {
                    onClick?.Invoke(item);
                }
            }
        }
    }
}
