using UnityEngine;

public class Globals : MonoBehaviour
{
    [SerializeField, Header("Name Color")] 
    Color _playerColor = Color.white;
    [SerializeField] 
    Color _enemyColor = Color.white;
    [SerializeField] 
    Color _environmentColor = Color.white;
    [SerializeField, Header("Damage Type Color")]
    Color _damagePhysical = Color.white;
    [SerializeField]
    Color _damageFire = Color.white;
    [SerializeField]
    Color _damageIce = Color.white;
    [SerializeField]
    Color _damageMagic = Color.white;
    [SerializeField]
    Color _damagePoison = Color.white;

    static Globals Instance;

    private void Awake()
    {
        Instance = this;
    }

    public static Color GetColor(Faction faction)
    {
        switch (faction)
        {
            case Faction.Player: return Instance._playerColor;
            case Faction.Enemy: return Instance._enemyColor;
            case Faction.Environment: return Instance._environmentColor;
        }

        return Color.white;
    }

    public static Color GetColor(DamageType type)
    {
        switch (type)
        {
            case DamageType.Physical: return Instance._damagePhysical;
            case DamageType.Fire: return Instance._damageFire;
            case DamageType.Ice: return Instance._damageIce;
            case DamageType.Magic: return Instance._damageMagic;
            case DamageType.Poison: return Instance._damagePoison;
        }

        return Color.white;
    }
}
