using UnityEngine;

public class GameCursor : FollowMouse
{
    SpriteRenderer spriteRenderer;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void SetImage(Sprite sprite, float scale)
    {
        spriteRenderer.sprite = sprite;
        transform.localScale = new Vector3(scale, scale, 1);
    }

    public Sprite GetImage()
    {
        return spriteRenderer.sprite;
    }
}
