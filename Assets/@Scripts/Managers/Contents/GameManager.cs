using Data;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using static Define;
using static UnityEngine.Rendering.DebugUI;
using Random = UnityEngine.Random;

public class GameManager
{

    #region Hero
    private Vector2 _moveDir;
	public Vector2 MoveDir
	{
		get { return _moveDir; }
		set
		{
			_moveDir = value;
			OnMoveDirChanged?.Invoke(value);
		}
	}

	private float _hp;
	public float HP
	{
		get { return _hp; }
		set
		{
			_hp = value;
			BroadcastEvent(EBroadcastEventType.ChangeHp, _hp);
		}
	}

	private EJoystickState _joystickState;
	public EJoystickState JoystickState
	{
		get { return _joystickState; }
		set
		{
			_joystickState = value;
			OnJoystickStateChanged?.Invoke(_joystickState);
		}
	}

	private ESkillSlot _skillSlot;
	public ESkillSlot SkillSlot
	{
		get { return _skillSlot; }
		set
		{
			_skillSlot = value;
			OnSkillSlotChanged?.Invoke(_skillSlot);
		}
	}

    public void BroadcastEvent(EBroadcastEventType eventType, float value)
    {
        OnBroadcastEvent?.Invoke(eventType, value);
    }
    #endregion

    #region Action
    public event Action<Vector2> OnMoveDirChanged;
	public event Action<EJoystickState> OnJoystickStateChanged;
	public event Action<ESkillSlot> OnSkillSlotChanged;
	public event Action<EBroadcastEventType, float> OnBroadcastEvent;
	#endregion
}
