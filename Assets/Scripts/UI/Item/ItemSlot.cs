using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemSlot : MonoBehaviour
{
    public InventoryItem item;

    [SerializeField] private Image itemImage;
    [SerializeField] private TextMeshProUGUI itemText;

    public void UpdateSlot(InventoryItem newItem)
    {
        item = newItem;

        itemImage.color = Color.white;

        if (item != null)
        {
            itemImage.sprite = item.data.icon;
            if (item.stackSize > 1)
            {
                itemText.text = item.stackSize.ToString();
            }
            else
            {
                itemText.text = string.Empty;
            }
        }
    }
}