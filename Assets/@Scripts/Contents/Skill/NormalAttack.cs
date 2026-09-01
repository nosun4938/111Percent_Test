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

	public override void SetInfo(Creature owner, int skillTemplateID)
	{
		base.SetInfo(owner, skillTemplateID);
	}

	public override void DoSkill()
	{
        Owner.PlayingSkill = this;
        Owner.LookAtTarget(Owner.Target);
        Owner.PlayAnimation(SkillData.AnimName);
        base.DoSkill();
	}

	protected override void OnAttackEvent()
	{
		if (Owner.Target.IsValid() == false)
			return;

        if (Owner.PlayingSkill != this)
            return;

        Owner.OnDamaged(Owner.Target, this);
        Owner.Target.OnDamaged(Owner, this);
	}
}
