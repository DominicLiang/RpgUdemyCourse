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
    //     renderer.material.SetInt("_Flash", 1);
    //     await Task.Delay((int)0.1f * 1000);
    //     renderer.material.SetInt("_Flash", 0);
    // });
    IEnumerator Flash()
    {
        render.material.SetInt("_Flash", 1);
        yield return new WaitForSeconds(flashTime);
        render.material.SetInt("_Flash", 0);
    }
}
