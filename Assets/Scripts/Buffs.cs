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

    public void Tick()
    {
        foreach (var buff in activeBuffs)
        {
            buff.tickDuration--;
            buff.type.Run(buff, character);
        }

        activeBuffs.RemoveAll((b) => b.tickDuration <= 0);
    }
}
