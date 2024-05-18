using UnityEngine;

public class SwordSkill : Skill
{
    [Header("Skill Value")]
    [SerializeField] private SwordType swordType = SwordType.Regular;
    [SerializeField] private GameObject swordPrefab;
    [SerializeField] private Vector2 launchForce;
    [SerializeField] private float returnSpeed;

    [Header("Dots Value")]
    [SerializeField] private int numberOfDots;
    [SerializeField] private float spaceBetweenDots;
    [SerializeField] private GameObject dotsPrefab;

    [Header("Regular Value")]
    [SerializeField] private float regularGravity;

    [Header("Bounce Value")]
    [SerializeField] private float bounceGravity;
    [SerializeField] private float bounceSpeed;
    [SerializeField] private int bounceAmount;

    [Header("Pierce Value")]
    [SerializeField] private float pierceGravity;
    [SerializeField] private int pierceAmount;

    [Header("Spin Value")]
    [SerializeField] private float SpinGravity;
    [SerializeField] private float maxTravelDistance;
    [SerializeField] private float spinDuration;
    [SerializeField] private float hitCooldown;

    private float swordGravity;
    private GameObject[] dots;
    private Vector2 finalDir;

    protected override void Start()
    {
        base.Start();

        SetGravity();

        GenereateDots();
    }

    protected override void Update()
    {
        base.Update();

        SetGravity();

        if (!player.InputController.isAimSwordPressed)
        {
            var dir = AimDirection();
            finalDir = new Vector2(dir.x * launchForce.x, dir.y * launchForce.y);
        }
        else
        {
            for (int i = 0; i < numberOfDots; i++)
            {
                dots[i].transform.position = DotsPosition(i * spaceBetweenDots);
            }
        }
    }

    public void CreateSword()
    {
        var newSword = Instantiate(swordPrefab, player.transform.position + new Vector3(0, 1, 0), transform.rotation);
        newSword.transform.SetParent(PlayerManager.Instance.fx.transform);
        var newSwordController = newSword.GetComponent<SwordSKillController>();

        newSwordController.Setup(
            swordType,
            player,
            finalDir,
            swordGravity,
            returnSpeed,
            bounceSpeed,
            bounceAmount,
            pierceAmount,
            maxTravelDistance,
            spinDuration,
            hitCooldown);

        player.UsedSword = newSword;

        SetDotsActive(false);
    }

    private void SetGravity()
    {
        switch (swordType)
        {
            case SwordType.Regular:
                swordGravity = regularGravity;
                break;
            case SwordType.Bounce:
                swordGravity = bounceGravity;
                break;
            case SwordType.Pierce:
                swordGravity = pierceGravity;
                break;
            case SwordType.Spin:
                swordGravity = SpinGravity;
                break;
            default:
                break;
        }
    }

    #region Aim
    public void SetDotsActive(bool isActive)
    {
        for (int i = 0; i < numberOfDots; i++)
        {
            dots[i].SetActive(isActive);
        }
    }

    private void GenereateDots()
    {
        dots = new GameObject[numberOfDots];
        var parent = PlayerManager.Instance.fx.transform;
        for (int i = 0; i < numberOfDots; i++)
        {
            dots[i] = Instantiate(dotsPrefab, player.transform.position + new Vector3(0, 1, 0), Quaternion.identity, parent);
            dots[i].SetActive(false);
        }
    }

    private Vector2 AimDirection()
    {
        var playerPos = player.transform.position + new Vector3(0, 1, 0);
        var mousePos = Camera.main.ScreenToWorldPoint(player.InputController.mousePosition);
        return (mousePos - playerPos).normalized;
    }

    private Vector2 DotsPosition(float space)
    {
        var aimPos = AimDirection();
        var pos = (Vector2)player.transform.position + new Vector2(0, 1)
                + new Vector2(aimPos.x * launchForce.x, aimPos.y * launchForce.y) * space
                + 0.5f * (Physics2D.gravity * swordGravity) * (space * space);
        return pos;
    }
    #endregion

    protected override void SkillFunction()
    {
    }
}
