using UnityEngine;

public class Shield : SkillBase
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

        Owner.StartCoroutine(Owner.CoBlock(3.0f));
    }
}
