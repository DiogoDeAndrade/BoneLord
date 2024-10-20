using NaughtyAttributes;
using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Item", menuName = "Bonelord/Item")]
public class ItemDef : ScriptableObject
{
    public Sprite           sprite;
    public string           displayName;
    [Header("Skeleton Summon Properties")]
    public int              hp;
    public string           title;
    public bool             hasColor;
    [ShowIf("hasColor")]
    public Color            color = Color.white;
    [ShowIf("hasColor")]
    public int              colorPriority;
    public List<Buff>       buffsToApply;
    public List<Hypertag>   categories;

    internal bool IsA(Hypertag tag)
    {
        if (categories == null) return false;

        foreach (var category in categories)
        {
            if (category == tag) return true;
        }

        return false;
    }
}
