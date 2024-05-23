using UnityEngine;

public class ItemObject : MonoBehaviour
{
    private ItemData itemData;

    public ItemData ItemData
    {
        get
        {
            return itemData;
        }
        set
        {
            itemData = value;
            GetComponent<SpriteRenderer>().sprite = value.icon;
            gameObject.name = value.name;
        }
    }

    private Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public void Setup(ItemData item, Vector2 velocity)
    {
        ItemData = item;
        GetComponent<Rigidbody2D>().velocity = velocity;
    }

    public void PickupItem()
    {
        Inventory.Instance.AddItem(ItemData);
        Destroy(gameObject);
    }
}