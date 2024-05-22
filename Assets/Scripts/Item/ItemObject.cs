using UnityEngine;

public class ItemObject : MonoBehaviour
{
    [SerializeField] private ItemData itemData;

    private void OnValidate()
    {
        GetComponent<SpriteRenderer>().sprite = itemData.icon;
        gameObject.name = itemData.name;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Inventory.Instance.AddItem(itemData);
            Destroy(gameObject);
        }
    }
}