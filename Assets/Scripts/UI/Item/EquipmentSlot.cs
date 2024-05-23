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
        Inventory.Instance.UnEquipItem(item.data);
    }
}