using UnityEngine;

[CreateAssetMenu(fileName = "HealEffect", menuName = "Data/ItemEffect/HealEffect")]
public class HealEffect : ItemEffect
{
    [Range(0f, 1f)]
    [SerializeField] private float healPercent;

    public override void ExecuteEffect(GameObject from, GameObject to)
    {
        var stats = PlayerManager.Instance.player.GetComponent<Damageable>();
        int healAmount = Mathf.RoundToInt(stats.MaxHp.GetValue() * healPercent);
        stats.IncreaseHealthBy(healAmount);
    }
}