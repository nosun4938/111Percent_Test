using UnityEngine;
using UnityEngine.EventSystems;
using static Define;

public class UI_SwordSubitem : UI_Base
{
    enum GameObjects
    {
        PriceArea,
        EquippedImage,
    }

    enum Buttons
    {
        EquipButton,
        BuyButton,
    }

    enum Images
    {
        PriceImage,
        SwordImage,
    }

    enum Texts
    {
        PriceText,
        AtkBonusText,
    }

    int _templateID = -1;
    Item _item;

    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        BindObjects(typeof(GameObjects));
        BindButtons(typeof(Buttons));
        BindTexts(typeof(Texts));
        BindImages(typeof(Images));

        GetButton((int)Buttons.EquipButton).gameObject.BindEvent(OnClickEquipButton);
        GetButton((int)Buttons.BuyButton).gameObject.BindEvent(OnClickBuyButton);

        GetObject((int)GameObjects.EquippedImage).SetActive(false);
        Refresh();

        return true;
    }

    public void SetInfo(int slotNumber)
    {
        _templateID = slotNumber;
        GetImage((int)Images.SwordImage).sprite = Managers.Resource.Load<Sprite>($"Sword_{_templateID}.sprite");
        GetText((int)Texts.AtkBonusText).text = $"Atk +{Managers.Data.SwordDic[_templateID].Damage}";
        GetText((int)Texts.PriceText).text = $"{Managers.Data.SwordDic[_templateID].Price}G";

        Refresh();
    }

    private void OnEnable()
    {
        Managers.Game.OnEEquippedItemChanged -= OnEEquippedEventHandler;
        Managers.Game.OnEEquippedItemChanged += OnEEquippedEventHandler;
    }

    private void OnDisable()
    {
        Managers.Game.OnEEquippedItemChanged -= OnEEquippedEventHandler;
    }

    public void Refresh()
    {
        if (_init == false)
            return;

        if (_templateID < 0)
            return;

        if (Managers.Inventory.HasItem(_templateID) == false)
            return;

        GetObject((int)GameObjects.PriceArea).SetActive(false);
        _item = Managers.Inventory.GetItemByTemplateId(_templateID);

        if (_item.EquipSlot == (int)EEquipSlotType.Weapon)
            GetObject((int)GameObjects.EquippedImage).SetActive(true);
        else
            GetObject((int)GameObjects.EquippedImage).SetActive(false);
    }

    #region Event
    void OnClickEquipButton(PointerEventData evt)
    {
        if (_item  == null)
        {
            Debug.Log("아이템 없음");
            return;
        }
        Managers.Inventory.EquipItem(_item.InstanceID);
        Refresh();
    }

    void OnClickBuyButton(PointerEventData evt)
    {
        int price = Managers.Data.SwordDic[_templateID].Price;
        if (Managers.Game.Gold < price)
        {
            Debug.Log("골드 부족");
            return;
        }

        Managers.Game.EarnGold(-price);
        _item = Managers.Inventory.MakeItem(_templateID);
        Debug.Log("구매 성공");

        Managers.Game.SaveGame();

        GetObject((int)GameObjects.PriceArea).SetActive(false);
    }

    void OnEEquippedEventHandler(EEquipSlotType slot)
    {
        if (slot == EEquipSlotType.Weapon)
        {
            Managers.Game.ChangeWeapon();
            Refresh();
        }
    }
    #endregion
}
