using System;
using UnityEngine;
using UnityEngine.UI;

public class SmallHpBar : MonoBehaviour
{
    private Slider slider;
    private RectTransform rectTransform;
    private FlipSprite flipSprite;
    private Damageable damageable;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        flipSprite = GetComponentInParent<FlipSprite>();
        slider = GetComponentInChildren<Slider>();
        damageable = GetComponentInParent<Damageable>();
    }

    private void OnEnable()
    {
        flipSprite.OnFlip += Flip;
        damageable.OnTakeDamage += UpdateHpBar;
    }

    private void OnDisable()
    {
        flipSprite.OnFlip -= Flip;
        damageable.OnTakeDamage -= UpdateHpBar;
    }

    private void UpdateHpBar(GameObject object1, GameObject object2)
    {
        slider.maxValue = damageable.MaxHp.GetValue();
        slider.value = damageable.currentHp;
    }

    private void Flip()
    {
        rectTransform.Rotate(0, 180, 0);
    }
}