using Data;
using UnityEngine;
using static Define;

public class Item
{
    public ItemSaveData SaveData { get; set; }

    public int InstanceID
    {
        get { return SaveData.InstanceID; }
        set { SaveData.InstanceID = value;}
    }
    public int TemplateID
    {
        get { return SaveData.TemplateID; }
        set { SaveData.TemplateID = value;}
    }
    public int Count
    {
        get { return SaveData.Count; }
        set { SaveData.Count = value;}
    }
    public int EquipSlot
    {
        get { return SaveData.EquipSlot; }
        set { SaveData.EquipSlot = value;}
    }

    public Data.ItemData TemplateData
    {
        get
        {
            return Managers.Data.ItemDic[TemplateID];
        }
    }

    public EItemType ItemType { get; private set; }
    public Item(ItemSaveData saveData)
    {
        SaveData = saveData;
        ItemType = TemplateData.Type;
    }

    public virtual bool Init()
    {
        return true;
    }

    public static Item MakeItem(ItemSaveData itemInfo)
    {
        if (Managers.Data.ItemDic.TryGetValue(itemInfo.TemplateID, out ItemData itemData) == false)
            return null;

        Item item = null;

        switch (itemData.Type)
        {
            case EItemType.Sword:
                item = new Sword(itemInfo);
                break;
        }

        return item;
    }

    #region Helper
    public bool IsEquippable()
    {
        return GetEquipItemEquipSlot() != EEquipSlotType.None;
    }

    public EEquipSlotType GetEquipItemEquipSlot()
    {
        if (ItemType == EItemType.Sword)
            return EEquipSlotType.Weapon;

        return EEquipSlotType.None;
    }

    public bool IsEquippedItem()
    {
        return SaveData.EquipSlot > (int)EEquipSlotType.None && SaveData.EquipSlot < (int)EEquipSlotType.EquipMax;
    }

    public bool IsInInventory()
    {
        return SaveData.EquipSlot == (int)EEquipSlotType.Inventory;
    }
    #endregion
}

public class Sword : Item
{
    public int Damage { get; private set; }
    public int Price { get; private set; }
    protected Data.SwordData SwordData { get { return (Data.SwordData)TemplateData; } }

    public Sword(ItemSaveData saveData) : base(saveData)
    {
        Init();
    }

    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        if (TemplateData == null)
            return false;

        if (TemplateData.Type != EItemType.Sword)
            return false;

        SwordData data = (SwordData)TemplateData;
        Damage = data.Damage;
        Price = data.Price;

        return true;
    }
}
