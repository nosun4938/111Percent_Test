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

        CriRate = HeroData.CriRate;
        CriDamage = HeroData.CriDamage;
    }

    private void Update()
    {
		if (Managers.Map == null)
            return;

		switch (CreatureState)
		{
			case ECreatureState.Idle:
				UpdateIdle();
				break;
			case ECreatureState.Skill:
				UpdateSkill();
				break;
            case ECreatureState.Dead:
                UpdateDead();
                break;
		}

        // Map Transition
        Managers.Map.StageTransition.CheckMapChanged(CellPos);
    }

    #region State Machine
    private void UpdateIdle()
    {
		EFindPathResult result = MoveOneCellToward(_moveDir);
		
        if (result != EFindPathResult.Fail_Monster)
			return;

		if (Target.IsValid() == false)
			return;

		CreatureState = ECreatureState.Skill;
		// Skill 사용
    }

	private void UpdateSkill()
	{
        if (_coWait != null)
            return;

        if (Target.IsValid() == false)
        {
            CreatureState = ECreatureState.Idle;
            return;
        }

        // DoSkill
        Skills.DefaultSkill.DoSkill();
        LookAtTarget(Target);

        float delay = Skills.DefaultSkill.SkillData.Duration;
        StartWait(delay);
    }

    private void UpdateDead()
    {
        if (LerpCellPosCompleted)
        {
            Hp = MaxHp;
            CreatureState = ECreatureState.Idle;
        }
    }
    #endregion

    #region Battle
    public override void OnDamaged(BaseObject attacker, SkillBase skill)
    {
        base.OnDamaged(attacker, skill);
    }

    public override void OnDead(BaseObject attacker, SkillBase skill)
    {
        base.OnDead(attacker, skill);

        CreatureState = ECreatureState.Dead;
        Managers.Map.MoveTo(this, new Vector3Int(0, 1, 0));
    }
    #endregion

    #region Event Handler
    private void HandleOnMoveDirChanged(Vector2 dir)
	{
        _moveDir = dir;
    }

    private void OnDisable()
    {
        Managers.Game.OnMoveDirChanged -= HandleOnMoveDirChanged;
    }
	#endregion

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
            case ECreatureState.Dead:
                PlayAnimation(AnimName.IDLE);
                break;
            default:
                break;
        }
    }
    #endregion
    
    #region Wait
    protected Coroutine _coWait;

    protected void StartWait(float seconds)
    {
        CancelWait();
        _coWait = StartCoroutine(CoWait(seconds));
    }

    IEnumerator CoWait(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        _coWait = null;
    }

    protected void CancelWait()
    {
        if (_coWait != null)
            StopCoroutine(_coWait);
        _coWait = null;
    }
    #endregion
}
