using UnityEngine;
using UnityEngine.EventSystems;

public class UI_SwordSubitem : UI_Base
{
    enum Buttons
    {
        EquipButton,
    }

    enum Images
    {
        SwordImage,
    }

    enum Texts
    {
        EquipText,
        AtkBonusText,
    }

    int _templateID = -1;
    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        BindButtons(typeof(Buttons));
        BindTexts(typeof(Texts));
        BindImages(typeof(Images));

        GetButton((int)Buttons.EquipButton).gameObject.BindEvent(OnClickEquipButton);

        Refresh();

        return true;
    }

    public void SetInfo(int slotNumber)
    {
        _templateID = slotNumber;

        Refresh();
    }

    void Refresh()
    {
        if (_init == false)
            return;

        if (_templateID < 0)
            return;

        if (Managers.Inventory.HasItem(_templateID) == false)
        {
            Debug.Log($"Don't Have Item {_templateID}");
            gameObject.SetActive(false);
            return;
        }

        gameObject.SetActive(true);
        GetImage((int)Images.SwordImage).sprite = Managers.Resource.Load<Sprite>($"Sword_{_templateID}.sprite");
        GetText((int)Texts.AtkBonusText).text = $"Atk +{Managers.Data.SwordDic[_templateID].Damage}";
    }

    void OnClickEquipButton(PointerEventData evt)
    {
        Item item = Managers.Inventory.GetItemByTemplateId(_templateID);
        if (item  == null)
        {
            Debug.Log("No Item");
            return;
        }
        Managers.Inventory.EquipItem(item.InstanceID);
    }
}
