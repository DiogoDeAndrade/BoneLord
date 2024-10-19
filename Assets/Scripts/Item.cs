using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Item", menuName = "Bonelord/Item")]
public class Item : ScriptableObject
{
    public Sprite           sprite;
    public string           displayName;
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
