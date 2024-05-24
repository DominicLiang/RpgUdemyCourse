using UnityEngine;

public class ItemObject : MonoBehaviour
{
    [SerializeField] private ItemData itemData;

    private void OnValidate()
    {
        if (!itemData) return;
        GetComponent<SpriteRenderer>().sprite = itemData.icon;
        gameObject.name = itemData.name;
    }

    private Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public void Setup(ItemData item, Vector2 velocity)
    {
        itemData = item;
        GetComponent<Rigidbody2D>().velocity = velocity;
        GetComponent<SpriteRenderer>().sprite = item.icon;
        gameObject.name = item.name;
    }

    public void PickupItem()
    {
        if (!Inventory.Instance.CanAddItem())
        {
            rb.velocity = new Vector2(0, 7);
            return;
        }
        Inventory.Instance.AddItem(itemData);
        Destroy(gameObject);
    }
}