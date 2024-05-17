using UnityEngine;

public class SwordSkill : Skill
{
    [Header("Skill Value")]
    [SerializeField] private GameObject swordPrefab;
    [SerializeField] private Vector2 launchForce;
    [SerializeField] private float swordGravity;

    [Header("Aim Dots")]
    [SerializeField] private int numberOfDots;
    [SerializeField] private float spaceBetweenDots;
    [SerializeField] private GameObject dotsPrefab;

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
        newSwordController.Setup(finalDir, swordGravity, player);

        player.UsedSword = newSword;

        SetDotsActive(false);
    }

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

    protected override void SkillFunction()
    {
    }
}
