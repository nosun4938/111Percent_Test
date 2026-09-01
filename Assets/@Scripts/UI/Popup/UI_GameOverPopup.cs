using UnityEngine;
using static Define;

public class UI_GameOverPopup : UI_Popup
{
    enum Texts
    {
        ScoreText,
    }

    enum GameObjects
    {
        CloseImage,
    }

    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        BindTexts(typeof(Texts));
        BindObjects(typeof(GameObjects));

        GetText((int)Texts.ScoreText).text = $"{Managers.Game.SaveData.Score}m";
        GetObject((int)GameObjects.CloseImage).BindEvent((evt) =>
        {
            Debug.Log("Data Saved");
            Managers.Game.SaveData.Score = 0;
            Managers.Game.SaveGame();

            Debug.Log("ChangeScene");
            Managers.UI.CloseAllPopupUI();
            Managers.Scene.LoadScene(EScene.TitleScene);
        });
        return true;
    }
}
