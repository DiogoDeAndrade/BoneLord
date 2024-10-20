using NaughtyAttributes;
using System.Runtime.CompilerServices;
using UnityEngine;

[CreateAssetMenu(fileName = "Buff", menuName = "Bonelord/Buff")]
public class Buff : ScriptableObject
{
    public class Instance
    {
        public Buff     type;
        public int      stackCount;
        public int      tickDuration;
        public float    startTime;

        public void Reset()
        {
            tickDuration = type.tickDuration;
            startTime = Time.time;
        }

        public float elapsedTime => Time.time - startTime;
        public float elapsedTimePercentage => (tickDuration == 0) ? (0.0f) : ((Time.time - startTime) / (type.tickDuration * Globals.tickFrequency));
    }

    public enum Type { DOT, ModifyHPChange };
    public enum FilterChange { Negative, Any, Positive };

    [ResizableTextArea]
    public string       description;
    public Sprite       image;
    public Type         type;
    public int          maxStack = 1;

    [ShowIf("needDamageType")]
    public DamageType   damageType;
    [ShowIf("needDuration")]
    public int          tickDuration;
    [ShowIf("isDamage")]
    public int          damagePerTick;
    [ShowIf("needFilter")]
    public FilterChange filterChange = FilterChange.Any;
    [ShowIf("isModify")]
    public float        multiplier = 1.0f;
    [ShowIf("isModify")]
    public float        constant = 0;
    [ShowIf("isModify")]
    public bool         anyDamageType = true;

    public bool isDamage => (type == Type.DOT);
    public bool isModify => (type == Type.ModifyHPChange);
    public bool needDuration => (type == Type.DOT) || (type == Type.ModifyHPChange);
    public bool needDamageType => (type == Type.DOT) || ((type == Type.ModifyHPChange) && (!anyDamageType));
    public bool needFilter => (type == Type.ModifyHPChange);

    public Instance Start()
    {
        Instance newInstance = new Instance()
        {
            type = this,
            stackCount = 1
        };

        newInstance.Reset();

        return newInstance;
    }

    public bool RunTick(Instance instance, Character character)
    {
        switch (type)
        {
            case Type.DOT:
                return RunDOT(instance, character);
            case Type.ModifyHPChange:
                // Nothing to do on a tick, it just affects damage that goes in the system
                break;
            default:
                break;
        }

        return false;
    }

    private bool RunDOT(Instance instance, Character character)
    {
        character.DealDamage(damagePerTick, damageType);

        return true;
    }

    internal (float, DamageType) ModifyDamage(Instance buff, Character character, float damage, DamageType damageType)
    {
        if (!anyDamageType)
        {
            if (damageType != this.damageType)
            {
                return (damage, damageType);
            }
        }
        switch (filterChange)
        {
            case FilterChange.Negative:
                if (damage >= 0) return (damage, damageType);
                break;
            case FilterChange.Positive:
                if (damage <= 0) return (damage, damageType);
                break;
        }

        return (damage * multiplier + constant, damageType);
    }
}
