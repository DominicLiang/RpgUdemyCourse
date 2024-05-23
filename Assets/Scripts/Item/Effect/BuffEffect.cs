using UnityEngine;

[CreateAssetMenu(fileName = "BuffEffect", menuName = "Data/ItemEffect/BuffEffect")]
public class BuffEffect : ItemEffect
{
    private Damageable stats;
    [SerializeField] private StatType buffType;
    [SerializeField] private int buffAmount;
    [SerializeField] private int buffDuration;

    public override void ExecuteEffect(GameObject from, GameObject to)
    {
        stats = PlayerManager.Instance.player.GetComponent<Damageable>();
        stats.IncreaseStatBy(buffAmount, buffDuration, StatsToModify());
    }

    public Stats StatsToModify() => buffType switch
    {
        StatType.Strength => stats.Str,
        StatType.Agility => stats.Agi,
        StatType.Intelegence => stats.Int,
        StatType.Vitality => stats.Vit,
        StatType.Damage => stats.Damage,
        StatType.CritChance => stats.CritChance,
        StatType.CritPower => stats.CritPower,
        StatType.Health => stats.MaxHp,
        StatType.Armor => stats.Armor,
        StatType.Evasion => stats.Evasion,
        StatType.MagicRes => stats.MagicResistance,
        StatType.FireDamage => stats.FireDamage,
        StatType.IceDamage => stats.IceDamage,
        StatType.LightingDamage => stats.LightingDamage,
        _ => null
    };
}