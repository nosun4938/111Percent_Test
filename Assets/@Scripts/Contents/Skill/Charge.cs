using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class Charge : SkillBase
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
        Debug.Log("Charge");

        Vector3Int lastTargetPos = Owner.CellPos;
        List<Monster> targets = Managers.Object.FindLineTargets(Owner, 5);
        foreach (var target in targets)
        {
            if (target.IsValid())
            {
                if (target.CellPos.y < lastTargetPos.y)
                    lastTargetPos = target.CellPos;
                target.OnDamaged(Owner, this);
            }
        }

        while (Managers.Map.CanGo(Owner,lastTargetPos) == false)
        {
            lastTargetPos.y++;
        }

        Owner.TargetCellPos = lastTargetPos;
    }
}
