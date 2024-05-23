using UnityEngine;

[CreateAssetMenu(fileName = "IceAndFireEffect", menuName = "Data/ItemEffect/IceAndFire")]
public class IceAndFireEffect : ItemEffect
{
    [SerializeField] private GameObject iceAndFirePrefab;
    [SerializeField] private Vector2 velocity;

    public override void ExecuteEffect(GameObject from, GameObject to)
    {
        var pos = from.transform.position + new Vector3(0, 1);
        var rot = from.transform.rotation;
        var parent = PlayerManager.Instance.fx.transform;
        if (!from.TryGetComponent(out Player player)) return;
        var thirdAttack = (player.AttackState as AttackState).comboCounter == 2;
        if (!thirdAttack) return;
        var effect = Instantiate(iceAndFirePrefab, pos, rot, parent);
        if (!effect.TryGetComponent(out Rigidbody2D rb)) return;
        rb.velocity = velocity * player.Flip.facingDir;
    }
}