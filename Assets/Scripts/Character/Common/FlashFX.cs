using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class FlashFX : MonoBehaviour
{
    public float flashTime = 0.1f;
    public List<Color> igniteColor;
    public List<Color> chillColor;
    public List<Color> shockColor;

    private SpriteRenderer sr;
    private Damageable damageable;

    private void Start()
    {
        sr = transform.GetComponentInChildren<SpriteRenderer>();

        damageable = transform.GetComponent<Damageable>();
        damageable.OnTakeDamage += (from, to) => UniTask.ToCoroutine(async () =>
       {
           sr.material.SetInt("_Flash", Convert.ToInt32(true));
           await UniTask.WaitForSeconds(5);
           sr.material.SetInt("_Flash", Convert.ToInt32(false));
       });
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
        sr.material.SetInt("_Flash", Convert.ToInt32(true));
        yield return new WaitForSeconds(5);
        sr.material.SetInt("_Flash", Convert.ToInt32(false));
    }

    public void RedBlink(bool isOn)
    {
        sr.material.SetInt("_Blink", Convert.ToInt32(isOn));
    }

    public void AlimentsFxFor(List<Color> colors, float seconds)
    {
        StartCoroutine(AlimentsFx(colors, seconds));
    }

    private IEnumerator AlimentsFx(List<Color> colors, float seconds)
    {
        var coroutine = StartCoroutine(RepeatingColorFx(colors));
        yield return new WaitForSeconds(seconds);
        StopCoroutine(coroutine);
        sr.color = Color.white;
    }

    private IEnumerator RepeatingColorFx(List<Color> colors)
    {
        while (true)
        {
            if (sr.color != colors[0])
            {
                sr.color = colors[0];
            }
            else
            {
                sr.color = colors[1];
            }
            yield return new WaitForSeconds(0.3f);
        }
    }
}
