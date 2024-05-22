using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public static Inventory Instance { get; private set; }

    public List<InventoryItem> equipmentItems;
    public Dictionary<ItemDataEquipment, InventoryItem> equipmentDic;

    public List<InventoryItem> inventoryItems;
    public Dictionary<ItemData, InventoryItem> inventoryDic;

    public List<InventoryItem> stashItems;
    public Dictionary<ItemData, InventoryItem> stashDic;

    [Header("UI")]
    [SerializeField] private Transform equipmentSlotParent;
    [SerializeField] private Transform inventorySlotParent;
    [SerializeField] private Transform stashSlotParent;

    private EquipmentSlot[] equipmentSlots;
    private ItemSlot[] inventoryItemSlots;
    private ItemSlot[] stashItemSlots;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    private void Start()
    {
        equipmentItems = new List<InventoryItem>();
        equipmentDic = new Dictionary<ItemDataEquipment, InventoryItem>();

        inventoryItems = new List<InventoryItem>();
        inventoryDic = new Dictionary<ItemData, InventoryItem>();

        stashItems = new List<InventoryItem>();
        stashDic = new Dictionary<ItemData, InventoryItem>();

        equipmentSlots = equipmentSlotParent.GetComponentsInChildren<EquipmentSlot>();
        inventoryItemSlots = inventorySlotParent.GetComponentsInChildren<ItemSlot>();
        stashItemSlots = stashSlotParent.GetComponentsInChildren<ItemSlot>();
    }

    public void EquipItem(ItemData item)
    {
        var newEquipment = item as ItemDataEquipment;
        var newItem = new InventoryItem(newEquipment);

        var old = equipmentDic.FirstOrDefault(x => x.Key.equipmentType == newEquipment.equipmentType).Key;

        if (old)
        {
            UnEquipmentMethod(old);
            AddItemMethod(inventoryItems, inventoryDic, old);
        }

        RemoveItemMethod(inventoryItems, inventoryDic, item);
        EquipmentMethod(newEquipment, newItem);

        UpdateSlotUI(inventoryItemSlots, inventoryItems);
        UpdateSlotUI(equipmentSlots, equipmentItems);
    }

    private void EquipmentMethod(ItemDataEquipment newEquipment, InventoryItem newItem)
    {
        equipmentItems.Add(newItem);
        equipmentDic.Add(newEquipment, newItem);
    }

    private void UnEquipmentMethod(ItemDataEquipment old)
    {
        if (equipmentDic.TryGetValue(old, out var oldValue))
        {
            equipmentItems.Remove(oldValue);
            equipmentDic.Remove(old);
        }
    }

    public void AddItem(ItemData item)
    {
        switch (item.itemType)
        {
            case ItemType.Material:
                AddItemMethod(stashItems, stashDic, item);
                UpdateSlotUI(stashItemSlots, stashItems);
                break;
            case ItemType.Equipment:
                AddItemMethod(inventoryItems, inventoryDic, item);
                UpdateSlotUI(inventoryItemSlots, inventoryItems);
                break;
            default:
                break;
        }
    }

    public void RemoveItem(ItemData item)
    {
        switch (item.itemType)
        {
            case ItemType.Material:
                RemoveItemMethod(stashItems, stashDic, item);
                UpdateSlotUI(stashItemSlots, stashItems);
                break;
            case ItemType.Equipment:
                RemoveItemMethod(inventoryItems, inventoryDic, item);
                UpdateSlotUI(inventoryItemSlots, inventoryItems);
                break;
            default:
                break;
        }
    }

    private void AddItemMethod(List<InventoryItem> items, Dictionary<ItemData, InventoryItem> itemDic, ItemData item)
    {
        if (itemDic.TryGetValue(item, out InventoryItem value))
        {
            value.AddStack();
        }
        else
        {
            var newItem = new InventoryItem(item);
            items.Add(newItem);
            itemDic.Add(item, newItem);
        }
    }

    private void RemoveItemMethod(List<InventoryItem> items, Dictionary<ItemData, InventoryItem> itemDic, ItemData item)
    {
        if (itemDic.TryGetValue(item, out InventoryItem value))
        {
            if (value.stackSize <= 1)
            {
                items.Remove(value);
                itemDic.Remove(item);
            }
            else
            {
                value.RemoveStack();
            }
        }
    }

    private void UpdateSlotUI(ItemSlot[] slots, List<InventoryItem> items)
    {
        for (int i = 0; i < slots.Length; i++)
        {
            var slot = slots[i] as EquipmentSlot;

            if (!slot)
            {
                if (i < items.Count)
                {
                    slots[i].UpdateSlot(items[i]);
                }
                else
                {
                    slots[i].UpdateSlot(null);
                }
            }
            else
            {
                for (int j = 0; j < items.Count; j++)
                {
                    var data = items[j].data as ItemDataEquipment;
                    if (!data || slot.slotType != data.equipmentType) continue;
                    slots[i].UpdateSlot(items[j]);
                }
            }
        }
    }
}