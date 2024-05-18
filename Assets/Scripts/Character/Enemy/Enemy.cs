using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Damageable))]
[RequireComponent(typeof(FlashFX))]
public class Enemy : Character
{
    #region Value
    [Header("Move Value")]
    public float defaultMoveSpeed = 7f;
    [HideInInspector] public float moveSpeed;

    [Header("Attack Value")]
    public float attackDistance = 2f;
    public float attackCooldown = 0.4f;
    public float lostPlayerTime = 7f;

    [Header("Stun Value")]
    public float stunTime = 2f;
    public bool canBeStun = true;
    [HideInInspector] public GameObject counterImage;
    #endregion

    #region Component
    public Damageable Damageable { get; private set; }
    public FlashFX FlashFX { get; private set; }
    #endregion

    protected override void Start()
    {
        base.Start();

        moveSpeed = defaultMoveSpeed;

        Damageable = GetComponent<Damageable>();
        FlashFX = GetComponent<FlashFX>();

        counterImage = transform.Find("CounterImage").gameObject;
    }

    public void FreezeTimeForSeconds(float seconds)
    {
        StartCoroutine(FreezeTimeFor(seconds));
    }

    protected virtual IEnumerator FreezeTimeFor(float seconds)
    {
        FreezeTime(true);
        yield return new WaitForSeconds(seconds);
        FreezeTime(false);
    }

    public virtual void FreezeTime(bool toggle)
    {
        if (toggle)
        {
            moveSpeed = 0;
            Anim.speed = 0;
        }
        else
        {
            moveSpeed = defaultMoveSpeed;
            Anim.speed = 1;
        }
    }

    public virtual void OpenCounterAttackWindow()
    {
        canBeStun = true;
        counterImage.SetActive(true);
    }

    public virtual void CloseCounterAttackWindow()
    {
        canBeStun = false;
        counterImage.SetActive(false);
    }
}
