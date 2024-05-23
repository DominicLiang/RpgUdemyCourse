using JetBrains.Annotations;
using UnityEngine;

[CreateAssetMenu(fileName = "NewItemData", menuName = "Data/Equipment")]
public class ItemDataEquipment : ItemData
{
    public float ItemCooldown;

    public EquipmentType equipmentType;

    public ItemEffect[] itemEffects;

    [Header("Major Stats")]
    public int strength;
    public int agility;
    public int intelligence;
    public int vitality;

    [Header("Offensive Stats")]
    public int damage;
    public int critChance;
    public int critPower;

    [Header("Defensive Stats")]
    public int health;
    public int armor;
    public int evasion;
    public int magicResistance;

    [Header("Magic Stats")]
    public int fireDamage;
    public int iceDamege;
    public int lightingDamage;

    public void ExecuteItemEffect(GameObject from, GameObject to)
    {
        foreach (var effect in itemEffects)
        {
            effect.ExecuteEffect(from, to);
        }
    }

    public void AddModifiers()
    {
        var playerDamageable = PlayerManager.Instance.player.GetComponent<PlayerDamageable>();
        playerDamageable.Str.AddModifier(strength);
        playerDamageable.Agi.AddModifier(agility);
        playerDamageable.Int.AddModifier(intelligence);
        playerDamageable.Vit.AddModifier(vitality);

        playerDamageable.Damage.AddModifier(damage);
        playerDamageable.CritChance.AddModifier(critChance);
        playerDamageable.CritPower.AddModifier(critPower);

        playerDamageable.MaxHp.AddModifier(health);
        playerDamageable.Armor.AddModifier(armor);
        playerDamageable.Evasion.AddModifier(evasion);
        playerDamageable.MagicResistance.AddModifier(magicResistance);

        playerDamageable.FireDamage.AddModifier(fireDamage);
        playerDamageable.IceDamage.AddModifier(iceDamege);
        playerDamageable.LightingDamage.AddModifier(lightingDamage);
    }

    public void RemoveModifiers()
    {
        var playerDamageable = PlayerManager.Instance.player.GetComponent<PlayerDamageable>();
        playerDamageable.Str.RemoveModifier(strength);
        playerDamageable.Agi.RemoveModifier(agility);
        playerDamageable.Int.RemoveModifier(intelligence);
        playerDamageable.Vit.RemoveModifier(vitality);
        playerDamageable.Damage.RemoveModifier(damage);
        playerDamageable.CritChance.RemoveModifier(critChance);
        playerDamageable.CritPower.RemoveModifier(critPower);

        playerDamageable.MaxHp.RemoveModifier(health);
        playerDamageable.Armor.RemoveModifier(armor);
        playerDamageable.Evasion.RemoveModifier(evasion);
        playerDamageable.MagicResistance.RemoveModifier(magicResistance);

        playerDamageable.FireDamage.RemoveModifier(fireDamage);
        playerDamageable.IceDamage.RemoveModifier(iceDamege);
        playerDamageable.LightingDamage.RemoveModifier(lightingDamage);
    }
}