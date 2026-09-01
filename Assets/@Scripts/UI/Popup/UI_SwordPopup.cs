using Newtonsoft.Json.Bson;
using UnityEngine;
using UnityEngine.EventSystems;

public class UI_SwordPopup : UI_Popup
{
    enum Buttons
    {
        CloseButton,
    }

    enum GameObjects
    {
        Slot1,
        Slot2,
        Slot3,
        Slot4,
        Slot5,
        Slot6,
        Slot7,
        Slot8,
        Slot9,
        Slot10,
    }

    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        BindButtons(typeof(Buttons));
        BindObjects(typeof(GameObjects));

        for (int i = 0; i < 10; i++)
        {
            Transform slot = GetObject(i).transform;
            UI_SwordSubitem subItem = Managers.UI.MakeSubItem<UI_SwordSubitem>(slot);
            subItem.SetInfo(i + 1);
        }

        GetButton((int)Buttons.CloseButton).gameObject.BindEvent(OnClickCloseButton);
        return true;
    }

    void OnClickCloseButton(PointerEventData evt)
    {
        Managers.UI.ClosePopupUI(this);
    }
}
