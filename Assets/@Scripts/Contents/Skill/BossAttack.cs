using UnityEngine;

public class BossAttack : SkillBase
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
        base.DoSkill();
    }

    protected override void OnAttackEvent()
    {
        if (Owner.Target.IsValid() == false)
            return;

        if (Owner.PlayingSkill != this)
            return;

        Owner.Target.OnDamaged(Owner, this);
    }
}
