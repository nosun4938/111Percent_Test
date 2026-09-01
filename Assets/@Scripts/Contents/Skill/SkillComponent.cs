using Data;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;
using Random = UnityEngine.Random;

public class SkillComponent : InitBase
{
	public List<SkillBase> SkillList { get; } = new List<SkillBase>();
	public List<SkillBase> ActiveSkills { get; set; } = new List<SkillBase>();

	public SkillBase DefaultSkill { get; private set; }
	public SkillBase ASkill { get; private set; }
	public SkillBase BSkill { get; private set; }
	public SkillBase CSkill { get; private set; }

	Hero _owner;

	public override bool Init()
	{
		if (base.Init() == false)
			return false;

		return true;
	}

	public void SetInfo(Hero owner, HeroData heroData)
	{
		_owner = owner;

		AddSkill(heroData.DefaultSkillId, ESkillSlot.Default);
		AddSkill(heroData.SkillAId, ESkillSlot.A);
		AddSkill(heroData.SkillBId, ESkillSlot.B);
		AddSkill(heroData.SkillCId, ESkillSlot.C);
	}

	public void AddSkill(int skillTemplateID, ESkillSlot skillSlot)
	{
		if (skillTemplateID == 0)
			return;

		if (Managers.Data.SkillDic.TryGetValue(skillTemplateID, out var data) == false)
		{
			Debug.LogWarning($"AddSkill Failed {skillTemplateID}");
			return;
		}

		SkillBase skill = gameObject.AddComponent(Type.GetType(data.ClassName)) as SkillBase;
		if (skill == null)
			return;

		skill.SetInfo(_owner, skillTemplateID);

		SkillList.Add(skill);

		switch (skillSlot)
		{
			case Define.ESkillSlot.Default:
				DefaultSkill = skill;
				break;
			case Define.ESkillSlot.A:
				ASkill = skill;
				ActiveSkills.Add(skill);
				break;
			case Define.ESkillSlot.B:
				BSkill = skill;
				ActiveSkills.Add(skill);
				break;
			case Define.ESkillSlot.C:
				CSkill = skill;
				ActiveSkills.Add(skill);
				break;
		}
	}
}
