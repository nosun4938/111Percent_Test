using UnityEngine;
using UnityEngine.EventSystems;
using static Define;

public class UI_GameScene : UI_Scene
{
    enum Buttons
    {
        SkillAButton,
        SkillBButton,
        SkillCButton
    }

    enum Texts
    {
        LevelText,
        AttackPowerText,
        GoldCountText,
    }

    enum Images
    {
        HpBar,
    }

    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        BindButtons(typeof(Buttons));
        BindTexts(typeof(Texts));
        BindImages(typeof(Images));

        GetButton((int)Buttons.SkillAButton).gameObject.BindEvent(OnClickSkillAButton);
        GetButton((int)Buttons.SkillBButton).gameObject.BindEvent(OnClickSkillBButton);
        GetButton((int)Buttons.SkillCButton).gameObject.BindEvent(OnClickSkillCButton);

        Refresh();

        Managers.Game.OnBroadcastEvent -= OnBroadcastEventHandler;
        Managers.Game.OnBroadcastEvent += OnBroadcastEventHandler;

        return true;
    }

    private void Update()
    {
        
    }

    private void OnDisable()
    {
        Managers.Game.OnBroadcastEvent -= OnBroadcastEventHandler;
    }

    public void SetInfo()
    {
        Refresh();
    }

    void Refresh()
    {
        RefreshImages();
        RefreshAttackPowerText();
        RefreshGoldText();
        RefreshScoreText();
    }

    void OnBroadcastEventHandler(EBroadcastEventType eventType, float hp)
    {
        switch (eventType)
        {
            case EBroadcastEventType.ChangeHp:
                GetImage((int)Images.HpBar).rectTransform.localScale = new Vector3(Managers.Game.HpRatio, 1, 1);
                break;
            case EBroadcastEventType.ChangeGold:
                RefreshGoldText();
                break;
            case EBroadcastEventType.ScoreUp:
                RefreshScoreText();
                break;
        }
    }

    void OnClickSkillAButton(PointerEventData evt)
    {
        Managers.Game.SkillSlot = (Define.ESkillSlot.A);
    }
    void OnClickSkillBButton(PointerEventData evt)
    {
        Managers.Game.SkillSlot = (Define.ESkillSlot.B);
    }
    void OnClickSkillCButton(PointerEventData evt)
    {
        Managers.Game.SkillSlot = (Define.ESkillSlot.C);
    }

    void RefreshImages()
    {
        GetImage((int)Images.HpBar).rectTransform.localScale = Vector3.one;
    }

    public void RefreshAttackPowerText()
    {
        GetText((int)Texts.AttackPowerText).text = $"50";
    }

    public void RefreshGoldText()
    {
        GetText((int)Texts.GoldCountText).text = Managers.Game.Gold.ToString();
    }

    public void RefreshScoreText()
    {
        GetText((int)Texts.LevelText).text = Managers.Game.Score.ToString();
    }
}