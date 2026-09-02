using Unity.VisualScripting;
using UnityEngine;
using static Define;

public class TitleScene : BaseScene
{
    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        SceneType = Define.EScene.TitleScene;

        // Sound
        Managers.Sound.Init();
        Managers.Sound.Play(ESound.Bgm, "TitleScene", pitch: 0.1f);

        return true;
    }

    public override void Clear()
    {

    }
}
