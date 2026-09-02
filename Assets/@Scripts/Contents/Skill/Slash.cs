using System.Collections.Generic;
using UnityEngine;

public class Slash : SkillBase
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
        Owner.PlayAnimation(SkillData.AnimName);
        base.DoSkill();
    }

    protected override void OnAttackEvent()
    {
        if (Owner.PlayingSkill != this)
            return;
        Debug.Log("Slash");

        Vector3Int lastTargetPos = Owner.CellPos;
        List<Monster> targets = Managers.Object.FindCircleTargets(Owner, 3);
        foreach (var target in targets)
        {
            if (target.IsValid())
            {
                if (target.CellPos.y < lastTargetPos.y)
                    lastTargetPos.y = target.CellPos.y;
                target.OnDamaged(Owner, this);
            }
        }

        while (Managers.Map.CanGo(Owner, lastTargetPos) == false)
        {
            lastTargetPos.y++;
        }

        Owner.TargetCellPos = lastTargetPos;
    }
}
