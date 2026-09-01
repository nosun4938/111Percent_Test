using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SkillBase : InitBase
{
	public Creature Owner { get; protected set; }
	public float RemainCoolTime { get; set; }

	public Data.SkillData SkillData { get; private set; }

	public override bool Init()
	{
		if (base.Init() == false)
			return false;

		return true;
	}

	public virtual void SetInfo(Creature owner, int skillTemplateID)
	{
		Owner = owner;
		SkillData = Managers.Data.SkillDic[skillTemplateID];

		
	}

	public virtual void DoSkill()
	{
		// 준비된 스킬에서 해제
		if (Owner.Skills != null)
			Owner.Skills.ActiveSkills.Remove(this);
		
		OnAttackEvent();
        StartCoroutine(CoCountdownCooldown());
	}

	private IEnumerator CoCountdownCooldown()
	{
		RemainCoolTime = SkillData.CoolTime;
		yield return new WaitForSeconds(SkillData.CoolTime);
		RemainCoolTime = 0;

		// 준비된 스킬에 추가
		if (Owner.Skills != null)
			Owner.Skills.ActiveSkills.Add(this);
	}

	public virtual void CancelSkill()
	{

	}

	protected abstract void OnAttackEvent();
}
