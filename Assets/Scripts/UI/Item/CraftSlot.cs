using UnityEngine.EventSystems;

public class CraftSlot : ItemSlot
{
    private void OnEnable()
    {
        UpdateSlot(item);
    }

    public override void OnPointerClick(PointerEventData eventData)
    {
        var craftData = item.data as ItemDataEquipment;
        Inventory.Instance.CanCraft(craftData, craftData.craftingMaterials);
    }
}