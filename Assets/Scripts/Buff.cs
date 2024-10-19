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
        public float elapsedTimePercentage => (Time.time - startTime) / (type.tickDuration * Globals.tickFrequency);
    }

    public enum Type { DOT };

    public Sprite       image;
    public Type         type;
    public int          maxStack = 1;

    [ShowIf("isDamage")]
    public DamageType   damageType;
    [ShowIf("needDuration")]
    public int          tickDuration;
    [ShowIf("isDamage")]
    public int          damagePerTick;

    public bool isDamage => (type == Type.DOT);
    public bool needDuration => (type == Type.DOT);

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

    public bool Run(Instance instance, Character character)
    {
        switch (type)
        {
            case Type.DOT:
                return RunDOT(instance, character);
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
}
