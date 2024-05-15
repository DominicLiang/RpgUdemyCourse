using System;
using UnityEngine;

public class Damageable : MonoBehaviour
{
    public event Action<GameObject, GameObject> onTakeDamage;

    public void TakeDamage(GameObject from, GameObject to, int damage)
    {
        Debug.Log($"{to} take {damage} damage!");
        onTakeDamage?.Invoke(from, to);
    }
}
