using System;
using Cinemachine;
using UnityEngine;

public abstract class Damageable : MonoBehaviour
{
    public int currentHp;

    [Header("Major Stats")]
    public Stats Str;
    public Stats Agi;
    public Stats Int;
    public Stats Vit;

    [Header("offensive Stats")]
    public Stats Damage;
    public Stats CritChance;
    public Stats CritPower;

    [Header("Defensive Stats")]
    public Stats MaxHp;
    public Stats Armor;
    public Stats Evasion;
    public Stats MagicResistance;

    [Header("Magic Stats")]
    public Stats FireDamage;
    public Stats IceDamage;
    public Stats LightingDamage;

    public bool IsIgnited;
    public bool IsChilled;
    public bool IsShocked;

    public event Action<GameObject, GameObject> OnTakeDamage;

    protected virtual void Start()
    {
        currentHp = MaxHp.GetValue();
        CritPower.SetDefaultValue(150);
    }

    public virtual void TakeDamage(GameObject from, bool isMagic = false)
    {
        if (!from.TryGetComponent(out Damageable damageFrom)) return;

        var damage = isMagic ? CalculateMagicDamage(damageFrom, this) : CalculateDamage(damageFrom, this);
        currentHp -= damage;

        if (isMagic)
        {
            var fireDamage = damageFrom.FireDamage.GetValue();
            var iceDamage = damageFrom.IceDamage.GetValue();
            var lightingDamage = damageFrom.LightingDamage.GetValue();

            bool canApplyIgnite = fireDamage > iceDamage && fireDamage > lightingDamage;
            bool canApplyChill = iceDamage > lightingDamage && iceDamage > fireDamage;
            bool canApplyShock = lightingDamage > fireDamage && lightingDamage > iceDamage;

            ApplyAilments(canApplyIgnite, canApplyChill, canApplyShock);
        }

        if (currentHp > 0)
        {
            if (damage != 0)
            {
                Debug.Log($"{gameObject.name} 受到了来自 {from.name} 的 {damage} 点伤害");
                OnTakeDamage?.Invoke(from, gameObject);
                AttackSense.Instance.HitPause(0.1f);
                AttackSense.Instance.GetComponent<CinemachineImpulseSource>().GenerateImpulse();
            }
            else
            {
                Debug.Log($"{gameObject.name} 回避了来自 {from.name} 的攻击");
            }
        }
        else
        {
            Die();
        }
    }

    private int CalculateDamage(Damageable from, Damageable to)
    {
        var finalEvasion = to.Evasion.GetValue() + to.Agi.GetValue();
        if (UnityEngine.Random.Range(0, 100) <= finalEvasion) return 0;

        var finalDamage = from.Damage.GetValue() + from.Str.GetValue() - to.Armor.GetValue() - to.Vit.GetValue();

        var finalCritical = CritChance.GetValue() + Agi.GetValue();
        if (UnityEngine.Random.Range(0, 100) <= finalCritical)
        {
            var finalCritPower = (CritPower.GetValue() + Str.GetValue()) * 0.01f;
            finalDamage = Mathf.RoundToInt(finalDamage * finalCritPower);
        }

        finalDamage = Mathf.Clamp(finalDamage, 1, int.MaxValue);
        return finalDamage;
    }

    private int CalculateMagicDamage(Damageable from, Damageable to)
    {
        var fireDamage = from.FireDamage.GetValue();
        var iceDamage = from.IceDamage.GetValue();
        var lightingDamage = from.LightingDamage.GetValue();
        var finalMagicalDamage = fireDamage + iceDamage + lightingDamage + from.Int.GetValue();
        finalMagicalDamage -= to.MagicResistance.GetValue() + (to.Int.GetValue() * 3);
        finalMagicalDamage = Mathf.Clamp(finalMagicalDamage, 1, int.MaxValue);

        return finalMagicalDamage;
    }

    public void ApplyAilments(bool ignite, bool chill, bool shock)
    {
        if (IsIgnited || IsChilled || IsShocked) return;
        IsIgnited = ignite;
        IsChilled = chill;
        IsShocked = shock;
    }

    protected abstract void Die();
}
