using System;
using System.Collections;
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

    [Header("Ignite")]
    public float igniteDuration;
    private float ignitedTimer;
    private float igniteDamageCooldown = 0.5f;
    private float igniteDamageTimer;
    private int igniteDamage;

    [Header("Chill")]
    public float chillDuration;
    private float chilledTimer;

    [Header("Shock")]
    public GameObject thunderStrikePrefab;
    public float shockDuration;
    private float shockedTimer;

    private bool isDead;

    public event Action<GameObject, GameObject> OnTakeDamage;
    private FlashFX flashFX;
    private Character character;

    protected virtual void Start()
    {
        currentHp = MaxHp.GetValue();
        CritPower.SetDefaultValue(150);
        flashFX = GetComponent<FlashFX>();
        character = GetComponent<Character>();
        isDead = false;
    }

    private void Update()
    {
        ignitedTimer -= Time.deltaTime;
        igniteDamageTimer -= Time.deltaTime;
        chilledTimer -= Time.deltaTime;
        shockedTimer -= Time.deltaTime;

        if (ignitedTimer < 0)
        {
            IsIgnited = false;
        }
        if (chilledTimer < 0)
        {
            IsChilled = false;
        }
        if (shockedTimer < 0)
        {
            IsShocked = false;
        }

        if (IsIgnited && igniteDamageTimer < 0)
        {
            currentHp -= igniteDamage;
            if (currentHp <= 0)
            {
                Die();
            }
            igniteDamageTimer = igniteDamageCooldown;
        }
    }

    public virtual void TakeDamage(GameObject from, bool isMagic = false, bool canEffect = true)
    {
        if (isDead) return;

        var damageFrom = from.GetComponent<Damageable>();

        var damage = isMagic ? CalculateMagicDamage(damageFrom, this) : CalculateDamage(damageFrom, this);
        currentHp -= damage;

        if (from.CompareTag("Player") && canEffect)
        {
            Inventory.Instance.GetEquipmentByType(EquipmentType.Weapon)?.ExecuteItemEffect(from, gameObject);
        }
        if (CompareTag("Player") && ((float)currentHp / MaxHp.GetValue()) < 0.3)
        {
            Inventory.Instance.GetEquipmentByType(EquipmentType.Armor)?.ExecuteItemEffect(from, gameObject);
        }

        if (isMagic)
        {
            var fireDamage = damageFrom.FireDamage.GetValue();
            var iceDamage = damageFrom.IceDamage.GetValue();
            var lightingDamage = damageFrom.LightingDamage.GetValue();

            if (Mathf.Max(fireDamage, iceDamage, lightingDamage) <= 0) return;

            bool canApplyIgnite = fireDamage > iceDamage && fireDamage > lightingDamage;
            bool canApplyChill = iceDamage > lightingDamage && iceDamage > fireDamage;
            bool canApplyShock = lightingDamage > fireDamage && lightingDamage > iceDamage;

            while (!canApplyIgnite && !canApplyChill && !canApplyShock)
            {
                if (UnityEngine.Random.value < 0.3f && fireDamage > 0)
                {
                    canApplyIgnite = true;
                    break;
                }
                if (UnityEngine.Random.value < 0.45f && iceDamage > 0)
                {
                    canApplyChill = true;
                    break;
                }
                if (UnityEngine.Random.value < 0.7f && lightingDamage > 0)
                {
                    canApplyShock = true;
                    break;
                }
            }

            if (canApplyIgnite)
            {
                SetupIgniteDamage(Mathf.RoundToInt(fireDamage * 0.2f));
            }

            ApplyAilments(canApplyIgnite, canApplyChill, canApplyShock);
        }

        if (currentHp > 0)
        {
            if (damage != 0)
            {
                // Debug.Log($"{gameObject.name} 受到了来自 {from.name} 的 {damage} 点伤害");
                OnTakeDamage?.Invoke(from, gameObject);
                AttackSense.Instance.HitPause(0.1f);
                AttackSense.Instance.GetComponent<CinemachineImpulseSource>().GenerateImpulse();
            }
            else
            {
                // Debug.Log($"{gameObject.name} 回避了来自 {from.name} 的攻击");
            }
        }
        else
        {
            isDead = true;
            Die();
        }
    }

    private int CalculateDamage(Damageable from, Damageable to)
    {
        var finalEvasion = to.Evasion.GetValue() + to.Agi.GetValue();
        if (from.IsShocked) finalEvasion += 20;
        if (UnityEngine.Random.Range(0, 100) <= finalEvasion) return 0;

        var finalDamage = from.Damage.GetValue() + from.Str.GetValue() - to.Vit.GetValue();

        if (from.IsChilled)
        {
            finalDamage -= Mathf.RoundToInt(from.Armor.GetValue() * 0.8f);
        }
        else
        {
            finalDamage -= to.Armor.GetValue();
        }

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
        var canApplyIgnite = !IsIgnited && !IsChilled && !IsShocked;
        var canApplyChill = !IsIgnited && !IsChilled && !IsShocked;
        var canApplyShock = !IsIgnited && !IsChilled;

        if (ignite && canApplyIgnite)
        {
            IsIgnited = ignite;
            ignitedTimer = igniteDuration;
            flashFX.AlimentsFxFor(flashFX.igniteColor, igniteDuration);
        }
        if (chill && canApplyChill)
        {
            IsChilled = chill;
            chilledTimer = chillDuration;
            flashFX.AlimentsFxFor(flashFX.chillColor, chillDuration);
            character.SlowBy(0.5f, chillDuration);
        }
        if (shock && canApplyShock)
        {
            if (!IsShocked)
            {
                IsShocked = shock;
                shockedTimer = shockDuration;
                flashFX.AlimentsFxFor(flashFX.shockColor, shockDuration);
            }
            else
            {
                var collider = Physics2D.OverlapCircleAll(transform.position, 25);

                var closeDis = Mathf.Infinity;

                Transform closeEnemy = null;
                foreach (var hit in collider)
                {
                    if (!hit.CompareTag("Enemy") || hit.transform == transform) continue;
                    var disToEnemy = Vector2.Distance(transform.position, hit.transform.position);
                    if (disToEnemy >= closeDis) continue;
                    closeDis = disToEnemy;
                    closeEnemy = hit.transform;
                }
                if (!closeEnemy || CompareTag("player")) closeEnemy = transform;
                var pos = transform.position + new Vector3(0, 1);
                var parent = PlayerManager.Instance.fx.transform;
                var strike = Instantiate(thunderStrikePrefab, pos, Quaternion.identity, parent);
                if (!strike.TryGetComponent(out ThunderStrikeController c)) return;
                if (!closeEnemy.TryGetComponent(out Damageable damageable)) return;
                c.Setup(damageable);
            }
        }
    }

    public void SetupIgniteDamage(int damage)
    {
        igniteDamage = damage;
    }

    public virtual void IncreaseHealthBy(int amount)
    {
        currentHp += amount;
        if (currentHp > MaxHp.GetValue())
            currentHp = MaxHp.GetValue();
    }

    public virtual void IncreaseStatBy(int modifier, float duration, Stats statsToModify)
    {
        StartCoroutine(StatModifier(modifier, duration, statsToModify));
        IEnumerator StatModifier(int modifier, float duration, Stats statsToModify)
        {
            statsToModify.AddModifier(modifier);
            yield return new WaitForSeconds(duration);
            statsToModify.RemoveModifier(modifier);
        }
    }

    protected abstract void Die();

    public Stats StatsOfType(StatType type) => type switch
    {
        StatType.Strength => Str,
        StatType.Agility => Agi,
        StatType.Intelegence => Int,
        StatType.Vitality => Vit,
        StatType.Damage => Damage,
        StatType.CritChance => CritChance,
        StatType.CritPower => CritPower,
        StatType.Health => MaxHp,
        StatType.Armor => Armor,
        StatType.Evasion => Evasion,
        StatType.MagicRes => MagicResistance,
        StatType.FireDamage => FireDamage,
        StatType.IceDamage => IceDamage,
        StatType.LightingDamage => LightingDamage,
        _ => null
    };
}
