using Data;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;
using static Define;

public class Hero : Creature
{
	public Data.HeroData HeroData { get; set; }

    Vector2 _moveDir = Vector2.zero;
    int lastY = -1;
    float _hpRatio;
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

    ESkillSlot _heroSkillSlot = ESkillSlot.Default;
    public ESkillSlot HeroSkillSlot
    {
        get { return _heroSkillSlot; }
        set { _heroSkillSlot = value;}
    }

	public override bool Init()
	{
		if (base.Init() == false)
			return false;

		ObjectType = EObjectType.Hero;

		Managers.Game.OnMoveDirChanged -= HandleOnMoveDirChanged;
        Managers.Game.OnMoveDirChanged += HandleOnMoveDirChanged;

        Managers.Game.OnSkillSlotChanged -= HandleOnSkillSlotChanged;
        Managers.Game.OnSkillSlotChanged += HandleOnSkillSlotChanged;

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
        EnterIdle();

        CriRate = HeroData.CriRate;
        CriDamage = HeroData.CriDamage;

        // Temp
        ModifyHp(MaxHp);
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
                //UpdateDead();
                break;
		}

        // Score
        if (lastY > CellPos.y)
            Managers.Game.EarnScore(lastY - CellPos.y);
        lastY = CellPos.y;

        // Map Transition
        Managers.Map.StageTransition.CheckMapChanged(CellPos);
    }

    #region State Machine
    private void EnterIdle()
    {
        CreatureState = ECreatureState.Idle;
        HeroSkillSlot = ESkillSlot.Default;
    }
    private void UpdateIdle()
    {
		EFindPathResult result = MoveOneCellToward(_moveDir);
		
        if (result != EFindPathResult.Fail_Monster)
			return;

		if (Target.IsValid() == false)
			return;
        
        // Skill 사용
        EnterSkill();
    }

    private void EnterSkill()
    {
        CreatureState = ECreatureState.Skill;
        
    }

	private void UpdateSkill()
	{
        if (_coWait != null)
            return;

        switch (HeroSkillSlot)
        {
            case ESkillSlot.A:
                DoASkill();
                break;
            case ESkillSlot.B:
                DoBSkill();
                break;
            case ESkillSlot.C:
                DoCSkill();
                break;
            default:
                DoDefaultSkill();
                break;
        }
    }

    private void DoASkill()
    {
        Skills.ASkill.DoSkill();
        SetCellPos(TargetCellPos, forceMove: true);
        
        float delay = Skills.DefaultSkill.SkillData.Duration;
        StartWait(delay);
    }
    private void DoBSkill()
    {
        Skills.BSkill.DoSkill();

        float delay = Skills.DefaultSkill.SkillData.Duration;
        StartWait(delay);

        EnterIdle();
    }
    private void DoCSkill()
    {
        Skills.CSkill.DoSkill();

        float delay = Skills.DefaultSkill.SkillData.Duration;
        StartWait(delay);

        EnterIdle();
    }
    private void DoDefaultSkill()
    {
        if (Target.IsValid() == false)
        {
            Managers.Map.MoveTo(this, TargetCellPos);
            TargetCellPos = default;
            CancelWait();
            return;
        }

        Skills.DefaultSkill.DoSkill();
        LookAtTarget(Target);

        float delay = Skills.DefaultSkill.SkillData.Duration;
        StartWait(delay);
    }

    private void EnterDead()
    {
        CreatureState = ECreatureState.Dead;
        Managers.UI.ShowPopupUI<UI_GameOverPopup>();
    }
    #endregion

    #region Battle
    public override void OnDamaged(BaseObject attacker, SkillBase skill)
    {
        base.OnDamaged(attacker, skill);
        Creature creature = attacker as Creature;
        if (creature == null)
            return;

        float finalDamage = creature.Atk * skill.SkillData.DamageMultiplier;
        ModifyHp(Hp = Mathf.Clamp(Hp - finalDamage, 0, MaxHp));

        Managers.Object.ShowDamageFont(transform.position, finalDamage, transform, false);

        if (Hp <= 0)
        {
            OnDead(attacker, skill);
            return;
        }
    }

    public override void OnDead(BaseObject attacker, SkillBase skill)
    {
        base.OnDead(attacker, skill);

        Managers.Map.MoveTo(this, new Vector3Int(0, 1, 0));
        EnterDead();
    }
    #endregion

    #region Event Handler
    private void HandleOnMoveDirChanged(Vector2 dir)
	{
        _moveDir = dir;
    }

    private void HandleOnSkillSlotChanged(ESkillSlot skillSlot)
    {
        // 쿨타임 체크
        if (Skills.ActiveSkills.Contains(SlotToSkillBase(skillSlot)) == false)
        {
            Debug.Log($"{skillSlot} is on CoolDown");
            return;
        }
        HeroSkillSlot = skillSlot;
        EnterSkill();
    }
    private SkillBase SlotToSkillBase(ESkillSlot skillSlot)
    {
        switch (skillSlot)
        {
            case ESkillSlot.A:
                return Skills.ASkill;
            case ESkillSlot.B:
                return Skills.BSkill;
            case ESkillSlot.C:
                return Skills.CSkill;
            default:
                return Skills.DefaultSkill;
        }
    }
    private void OnDisable()
    {
        Managers.Game.OnMoveDirChanged -= HandleOnMoveDirChanged;
        Managers.Game.OnSkillSlotChanged -= HandleOnSkillSlotChanged;
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
        EnterIdle();
        _coWait = null;
    }

    protected void CancelWait()
    {
        if (_coWait != null)
            StopCoroutine(_coWait);
        _coWait = null;
    }
    #endregion

    #region Temp
    public void ModifyHp(float hp)
    {
        Hp = hp;
        _hpRatio = Hp / MaxHp;
        Managers.Game.HpRatio = _hpRatio;
    }
    #endregion
}
