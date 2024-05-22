using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ItemSlot : MonoBehaviour, IPointerClickHandler
{
    public InventoryItem item;

    [SerializeField] private Image itemImage;
    [SerializeField] private TextMeshProUGUI itemText;

    public void OnPointerClick(PointerEventData eventData)
    {
        if (item.data.itemType != ItemType.Equipment) return;
        Inventory.Instance.EquipItem(item.data);
    }

    public void UpdateSlot(InventoryItem newItem)
    {
        item = newItem;
        if (item != null)
        {
            itemImage.color = Color.white;
            itemImage.sprite = item.data.icon;
            var text = item.stackSize > 1 ? item.stackSize.ToString() : string.Empty;
            itemText.text = text;
        }
        else
        {
            itemImage.color = Color.clear;
            itemImage.sprite = null;
            itemText.text = string.Empty;
        }
    }
}