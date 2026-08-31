using Data;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using static Define;

public class Hero : Creature
{
	public Data.HeroData HeroData { get; set; }
    public SkillComponent Skills { get; protected set; }

    Vector2 _moveDir = Vector2.zero;
	public override ECreatureState CreatureState
	{
		get { return _creatureState; }
		set
		{
			if (_creatureState != value)
			{
				base.CreatureState = value;
                UpdateAnimation();
            }
		}
	}

	EHeroMoveState _heroMoveState = EHeroMoveState.None;
	public EHeroMoveState HeroMoveState
	{
		get { return _heroMoveState; }
		private set
		{
			_heroMoveState = value;
		}
	}

	public override bool Init()
	{
		if (base.Init() == false)
			return false;

		ObjectType = EObjectType.Hero;

		Managers.Game.OnMoveDirChanged -= HandleOnMoveDirChanged;
        Managers.Game.OnMoveDirChanged += HandleOnMoveDirChanged;
        Managers.Game.OnJoystickStateChanged -= HandleOnJoystickStateChanged;
		Managers.Game.OnJoystickStateChanged += HandleOnJoystickStateChanged;

		// Map
		Collider.isTrigger = true;
		RigidBody.simulated = false;

		return true;
	}

	public override void SetInfo(int templateID)
	{
		base.SetInfo(templateID);
        HeroData = CreatureData as HeroData;

        // State
        CreatureState = ECreatureState.Idle;

        // Skills
        Skills = gameObject.GetOrAddComponent<SkillComponent>();
        Skills.SetInfo(this, HeroData);

        CriRate = new CreatureStat(HeroData.CriRate);
        CriDamage = new CreatureStat(HeroData.CriDamage);
    }

    private void Update()
    {
		if (Managers.Map == null)
            return;

		EFindPathResult result = MoveOneCellToward(_moveDir);
		if (result == EFindPathResult.Monster)
			return;

        // Map Transition
        Managers.Map.StageTransition.CheckMapChanged(CellPos);
    }

    private void HandleOnMoveDirChanged(Vector2 dir)
	{
        _moveDir = dir;
    }

	private void HandleOnJoystickStateChanged(EJoystickState joystickState)
	{
		switch (joystickState)
		{
			case Define.EJoystickState.PointerDown:
				HeroMoveState = EHeroMoveState.ForceMove;
				break;
			case Define.EJoystickState.Drag:
				HeroMoveState = EHeroMoveState.ForceMove;
				break;
			case Define.EJoystickState.PointerUp:
				HeroMoveState = EHeroMoveState.None;
				break;
			default:
				break;
		}
	}

    private void OnDisable()
    {
        Managers.Game.OnMoveDirChanged -= HandleOnMoveDirChanged;
        Managers.Game.OnJoystickStateChanged -= HandleOnJoystickStateChanged;
    }

    #region UpdateAnimation
    protected override void UpdateAnimation()
    {
        switch (CreatureState)
        {
            case ECreatureState.Idle:
                PlayAnimation(AnimName.IDLE);
                break;
            case ECreatureState.Skill:

                break;
            default:
                break;
        }
    }
    #endregion
}
