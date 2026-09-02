using Newtonsoft.Json.Bson;
using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using static Define;

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

    enum Texts
    {
        GoldText,
    }

    List<UI_SwordSubitem> subitems = new List<UI_SwordSubitem>();
    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        BindButtons(typeof(Buttons));
        BindObjects(typeof(GameObjects));
        BindTexts(typeof(Texts));

        for (int i = 0; i < 10; i++)
        {
            Transform slot = GetObject(i).transform;
            UI_SwordSubitem subItem = Managers.UI.MakeSubItem<UI_SwordSubitem>(slot);
            subitems.Add(subItem);
            subItem.SetInfo(i + 1);
        }

        GetButton((int)Buttons.CloseButton).gameObject.BindEvent(OnClickCloseButton);

        

        return true;
    }

    public void OnEnable()
    {
        Managers.Game.OnBroadcastEvent -= OnBroadcastEventHandler;
        Managers.Game.OnBroadcastEvent += OnBroadcastEventHandler;
        
        Refresh();

        // UI Sword Subitem
        for (int i = 0; i < subitems.Count; i++)
            subitems[i].Refresh();
    }

    private void OnDisable()
    {
        Managers.Game.OnBroadcastEvent -= OnBroadcastEventHandler;
    }

    void Refresh()
    {
        RefreshGoldText();
    }

    void OnClickCloseButton(PointerEventData evt)
    {
        Managers.UI.ClosePopupUI(this);
    }

    void OnBroadcastEventHandler(EBroadcastEventType eventType, float hp)
    {
        switch (eventType)
        {
            case EBroadcastEventType.ChangeGold:
                RefreshGoldText();
                break;
        }
    }
    public void RefreshGoldText()
    {
        GetText((int)Texts.GoldText).text = $"{Managers.Game.Gold.ToString()}G";
    }
}
