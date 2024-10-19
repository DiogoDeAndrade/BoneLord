using System;
using UnityEditorInternal.Profiling.Memory.Experimental;
using UnityEngine;

public class Pickable : MonoBehaviour
{
    [SerializeField]
    public Item    item;
   
    void Start()
    {
        SetItem(item);
    }

    void Update()
    {
        
    }
    internal void SetItem(Item item)
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
