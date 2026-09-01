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

        Managers.Game.OnBroadcastEvent -= OnHpBarChanged;
        Managers.Game.OnBroadcastEvent += OnHpBarChanged;

        return true;
    }

    private void Update()
    {
        
    }

    private void OnDisable()
    {
        Managers.Game.OnBroadcastEvent -= OnHpBarChanged;
    }

    public void SetInfo()
    {
        Refresh();
    }

    void Refresh()
    {
        RefreshImages();
        RefreshTexts();
    }

    void OnHpBarChanged(EBroadcastEventType eventType, float hp)
    {
        // 다른 방법 생각 필요
        if (eventType == EBroadcastEventType.ChangeHp)
            GetImage((int)Images.HpBar).rectTransform.localScale = Managers.Object.Player._hpRatio;
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

    void RefreshTexts()
    {
        GetText((int)Texts.AttackPowerText).text = $"{Managers.Object.Player.Atk}";
        GetText((int)Texts.GoldCountText).text = $"100";
        GetText((int)Texts.LevelText).text = $"4938";
    }
}