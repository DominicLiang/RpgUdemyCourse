using UnityEngine;

public class ThunderStrikeController : MonoBehaviour
{
    [SerializeField] private Damageable damageable;
    [SerializeField] private float speed;

    private bool triggered;
    private Animator anim;

    private void Start()
    {
        anim = GetComponentInChildren<Animator>();
    }

    public void Setup(Damageable damageable)
    {
        this.damageable = damageable;
    }

    private void Update()
    {
        if (!damageable || triggered) return;

        var pos = damageable.transform.position + new Vector3(0, 1);

        transform.position = Vector2.MoveTowards(transform.position, pos, speed * Time.deltaTime);
        transform.right = transform.position - pos;
        if (Vector2.Distance(transform.position, pos) < 0.1f)
        {
            anim.transform.localPosition = new Vector3(0, 0.5f);
            anim.transform.localRotation = Quaternion.identity;
            transform.localScale = new Vector3(3, 3);
            transform.localRotation = Quaternion.identity;
            triggered = true;
            anim.SetTrigger("Hit");
            Invoke(nameof(Damage), 0.1f);
        }
    }

    private void Damage()
    {
        damageable.TakeDamage(PlayerManager.Instance.player);
        Destroy(gameObject, 0.4f);
    }
}
