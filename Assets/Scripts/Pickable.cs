using UnityEngine;

public class Pickable : MonoBehaviour
{
    [SerializeField]
    public ItemDef    item;
   
    void Start()
    {
        SetItem(item);
    }

    void Update()
    {
        
    }
    internal void SetItem(ItemDef item)
    {
        this.item = item;

        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer)
        {
            spriteRenderer.sprite = item.sprite;
        }
    }

#if UNITY_EDITOR
    private void OnValidate()
    {
        if (item)
        {
            SetItem(item);
        }
    }
#endif

}
