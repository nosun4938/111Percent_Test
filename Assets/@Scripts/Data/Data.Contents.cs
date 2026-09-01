using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;


namespace Data
{
	#region CreatureData
	[Serializable]
	public class CreatureData
	{
		public int DataId;
        public string DescriptionTextID;
        public string PrefabLabel;
		
		public float MaxHp;
		public float Atk;
	}
	#endregion

	#region MonsterData
	[Serializable]
	public class MonsterData : CreatureData
	{
		public string IconImage;
        public int DropItemId;
	}

	[Serializable]
	public class MonsterDataLoader : ILoader<int, MonsterData>
	{
		public List<MonsterData> monsters = new List<MonsterData>();
		public Dictionary<int, MonsterData> MakeDict()
		{
			Dictionary<int, MonsterData> dict = new Dictionary<int, MonsterData>();
			foreach (MonsterData monster in monsters)
				dict.Add(monster.DataId, monster);
			return dict;
		}
	}
	#endregion

	#region HeroData
	[Serializable]
	public class HeroData : CreatureData
	{
        public float UpMaxHpBonus;
        public float CriRate;
        public float CriDamage;

        public int DefaultSkillId;
        public int SkillAId;
        public int SkillBId;
		public int SkillCId;
    }

	[Serializable]
	public class HeroDataLoader : ILoader<int, HeroData>
	{
		public List<HeroData> heroes = new List<HeroData>();
		public Dictionary<int, HeroData> MakeDict()
		{
			Dictionary<int, HeroData> dict = new Dictionary<int, HeroData>();
			foreach (HeroData hero in heroes)
				dict.Add(hero.DataId, hero);
			return dict;
		}
	}
	#endregion

	#region SkillData
	[Serializable]
	public class SkillData
	{
		public int DataId;
		public string Name;
		public string ClassName;
		public string Description;
		public string AnimName;
		public float CoolTime;
		public float Duration;
		public float DamageMultiplier;
		public string Sound;
		public float SkillRange;
		public int TargetCount;
		public List<int> EffectIds = new List<int>();
		public EEffectSize EffectSize;
	}

	[Serializable]
	public class SkillDataLoader : ILoader<int, SkillData>
	{
		public List<SkillData> skills = new List<SkillData>();

		public Dictionary<int, SkillData> MakeDict()
		{
			Dictionary<int, SkillData> dict = new Dictionary<int, SkillData>();
			foreach (SkillData skill in skills)
				dict.Add(skill.DataId, skill);
			return dict;
		}
	}
	#endregion

}