using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ItemSlot : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler, IPointerMoveHandler
{
    public InventoryItem item;

    [SerializeField] protected Image itemImage;
    [SerializeField] protected TextMeshProUGUI itemText;

    private UI ui;

    private void Start()
    {
        ui = GetComponentInParent<UI>();
    }

    public virtual void OnPointerClick(PointerEventData eventData)
    {
        if (item == null) return;
        if (item.data.itemType != ItemType.Equipment) return;
        Inventory.Instance.EquipItem(item.data);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (item == null) return;
        var equipment = item.data as ItemDataEquipment;
        if (equipment == null) return;
        ui.tooltip.ShowTooltip(equipment);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (item == null) return;
        ui.tooltip.HideTooltip();
    }

    public void OnPointerMove(PointerEventData eventData)
    {
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