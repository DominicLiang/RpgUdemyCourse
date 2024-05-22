using System;

[Serializable]
public class InventoryItem
{
    public int stackSize;
    public ItemData data;

    public InventoryItem(ItemData data)
    {
        this.data = data;
        AddStack();
    }

    public void AddStack()
    {
        stackSize++;
    }

    public void RemoveStack()
    {
        stackSize--;
    }
}