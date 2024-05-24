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
        stats.IncreaseStatBy(buffAmount, buffDuration, stats.StatsOfType(buffType));
    }
}