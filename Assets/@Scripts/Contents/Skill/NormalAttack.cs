using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalAttack : SkillBase
{
	public override bool Init()
	{
		if (base.Init() == false)
			return false;

		return true;
	}

	public override void SetInfo(Hero owner, int skillTemplateID)
	{
		base.SetInfo(owner, skillTemplateID);
	}

	public override void DoSkill()
	{
		base.DoSkill();

		Owner.CreatureState = Define.ECreatureState.Skill;
		Owner.PlayAnimation(SkillData.AnimName);

		Owner.LookAtTarget(Owner.Target);
	}

	void PickupTargetAndProcessHit()
	{
	}

	protected override void OnAttackEvent()
	{
		if (Owner.Target.IsValid() == false)
			return;
        Owner.OnDamaged(Owner.Target, this);
        Owner.Target.OnDamaged(Owner, this);
	}
}
