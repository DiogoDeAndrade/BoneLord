using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class DisplayItem : MonoBehaviour
{
    [SerializeField] private Image              image;
    [SerializeField] private TextMeshProUGUI    text;

    private int             _slot;
    private ItemDef            _item;
    private int             _count;
    private RectTransform   rectTransform;

    public int  slot => _slot;
    public ItemDef item => _item;
    public int  count => _count;

    private void Start()
    {
        rectTransform = transform as RectTransform;
    }

    public void SetItem(int slot, ItemDef item, int count)
    {
        _slot = slot;
        _item = item;
        _count = count;
        if (item)
        {
            image.sprite = item.sprite;
            image.enabled = true;
        }
        else
        {
            image.enabled = false;
        }
        text.text = $"{count}";
        text.enabled = (count > 1);
    }

    public bool Overlaps(Vector2 mousePos, Camera camera)
    {
        return rectTransform.ScreenPointOverlaps(mousePos, camera);
    }
}
