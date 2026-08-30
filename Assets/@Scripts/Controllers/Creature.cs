using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using static Define;

public class Creature : BaseObject
{
	public BaseObject Target { get; protected set; }
	public SkillComponent Skills { get; protected set; }

	public Data.CreatureData CreatureData { get; private set; }

	//public EffectComponent Effects { get; set; }

	float DistToTargetSqr
	{
		get
		{
			Vector3 dir = (Target.transform.position - transform.position);
			float distToTarget = Math.Max(0, dir.magnitude - Target.ExtraCells * 1f - ExtraCells * 1f); // TEMP
			return distToTarget * distToTarget;
		}
	}

	#region Stats
	public float Hp { get; set; }
	public CreatureStat MaxHp;
	public CreatureStat Atk;
	public CreatureStat CriRate;
	public CreatureStat CriDamage;
	public CreatureStat ReduceDamageRate;
	public CreatureStat LifeStealRate;
	public CreatureStat ThornsDamageRate; // 쏜즈
	public CreatureStat MoveSpeed;
	public CreatureStat AttackSpeedRate;
	#endregion

	protected ECreatureState _creatureState = ECreatureState.None;
	public virtual ECreatureState CreatureState
	{
		get { return _creatureState; }
		set
		{
			if (_creatureState != value)
			{
				_creatureState = value;
				//UpdateAnimation();
			}
		}
	}

	public override bool Init()
	{
		if (base.Init() == false)
			return false;

		return true;
	}

	public virtual void SetInfo(int templateID)
	{
		DataTemplateID = templateID;

		if (ObjectType == EObjectType.Hero)
			CreatureData = Managers.Data.HeroDic[templateID];
		else
			CreatureData = Managers.Data.MonsterDic[templateID];

		gameObject.name = $"{CreatureData.DataId}_{CreatureData.DescriptionTextID}";

		// RigidBody
		RigidBody.mass = 0;

		// Skills
		Skills = gameObject.GetOrAddComponent<SkillComponent>();
		Skills.SetInfo(this, CreatureData);

		// Stat
		Hp = CreatureData.MaxHp;
		MaxHp = new CreatureStat(CreatureData.MaxHp);
		Atk = new CreatureStat(CreatureData.Atk);
		CriRate = new CreatureStat(CreatureData.CriRate);
		CriDamage = new CreatureStat(CreatureData.CriDamage);
		ReduceDamageRate = new CreatureStat(0);
		LifeStealRate = new CreatureStat(0);
		ThornsDamageRate = new CreatureStat(0);
		AttackSpeedRate = new CreatureStat(1);

		// State
		CreatureState = ECreatureState.Idle;
	}

	#region Battle

	public override void OnDamaged(BaseObject attacker, SkillBase skill)
	{
		base.OnDamaged(attacker, skill);

		if (attacker.IsValid() == false)
			return;

		Creature creature = attacker as Creature;
		if (creature == null)
			return;

		float finalDamage = creature.Atk.Value;
		Hp = Mathf.Clamp(Hp - finalDamage, 0, MaxHp.Value);

		Managers.Object.ShowDamageFont(CenterPosition, finalDamage, transform, false);

		if (Hp <= 0)
		{
			OnDead(attacker, skill);
			CreatureState = ECreatureState.Dead;
			return;
		}

		// 스킬에 따른 Effect 적용
		//if (skill.SkillData.EffectIds != null)
			//Effects.GenerateEffects(skill.SkillData.EffectIds.ToArray(), EEffectSpawnType.Skill, skill);
	}

	public override void OnDead(BaseObject attacker, SkillBase skill)
	{
		base.OnDead(attacker, skill);
	}
	#endregion

	#region Misc
	protected bool IsValid(BaseObject bo)
	{
		return bo.IsValid();
	}
	#endregion
}
