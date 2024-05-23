using UnityEngine;

public class ItemTrigger : MonoBehaviour
{
    private ItemObject itemObject;

    private void Awake()
    {
        itemObject = GetComponentInParent<ItemObject>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            itemObject.PickupItem();
        }
    }
}