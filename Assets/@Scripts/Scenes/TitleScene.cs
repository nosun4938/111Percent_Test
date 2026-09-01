using Unity.VisualScripting;
using UnityEngine;

public class TitleScene : BaseScene
{
    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        SceneType = Define.EScene.TitleScene;

        // Camera
        CameraController camera = Camera.main.GetOrAddComponent<CameraController>();

        return true;
    }

    public override void Clear()
    {

    }
}
