using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DisplayItem : MonoBehaviour
{
    [SerializeField] private Image              image;
    [SerializeField] private TextMeshProUGUI    text;

    private Item    item;

    private void Start()
    {
    }

    public void SetItem(Item item, int count)
    {
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
}
