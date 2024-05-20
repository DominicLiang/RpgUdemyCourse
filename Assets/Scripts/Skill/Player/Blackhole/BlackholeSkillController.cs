using System.Collections.Generic;
using UnityEngine;

public class BlackholeSkillController : MonoBehaviour
{
    public GameObject hotkeyPrefab;
    public bool PlayerCanExitState { get; private set; }

    private float maxSize;
    private float growSpeed;
    private float shrinkSpeed;
    private bool canGrow;
    private bool canShrink;

    private int amountOfAttacks;
    private float cloneAttackCooldown;

    private float cloneAttackTimer;
    private bool cloneAttackReleased;

    private float blackholeTimer;
    private float blackholeDuration;

    private bool playerCanDisaper;

    private Player player;
    private List<KeyCode> keyCodes;
    private List<Transform> targets;
    private List<GameObject> createdHotkeys;

    private void Awake()
    {
        keyCodes = new List<KeyCode>
            {
                KeyCode.A,
                KeyCode.S,
                KeyCode.D,
                KeyCode.W,
                KeyCode.E
            };
        targets = new List<Transform>();
        createdHotkeys = new List<GameObject>();
        canGrow = true;
    }

    public void Setup(
        Player player,
        float maxSize,
        float growSpeed,
        float shrinkSpeed,
        int amountOfAttacks,
        float cloneAttackCooldown,
        float blackholeDuration)
    {
        this.player = player;
        this.maxSize = maxSize;
        this.growSpeed = growSpeed;
        this.shrinkSpeed = shrinkSpeed;
        this.amountOfAttacks = amountOfAttacks;
        this.cloneAttackCooldown = cloneAttackCooldown;
        this.blackholeDuration = blackholeDuration;
        blackholeTimer = blackholeDuration;

        playerCanDisaper = !SkillManager.Instance.Clone.crystalInsteadOfClone;
    }

    private void Update()
    {
        cloneAttackTimer -= Time.deltaTime;
        blackholeTimer -= Time.deltaTime;

        if (blackholeTimer < 0)
        {
            blackholeTimer = Mathf.Infinity;

            if (targets.Count > 0)
            {
                ReleaseCloneAttack();
            }
            else
            {
                FinishBlackhole();
            }
        }

        if (Input.GetKeyDown(KeyCode.F))
        {
            ReleaseCloneAttack();
        }

        CloneAttackLogic();

        if (canGrow && !canShrink)
        {
            var size = new Vector2(maxSize, maxSize);
            transform.localScale = Vector2.Lerp(transform.localScale, size, growSpeed * Time.deltaTime);
        }

        if (canShrink)
        {
            var size = new Vector2(-1, -1);
            transform.localScale = Vector2.Lerp(transform.localScale, size, shrinkSpeed * Time.deltaTime);
            if (transform.localScale.x <= 0)
            {
                Destroy(gameObject);
            }
        }
    }

    private void ReleaseCloneAttack()
    {
        if (targets.Count <= 0) return;
        cloneAttackReleased = true;
        DestoryHotkeys();
        if (!playerCanDisaper) return;
        player.MakeTransprent(true);
    }

    private void FinishBlackhole()
    {
        cloneAttackReleased = false;
        canShrink = true;
        PlayerCanExitState = true;
        DestoryHotkeys();
    }

    private void CloneAttackLogic()
    {
        if (cloneAttackTimer >= 0 || !cloneAttackReleased || amountOfAttacks <= 0) return;

        cloneAttackTimer = cloneAttackCooldown;

        int randomIndex = Random.Range(0, targets.Count);
        var pos = targets[randomIndex].position;

        var offset = new Vector3(Random.Range(0, 100) > 50 ? 2 : -2, 0, 0);

        if (SkillManager.Instance.Clone.crystalInsteadOfClone)
        {
            SkillManager.Instance.Crystal.CreateCrystal();
            SkillManager.Instance.Crystal.CurrentCrystalChooseRandomTarget();
        }
        else
        {
            SkillManager.Instance.Clone.CreateClone(pos, Quaternion.identity, offset);
        }

        amountOfAttacks--;

        if (amountOfAttacks > 0) return;
        Invoke(nameof(FinishBlackhole), 0.5f);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Enemy")) return;

        if (other.TryGetComponent(out Enemy enemy)) enemy.FreezeTime(true);

        CreateHotkey(other);
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (!other.CompareTag("Enemy")) return;

        if (other.TryGetComponent(out Enemy enemy)) enemy.FreezeTime(false);
    }

    private void CreateHotkey(Collider2D other)
    {
        if (keyCodes.Count <= 0 || cloneAttackReleased) return;

        var pos = other.transform.position + new Vector3(0, 2);
        var parent = PlayerManager.Instance.fx.transform;
        var newHotkey = Instantiate(hotkeyPrefab, pos, Quaternion.identity, parent);
        if (!createdHotkeys.Exists(h => h == newHotkey))
        {
            createdHotkeys.Add(newHotkey);
        }
        else
        {
            Destroy(newHotkey);
            return;
        }

        KeyCode choosenKey = keyCodes[Random.Range(0, keyCodes.Count)];
        keyCodes.Remove(choosenKey);

        var blackholeAC = newHotkey.GetComponent<BlackholeHotkeyController>();
        if (!blackholeAC) return;

        blackholeAC.SetupHotkey(choosenKey, other.transform, this);
    }

    private void DestoryHotkeys()
    {
        if (createdHotkeys.Count <= 0) return;
        for (int i = 0; i < createdHotkeys.Count; i++)
        {
            Destroy(createdHotkeys[i]);
        }
        createdHotkeys.Clear();
    }

    public void AddEnemyToList(Transform enemy)
    {
        if (targets.Exists(e => e == enemy)) return;
        targets.Add(enemy);
    }
}
