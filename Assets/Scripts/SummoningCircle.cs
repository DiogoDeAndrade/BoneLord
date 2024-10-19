using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using NaughtyAttributes;

public class SummoningCircle : MonoBehaviour
{
    [SerializeField] private Hypertag boneLord;

    Coroutine summonCR;

    Character playerCharacter;

    private void Start()
    {
        playerCharacter = gameObject.FindObjectOfTypeWithHypertag<Character>(boneLord);
    }

    public bool IsValid(List<Item> items)
    {
        if (summonCR != null) return false;

        return true;
    }

    public void Summon(List<Item> items)
    {
        if (summonCR != null) return;

        summonCR = StartCoroutine(SummonCR(items));
    }

    [Button("Test")]
    void test()
    {
        summonCR = StartCoroutine(SummonCR(null));
    }

    IEnumerator SummonCR(List<Item> items)
    {
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        Color originalColor = spriteRenderer.color;

        playerCharacter?.HoldCast(true);

        for (int i = 0; i < 10; i++)
        {
            yield return new WaitForSeconds(0.2f);

            GameObject go = new GameObject();
            go.transform.position = transform.position;
            go.transform.rotation = transform.rotation;
            go.transform.localScale = transform.localScale;
            var dat = go.AddComponent<DestroyAfterTime>();
            dat.time = 0.5f;
            dat.fadeOut = true;
            var spr = go.AddComponent<SpriteRenderer>();
            spr.color = originalColor.ChangeAlpha(0.5f);
            spr.sprite = spriteRenderer.sprite;
            var mov = go.AddComponent<MoveAnim>();
            mov.speed = Vector3.up * 50.0f;
        }

        // Spawn creature

        for (int i = 0; i < 10; i++)
        {
            yield return new WaitForSeconds(0.2f);

            GameObject go = new GameObject();
            go.transform.position = transform.position;
            go.transform.rotation = transform.rotation;
            go.transform.localScale = transform.localScale;
            var dat = go.AddComponent<DestroyAfterTime>();
            dat.time = 0.5f;
            dat.fadeOut = true;
            var spr = go.AddComponent<SpriteRenderer>();
            spr.color = originalColor.ChangeAlpha(0.5f);
            spr.sprite = spriteRenderer.sprite;
            var mov = go.AddComponent<MoveAnim>();
            mov.speed = Vector3.up * 50.0f;
        }

        playerCharacter?.HoldCast(false);

        summonCR = null;
    }
}
