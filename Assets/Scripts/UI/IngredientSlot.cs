using UnityEngine;
using UnityEngine.UI;

public class IngredientSlot : MonoBehaviour
{
    [SerializeField] private Image slot;

    private Item            item;
    private RectTransform   rectTransform;

    private void Start()
    {
        rectTransform = transform as RectTransform;
        SetItem(null);
    }

    public void SetItem(Item item)
    {
        this.item = item;
        if ((item != null) && (item.sprite != null))
        {
            slot.sprite = item.sprite;
            slot.enabled = true;
        }
        else
        {
            slot.enabled = false;
        }
    }

    public Item GetItem(bool remove)
    {
        Item prevItem = this.item;
        if (remove) SetItem(null);
        return prevItem;
    }

    public bool Overlaps(Vector2 mousePos, Camera camera)
    {
        return rectTransform.ScreenPointOverlaps(mousePos, camera);
    }
}
