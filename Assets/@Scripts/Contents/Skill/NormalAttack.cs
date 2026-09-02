using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

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

        Vector3Int lastTargetPos = Owner.Target.CellPos;
        Owner.Target.OnDamaged(Owner, this);
        Owner.OnDamaged(Owner.Target, this); // 몬스터랑 딜교했다는 설정

        if (Managers.Map.CanGo(Owner, lastTargetPos) == false)
            lastTargetPos = Owner.CellPos;
		
		Owner.TargetCellPos = lastTargetPos;
	}
}
