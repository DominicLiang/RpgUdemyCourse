using TMPro;
using UnityEngine;

public class StatsSlot : MonoBehaviour
{
    [SerializeField] private string statName;
    [SerializeField] private StatType statType;
    [SerializeField] private TextMeshProUGUI statNameText;
    [SerializeField] private TextMeshProUGUI statValueText;

    private void OnValidate()
    {
        gameObject.name = "Stat - " + statName;
        if (statNameText) statNameText.text = statName;
    }

    private void Start()
    {
        UpdateStatValue();
    }

    public void UpdateStatValue()
    {
        var playerStat = PlayerManager.Instance.player.GetComponent<Damageable>();
        if (playerStat)
        {
            statValueText.text = playerStat.StatsOfType(statType).GetValue().ToString();
        }
    }
}