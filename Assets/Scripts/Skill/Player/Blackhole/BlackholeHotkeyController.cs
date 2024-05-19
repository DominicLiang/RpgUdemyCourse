using TMPro;
using UnityEngine;

public class BlackholeHotkeyController : MonoBehaviour
{
    private KeyCode myHotkey;
    private TextMeshProUGUI myText;
    private Transform enemy;
    private BlackholeSkillController blackholeAC;
    private SpriteRenderer sr;

    public void SetupHotkey(KeyCode myHotkey, Transform enemy, BlackholeSkillController blackholeAC)
    {
        myText = GetComponentInChildren<TextMeshProUGUI>();
        myText.text = myHotkey.ToString();

        this.myHotkey = myHotkey;
        this.enemy = enemy;
        this.blackholeAC = blackholeAC;

        sr = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(myHotkey))
        {
            blackholeAC.AddEnemyToList(enemy);

            myText.color = Color.clear;
            sr.color = Color.clear;
        }
    }
}
