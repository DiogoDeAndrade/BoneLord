using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UISummoningCircle : UIPanel
{
    [SerializeField] private RectTransform  slotContainer;
    [SerializeField] private IngredientSlot slotPrefab;
    [SerializeField] private Image          circleImage;
    [SerializeField] private Image          orbImage;

    int slotCount = 3;
    Inventory               inventory;
    DisplayInventory        displayInventory;
    Item                    carryItem;
    List<IngredientSlot>    ingredientSlots;
    Camera                  mainCamera;
    SummoningCircle         summoningCircle;

    bool circleActive
    {
        get
        {
            if (ingredientSlots == null) return false;
            if (inventory.souls < 1) return false;
            foreach (var ingredient in ingredientSlots)
            {
                if (ingredient.GetItem(false) == null) return false;
            }
            List<Item> items = new List<Item>();
            foreach (var ingredient in ingredientSlots)
            {
                items.Add(ingredient.GetItem(false));
            }

            return summoningCircle.IsValid(items);
        }
    }

    protected void Awake()
    {
        displayInventory = GetComponentInChildren<DisplayInventory>();
        summoningCircle = FindFirstObjectByType<SummoningCircle>();

        Canvas canvas = GetComponentInParent<Canvas>();
        mainCamera = canvas.worldCamera;
    }

    public void SetSlotCount(int count)
    {
        slotCount = count;
    }

    public void SetInventory(Inventory inventory)
    {
        this.inventory = inventory;
        displayInventory.SetInventory(inventory);        
    }

    public override void Open()
    {
        base.Open();

        RefreshSlots();

        displayInventory.Open();
        displayInventory.onClick += ClickOnInventory;
    }

    public override void Close()
    {
        base.Close();
        
        displayInventory.Close();
        displayInventory.onClick -= ClickOnInventory;

        if (carryItem)
        {
            inventory.Add(carryItem);
            carryItem = null;
            PlayerControl.SetCursor(null);
        }
    }

    void RefreshSlots()
    {
        ingredientSlots = new List<IngredientSlot>(GetComponentsInChildren<IngredientSlot>());

        if (ingredientSlots.Count > slotCount)
        {
            // Need to delete some slots
            while (ingredientSlots.Count > slotCount)
            {
                var item = ingredientSlots[ingredientSlots.Count - 1].GetItem(true);
                inventory.Add(item);
                Destroy(ingredientSlots[ingredientSlots.Count - 1].gameObject);
                ingredientSlots.RemoveAt(ingredientSlots.Count - 1);
            }
        }
        else
        {
            while (ingredientSlots.Count < slotCount)
            {
                var newSlot = Instantiate(slotPrefab, slotContainer);
                ingredientSlots.Add(newSlot);
            }
        }

        // Update positions
        float radius = slotContainer.rect.width * 0.5f * 0.85f;
        for (int i = 0; i < ingredientSlots.Count; i++)
        {
            float angle = Mathf.Deg2Rad * (90.0f + i * (360 / ingredientSlots.Count));
            RectTransform rt = ingredientSlots[i].transform as RectTransform;
            rt.anchoredPosition = new Vector2(radius * Mathf.Cos(angle), radius * Mathf.Sin(angle));
        }
    }

    private void ClickOnInventory(DisplayItem item)
    {
        if (carryItem == null)
        {
            if (item.item != null)
            {
                carryItem = item.item;
                inventory.RemoveAt(item.slot);

                PlayerControl.SetCursor(carryItem.sprite, 2.0f);
            }
        }
        else if (carryItem)
        {
            if (item.item != null) 
            {
                if ((item.count == 1) && (item.item != carryItem))
                {
                    var swapItem = item.item;
                    inventory.Set(item.slot, carryItem, 1);
                    carryItem = swapItem;

                    PlayerControl.SetCursor(carryItem.sprite, 2.0f);
                }
                else if (item.item == carryItem)
                {
                    inventory.Set(item.slot, carryItem, item.count + 1);
                    carryItem = null;

                    PlayerControl.SetCursor(null);
                }
            }
            else
            {
                inventory.Set(item.slot, carryItem, 1);
                carryItem = null;

                PlayerControl.SetCursor(null);
            }
        }
    }

    private void ClickOnIngredient(IngredientSlot ingredient)
    {
        if (carryItem)
        {
            var prevItem = ingredient.GetItem(true);
            ingredient.SetItem(carryItem);
            carryItem = prevItem;
        }
        else
        {
            carryItem = ingredient.GetItem(true);
        }

        if (carryItem) PlayerControl.SetCursor(carryItem.sprite, 2.0f);
        else PlayerControl.SetCursor(null);
    }

    protected override void Update()
    {
        base.Update();

        if (Input.GetMouseButtonDown(0))
        {
            Vector2 mousePos = Input.mousePosition;

            if (ingredientSlots != null)
            {
                foreach (var ingredient in ingredientSlots)
                {
                    if (ingredient.Overlaps(mousePos, mainCamera))
                    {
                        ClickOnIngredient(ingredient);
                    }
                }
            }

            if ((orbImage.color.a == 1.0f) && (circleActive))
            {
                RectTransform orbTransform = orbImage.transform as RectTransform;
                if (orbTransform.ScreenPointOverlaps(mousePos, mainCamera))
                {
                    // Do the operation
                    List<Item> items = new List<Item>();
                    foreach (var ingredient in ingredientSlots)
                    {
                        items.Add(ingredient.GetItem(true));
                    }
                    Close();

                    inventory.ChangeSouls(-1);
                    summoningCircle.Summon(items);
                }
            }
        }

        if (circleActive)
        {
            circleImage.color = circleImage.color.MoveTowards(circleImage.color.ChangeAlpha(1.0f), Time.deltaTime * 4.0f);
            orbImage.color = orbImage.color.MoveTowards(orbImage.color.ChangeAlpha(1.0f), Time.deltaTime * 4.0f);
        }
        else
        {
            circleImage.color = circleImage.color.MoveTowards(circleImage.color.ChangeAlpha(0.25f), Time.deltaTime * 4.0f);
            orbImage.color = orbImage.color.MoveTowards(orbImage.color.ChangeAlpha(0.0f), Time.deltaTime * 4.0f);
        }
    }


}
