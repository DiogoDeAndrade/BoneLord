using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using NaughtyAttributes;

public class SummoningCircle : MonoBehaviour
{
    [SerializeField] private Hypertag   boneLord;
    [SerializeField] private Character  skeletonPrefab;
    [SerializeField] private Hypertag   boneItemTag;
    [SerializeField] private Hypertag   toolItemTag;
    [SerializeField] private Hypertag   minerItemTag;

    Coroutine summonCR;

    Character playerCharacter;

    private void Start()
    {
        playerCharacter = gameObject.FindObjectOfTypeWithHypertag<Character>(boneLord);
    }

    public bool IsValid(List<ItemDef> items)
    {
        if (summonCR != null) return false;

        int toolCount = 0;
        int boneCount = 0;
        foreach (var item in items)
        {
            if (item.IsA(boneItemTag)) boneCount++;
            if (item.IsA(toolItemTag)) toolCount++;
        }

        if (boneCount == 0) return false;
        if (toolCount > 1) return false;

        return true;
    }

    public void Summon(List<ItemDef> items)
    {
        if (summonCR != null) return;

        summonCR = StartCoroutine(SummonCR(items));
    }

    IEnumerator SummonCR(List<ItemDef> items)
    {
        SpriteRenderer circleSpriteRenderer = GetComponent<SpriteRenderer>();
        Color circleOriginalColor = circleSpriteRenderer.color;

        playerCharacter?.HoldCast(true);

        for (int i = 0; i < 5; i++)
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
            spr.color = circleOriginalColor.ChangeAlpha(0.5f);
            spr.sprite = circleSpriteRenderer.sprite;
            var mov = go.AddComponent<MoveAnim>();
            mov.speed = Vector3.up * 50.0f;
        }

        // Count bones
        ItemDef     tool = null;
        int         nBones = 0;
        int         HP = 10;
        Color       color = Globals.defaultSkeletonColor;
        int         colorPriority = -int.MaxValue;
        foreach (var item in items)
        {
            if (item.IsA(boneItemTag))
            {
                nBones++;
            }
            else if ((toolItemTag != null) && (item.IsA(toolItemTag)))
            {
                tool = item;
            }
            HP += (item.hp == 0) ? (Globals.defaultHPPerItem) : (item.hp);
            if (item.hasColor)
            {
                if (item.colorPriority > colorPriority)
                {
                    colorPriority = item.colorPriority;
                    color = item.color;
                }
            }
        }

        // Spawn creature
        Character newSkeleton = Instantiate(skeletonPrefab, transform.position, Quaternion.identity);
        newSkeleton.displayName = "Skeleton";
        newSkeleton.SetMaxHP(HP);
        var spriteRenderer = newSkeleton.GetComponent<SpriteRenderer>();
        spriteRenderer.color = color;
        var flash = newSkeleton.GetComponent<Flash>();

        if (nBones == items.Count)
        {
            newSkeleton.displayName = "Buff Skeleton";
        }
        if (tool == null)
        {
            float attackSpeed = 0.5f + nBones / items.Count;

            // Add melee attack component
            MeleeAttack ma = newSkeleton.gameObject.AddComponent<MeleeAttack>();
            ma.Set(20, 0.5f, attackSpeed, nBones, DamageType.Physical);
        }
        else
        {
            newSkeleton.SetTool(tool);

            if (tool.IsA(minerItemTag))
            {
                var miner = newSkeleton.gameObject.AddComponent<ResourceGather>();
                miner.SetTool(tool);
            }
        }

        yield return null;

        flash?.Execute(0.5f, Color.cyan, Color.cyan.ChangeAlpha(0));

        for (int i = 0; i < 5; i++)
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
            spr.color = circleOriginalColor.ChangeAlpha(0.5f);
            spr.sprite = circleSpriteRenderer.sprite;
            var mov = go.AddComponent<MoveAnim>();
            mov.speed = Vector3.up * 50.0f;
        }

        playerCharacter?.HoldCast(false);

        summonCR = null;
    }
}
