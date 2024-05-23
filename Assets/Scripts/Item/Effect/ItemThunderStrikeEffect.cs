using UnityEngine;

[CreateAssetMenu(fileName = "ThunderStrikeEffect", menuName = "Data/ItemEffect/ThunderStrike")]
public class ItemThunderStrikeEffect : ItemEffect
{
    [SerializeField] private GameObject thunderStrikePrefab;
    public override void ExecuteEffect(GameObject from, GameObject to)
    {
        var parent = PlayerManager.Instance.fx.transform;
        Instantiate(thunderStrikePrefab, to.transform.position, Quaternion.identity, parent);
    }
}