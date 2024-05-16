using System;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Damageable))]
public class FlashFX : MonoBehaviour
{
    public float flashTime = 0.1f;

    private SpriteRenderer render;
    private Damageable damageable;

    private void Start()
    {
        render = transform.GetComponentInChildren<SpriteRenderer>();

        damageable = transform.GetComponent<Damageable>();
        damageable.onTakeDamage += (from, to) => StartCoroutine(Flash());
    }

    // 大坑，一样的东西，用task就是无效，修改材质一定要用协程
    // Task.Run(async () =>
    // {
    //     render.material.SetInt("_Flash", 1);
    //     await Task.Delay((int)0.1f * 1000);
    //     render.material.SetInt("_Flash", 0);
    // });
    IEnumerator Flash()
    {
        render.material.SetColor("_Color", Color.white);
        render.material.SetInt("_Flash", Convert.ToInt32(true));
        yield return new WaitForSeconds(flashTime);
        render.material.SetInt("_Flash", Convert.ToInt32(false));
    }

    public void RedBlink()
    {
        render.material.SetColor("_Color", Color.red);
        var boolen = Convert.ToBoolean(render.material.GetInt("_Flash"));
        render.material.SetInt("_Flash", Convert.ToInt32(!boolen));
    }

    public void Reset()
    {
        CancelInvoke(nameof(RedBlink));
        render.material.SetInt("_Flash", Convert.ToInt32(false));
    }
}
