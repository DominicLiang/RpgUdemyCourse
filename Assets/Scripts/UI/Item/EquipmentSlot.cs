using UnityEngine.EventSystems;

public class EquipmentSlot : ItemSlot
{
    public EquipmentType slotType;

    private void OnValidate()
    {
        gameObject.name = $"SlotType - {slotType}";
    }

    public override void OnPointerClick(PointerEventData eventData)
    {
        if (item == null || item.data == null) return;
        Inventory.Instance.UnEquipItem(item.data);
    }
}