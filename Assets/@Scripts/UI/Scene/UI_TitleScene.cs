using UnityEngine;
using UnityEngine.EventSystems;
using static Define;

public class UI_TitleScene : UI_Scene
{
    enum Buttons
    {
        GameStartButton,
        SwordsButton
    }

    enum Texts
    {
        ScoreText
    }

    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        BindButtons(typeof(Buttons));
        BindTexts(typeof(Texts));

        GetButton((int)Buttons.GameStartButton).gameObject.BindEvent(OnClickGameStartButton);
        GetButton((int)Buttons.SwordsButton).gameObject.BindEvent(OnClickSwordButton);

        GetText((int)Texts.ScoreText).text = $"{Managers.Game.SaveData.HighScore}m";

        Refresh();

        return true;
    }

    void Refresh()
    {
        if (_init == false)
            return;

        gameObject.SetActive(true);
        GetText((int)Texts.ScoreText).text = $"{Managers.Game.SaveData.HighScore}m";
    }

    void OnClickGameStartButton(PointerEventData evt)
    {
        Debug.Log("ChangeScene");
        Managers.Scene.LoadScene(EScene.GameScene);
        //Managers.Game.GameStart();
    }

    void OnClickSwordButton(PointerEventData evt)
    {
        UI_SwordPopup popup = Managers.UI.ShowPopupUI<UI_SwordPopup>();
    }

    
}
