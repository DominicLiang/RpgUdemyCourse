using UnityEngine;

public class SwordSkill : Skill
{
    [Header("Skill Value")]
    public SwordType swordType = SwordType.Regular;
    [SerializeField] private GameObject swordPrefab;
    [SerializeField] private Vector2 launchForce;
    [SerializeField] private float returnSpeed;
    private float swordGravity;

    [Header("Aim Dots")]
    [SerializeField] private int numberOfDots;
    [SerializeField] private float spaceBetweenDots;
    [SerializeField] private GameObject dotsPrefab;

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

    private GameObject[] dots;
    private Vector2 finalDir;

    protected override void Start()
    {
        base.Start();

        GenereateDots();
    }

    protected override void Update()
    {
        base.Update();

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

        switch (swordType)
        {
            case SwordType.Bounce:
                newSwordController.SetupBounce(true, bounceSpeed, bounceAmount);
                swordGravity = bounceGravity;
                break;
            case SwordType.Pierce:
                newSwordController.SetupPierce(pierceAmount);
                swordGravity = pierceGravity;
                break;
            case SwordType.Spin:
                newSwordController.SetupSpin(true, maxTravelDistance, spinDuration, hitCooldown);
                swordGravity = SpinGravity;
                break;
            default:
                break;
        }

        newSwordController.Setup(finalDir, swordGravity, returnSpeed, player);

        player.UsedSword = newSword;

        SetDotsActive(false);
    }

    #region Aim
    public Vector2 AimDirection()
    {
        var playerPos = player.transform.position;
        var mousePos = Camera.main.ScreenToWorldPoint(player.InputController.mousePosition);
        return (mousePos - playerPos).normalized;
    }

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
            dots[i].transform.SetParent(PlayerManager.Instance.fx.transform);
            dots[i].SetActive(false);
        }
    }

    private Vector2 DotsPosition(float space)
    {
        var aimPos = AimDirection();
        var pos = (Vector2)player.transform.position + new Vector2(0, 1);
        pos += new Vector2(aimPos.x * launchForce.x, aimPos.y * launchForce.y) * space;
        pos += 0.5f * (Physics2D.gravity * swordGravity) * (space * space);
        return pos;
    }
    #endregion

    protected override void SkillFunction()
    {
    }
}
