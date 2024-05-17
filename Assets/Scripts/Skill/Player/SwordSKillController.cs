using UnityEngine;

public class SwordSKillController : MonoBehaviour
{
    [SerializeField] private float returnSpeed = 12f;
    private bool canRotate;
    private bool isReturning;

    //todo 自动删除

    private Animator anim;
    private Rigidbody2D rb;
    private CircleCollider2D cd;
    private Player player;

    private void Awake()
    {
        anim = GetComponentInChildren<Animator>();
        rb = GetComponent<Rigidbody2D>();
        cd = GetComponent<CircleCollider2D>();
        canRotate = true;
    }

    public void Setup(Vector2 dir, float gravityScale, Player player)
    {
        rb.velocity = dir;
        rb.gravityScale = gravityScale;
        this.player = player;

        anim.SetBool("Rotation", true);
    }

    private void Update()
    {
        if (canRotate)
            transform.right = rb.velocity;

        if (isReturning)
        {
            anim.SetBool("Rotation", true);
            var returnPos = player.transform.position + new Vector3(0, 1, 0);
            transform.position = Vector2.MoveTowards(transform.position, returnPos, returnSpeed * Time.deltaTime);
            if (Vector2.Distance(transform.position, returnPos) < 1)
            {
                player.CatchSword();
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (isReturning || other.CompareTag("Player")) return;
        canRotate = false;
        cd.enabled = false;
        rb.isKinematic = true;
        rb.constraints = RigidbodyConstraints2D.FreezeAll;
        transform.SetParent(other.transform);
        anim.SetBool("Rotation", false);
    }

    public void ReturnSword()
    {
        rb.constraints = RigidbodyConstraints2D.FreezeAll;
        transform.SetParent(PlayerManager.Instance.fx.transform);
        isReturning = true;
    }
}
