using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using static Define;

public class GameScene : BaseScene
{
    public override bool Init()
	{
		if (base.Init() == false)
			return false;

		SceneType = EScene.GameScene;

        // Map
		Managers.Map.LoadMap("TestMap");
        Managers.Map.StageTransition.SetInfo();

        // Hero
        Hero player = Managers.Object.Spawn<Hero>(new Vector3Int(0, 0, 0), 200001);
		Managers.Map.MoveTo(player, new Vector3Int(0, 1, 0), true);

        // Scene UI
        UI_GameScene gameSceneUI = Managers.UI.ShowSceneUI<UI_GameScene>();

        // JoyStick
        UI_Joystick joystickUI = Managers.UI.ShowBaseUI<UI_Joystick>();

        // Camera
        CameraController camera = Camera.main.GetOrAddComponent<CameraController>();
        camera.Target = player;

        // Sound
        Managers.Sound.Init();
        Managers.Sound.Play(ESound.Bgm, "GameScene", pitch: 0.1f);

        return true;
    }

	public override void Clear()
	{
        
    }
}
