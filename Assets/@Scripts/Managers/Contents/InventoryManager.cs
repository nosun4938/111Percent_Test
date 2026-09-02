using Data;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static Define;

public class InventoryManager
{
    
    int DEFAULT_INVENTORY_SLOT_COUNT = 30;
    public List<Item> AllItems { get; } = new List<Item>();

    // Cache
    Dictionary<int, Item> EquippedItems = new Dictionary<int, Item>();
    List<Item> InventoryItems = new List<Item>();

    public Item MakeItem(int itemTemplateID, int count = 1)
    {
        int itemDbID = Managers.Game.GenerateItemDbId();

        if (Managers.Data.ItemDic.TryGetValue(itemTemplateID, out ItemData itemData) == false)
            return null;

        ItemSaveData saveData = new ItemSaveData()
        {
            InstanceID = itemDbID,
            TemplateID = itemTemplateID,
            Count = count,
            EquipSlot = (int)EEquipSlotType.Inventory,
        };

        return AddItem(saveData);
    }

    public Item AddItem(ItemSaveData itemInfo)
    {
        Item item = Item.MakeItem(itemInfo);
        if (item == null)
            return null;

        if (item.IsEquippedItem())
        {
            EquippedItems.Add(item.SaveData.EquipSlot, item);
        }
        else if (item.IsInInventory())
        {
            InventoryItems.Add(item);
        }

        AllItems.Add(item);
        return item;
    }

    public void EquipItem(int instanceID)
    {
        Item item = InventoryItems.Find(x => x.SaveData.InstanceID == instanceID);
        if (item == null)
        {
            Debug.Log("Item 없음");
            return;
        }

        EEquipSlotType equipSlotType = item.GetEquipItemEquipSlot();
        if (equipSlotType == EEquipSlotType.None)
            return;

        // 기존 아이템 해제
        if (EquippedItems.TryGetValue((int)equipSlotType, out Item prev))
        {
            EquippedItems.Remove((int)equipSlotType);
            prev.EquipSlot = (int)EEquipSlotType.Inventory;
            InventoryItems.Add(prev);
        }

        // 아이템 장착
        InventoryItems.Remove(item);
        item.EquipSlot = (int)equipSlotType;
        EquippedItems[(int)equipSlotType] = item;

        Managers.Game.EquippedItemChange(equipSlotType);
    }

    public void UnEquipItem(int instanceID, bool checkFull = true)
    {
        Item item = EquippedItems.Values.Where(x => x.InstanceID == instanceID).FirstOrDefault();
        if (item == null)
            return;

        if (checkFull && IsInventoryFull())
            return;

        EquippedItems.Remove((int)item.EquipSlot);

        item.EquipSlot = (int)EEquipSlotType.Inventory;
        InventoryItems.Add(item);
    }

    public void Clear()
    {
        AllItems.Clear();
        EquippedItems.Clear();
        InventoryItems.Clear();
    }

    #region Helper
    public Item GetItem(int instanceId)
    {
        return AllItems.Find(item => item.InstanceID == instanceId);
    }
    public bool HasItem(int templateId)
    {
        return AllItems.Exists(item => item.TemplateID == templateId);
    }
    public Item GetItemByTemplateId(int templateId)
    {
        return AllItems.Find(item => item.TemplateID == templateId);
    }

    public Item GetEquippedItem(EEquipSlotType equipSlotType)
    {
        EquippedItems.TryGetValue((int)equipSlotType, out Item item);

        return item;
    }

    public Item GetEquippedItem(int instanceId)
    {
        return EquippedItems.Values.Where(x => x.InstanceID == instanceId).FirstOrDefault();
    }

    public Item GetItemInInventory(int instanceId)
    {
        return InventoryItems.Find(x => x.SaveData.InstanceID == instanceId);
    }

    public bool IsInventoryFull()
    {
        return InventoryItems.Count >= InventorySlotCount();
    }

    public int InventorySlotCount()
    {
        return DEFAULT_INVENTORY_SLOT_COUNT;
    }
    #endregion
}
