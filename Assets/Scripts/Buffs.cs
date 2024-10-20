using System;
using System.Collections.Generic;
using UnityEngine;

public class Buffs
{
    List<Buff.Instance> activeBuffs;
    Character           character;

    public Buffs(Character character)
    {
        activeBuffs = new();
        this.character = character;
    }

    public int Count => activeBuffs.Count;

    public bool Apply(Buff buffType)
    {
        var instance = GetInstance(buffType);
        if (instance != null)
        {
            if (instance.stackCount < buffType.maxStack)
            {
                instance.stackCount++;
            }
            instance.Reset();
            return true;
        }

        activeBuffs.Add(buffType.Start());

        return true;
    }

    public Buff.Instance GetInstance(Buff buffType)
    {
        foreach (var buff in activeBuffs)
        {
            if (buff.type == buffType)
            {
                return buff;
            }
        }

        return null;
    }
    public Buff.Instance GetInstance(int i)
    {
        if (activeBuffs.Count > i)
        {
            return activeBuffs[i];
        }

        return null;
    }

    public void Tick()
    {
        List<Buff.Instance> toDelete = new();

        foreach (var buff in activeBuffs)
        {
            if (buff.tickDuration > 0)
            {
                buff.tickDuration--;
                if (buff.tickDuration <= 0) toDelete.Add(buff);
            }
            buff.type.RunTick(buff, character);
        }

        activeBuffs.RemoveAll((b) => toDelete.IndexOf(b) != -1);
    }

    public (float, DamageType) ModifyDamage(float damage, DamageType damageType)
    {
        var processedDamage = damage;
        var processedDamageType = damageType;

        foreach (var buff in activeBuffs)
        {
            (processedDamage, processedDamageType) = buff.type.ModifyDamage(buff, character, processedDamage, processedDamageType);
        }

        return (processedDamage, processedDamageType);
    }
}
