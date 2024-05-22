public class EquipmentSlot : ItemSlot
{
    public EquipmentType slotType;

    private void OnValidate()
    {
        gameObject.name = $"SlotType - {slotType}";
    }
}