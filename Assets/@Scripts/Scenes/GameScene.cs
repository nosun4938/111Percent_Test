using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static Define;

public class GameScene : BaseScene
{
	public override bool Init()
	{
		if (base.Init() == false)
			return false;

		SceneType = EScene.GameScene;
		Managers.Map.LoadMap("TestMap");
        Managers.Map.StageTransition.SetInfo();

        // Hero
        Hero player = Managers.Object.Spawn<Hero>(new Vector3Int(0, 0, 0), 200001);
		Managers.Map.MoveTo(player, new Vector3Int(0, 1, 0), true);

		// JoyStick
		Managers.UI.ShowBaseUI<UI_Joystick>();

        // Camera
        CameraController camera = Camera.main.GetOrAddComponent<CameraController>();
        camera.Target = player;
        return true;
	}

	public override void Clear()
	{

	}
}
