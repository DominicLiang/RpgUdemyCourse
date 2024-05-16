using System;
using Cinemachine;
using UnityEngine;

public class Damageable : MonoBehaviour
{
    public event Action<GameObject, GameObject> onTakeDamage;

    public void TakeDamage(GameObject from, GameObject to, int damage)
    {
        Debug.Log($"{to} take {damage} damage!");
        onTakeDamage?.Invoke(from, to);

        AttackSense.Instance.HitPause(0.1f);
        AttackSense.Instance.GetComponent<CinemachineImpulseSource>().GenerateImpulse();
    }
}
