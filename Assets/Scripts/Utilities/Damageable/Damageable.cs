using UnityEngine;

public class Damageable : MonoBehaviour
{
    public void TakeDamage(GameObject attacked, int damage)
    {
        Debug.Log($"{attacked} take {damage} damage!");
    }
}
