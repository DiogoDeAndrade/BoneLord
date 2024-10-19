using UnityEngine;

public enum Faction { Player, Enemy, Environment };

static public class FactionExtensions
{
    public static bool IsEnemy(this Faction f1, Faction f2)
    {
        return (f1 != f2);
    }
}
