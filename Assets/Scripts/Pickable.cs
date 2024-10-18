using UnityEngine;

public class Pickable : MonoBehaviour
{
    [SerializeField]
    public Item    item;
   
    void Start()
    {
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer)
        {
            spriteRenderer.sprite = item.sprite;
        }
    }

    void Update()
    {
        
    }

#if UNITY_EDITOR
    private void OnValidate()
    {
        if (item)
        {
            SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
            if (spriteRenderer)
            {
                spriteRenderer.sprite = item.sprite;
            }
        }
    }
#endif
}
