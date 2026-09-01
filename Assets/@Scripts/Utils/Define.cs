using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Define
{
    public enum EFindPathResult
    {
        Fail_LerpCell,
        Fail_NoPath,
        Fail_MoveTo,
		Fail_Monster,
        Success,
    }
    public enum EScene
	{
		Unknown,
		TitleScene,
		GameScene,
	}

	public enum EUIEvent
	{
		Click,
		PointerDown,
		PointerUp,
		Drag,
	}

	public enum EJoystickState
	{
		PointerDown,
		PointerUp,
		Drag,
	}

	public enum ESound
	{
		Bgm,
		Effect,
		Max,
	}

	public enum EObjectType
	{
		None,
		Hero,
		Monster,
		Effect,
	}

	public enum ECreatureState
	{
		None,
		Idle,
		Move,
		Skill,
		OnDamaged,
		Dead
	}

	public enum EHeroMoveState
	{
		None,
		TargetMonster,
		CollectEnv,
		ReturnToCamp,
		ForceMove,
		ForcePath
	}

	public enum ELayer
	{
		Default = 0,
		TransparentFX = 1,
		IgnoreRaycast = 2,
		Dummy1 = 3,
		Water = 4,
		UI = 5,
		Hero = 6,
		Monster = 7,
		Env = 8,
		Obstacle = 9,
		Projectile = 10,
	}

	public enum ECellCollisionType
	{
		None,
		Wall,
		Monster,
	}

	public enum ESkillSlot
	{
		Default,
		A,
		B,
		C,
	}

	public enum EEffectSize
	{
		CircleSmall,
		CircleNormal,
		CircleBig,
		ConeSmall,
		ConeNormal,
		ConeBig,
	}

	public enum EStatModType
	{
		Add,
		PercentAdd,
		PercentMult,
	}

	public enum EEffectType
	{
		Buff,
		Debuff,
		CrowdControl,
	}

	public enum EEffectClearType
	{
		TimeOut, // 시간초과로 인한 Effect 종료
		ClearSkill, // 정화 스킬로 인한 Effect 종료
		TriggerOutAoE, // AoE스킬을 벗어난 종료
		EndOfAirborne, // 에어본이 끝난 경우 호출되는 종료
	}

	public enum ELanguage
	{
		Korean,
		English,
		French,
		SimplifiedChinese,
		TraditionalChinese,
		Japanese
	}

	public enum EItemGrade
	{
		None,
		Normal,
		Rare,
		Epic,
		Legendary
	}

	public enum EItemGroupType
	{
		None,
		Equipment,
		Consumable,
	}

	public enum EItemType
	{
		None,
		Weapon,
		Armor,
		Potion,
		Scroll
	}

	public enum EItemSubType
	{
		None,

		Sword,
		Dagger,
		Bow,

		Helmet,
		Armor,
		Shield,
		Gloves,
		Shoes,

		EnchantWeapon,
		EnchantArmor,

		HealthPotion,
		ManaPotion,
	}

	public enum EEquipSlotType
	{
		None,
		Weapon = 1,
		Helmet = 2,
		Armor = 3,
		Shield = 4,
		Gloves = 5,
		Shoes = 6,
		EquipMax,

		Inventory = 100,
		WareHouse = 200,
	}

	public enum EQuestPeriodType
	{
		Once, // 단발성
		Daily,
		Weekly,
		Infinite, // 무한으로
	}

	public enum EQuestCondition
	{
		None,
		Level,
		ItemLevel,

	}

	public enum EQuestObjectiveType
	{
		KillMonster,
		EarnMeat,
		SpendMeat,
		EarnWood,
		SpendWood,
		EarnMineral,
		SpendMineral,
		EarnGold,
		SpendGold,
		UseItem,
		Survival,
		ClearDungeon
	}

	public enum EQuestRewardType
	{
		Hero,
		Gold,
		Mineral,
		Meat,
		Wood,
		Item,
	}

	public enum EQuestState
	{
		None,
		Processing,
		Completed,
		Rewarded,
	}

	public enum EBroadcastEventType
	{
		None,
		ChangeHp,
		KillMonster,
		LevelUp,
		DungeonClear,
		ChangeInventory,
		ChangeCrew,
		QuestClear,
	}

	public enum EResourceType
	{
		Wood,
		Mineral,
		Meat,
		Gold,
		Materials,
		Dia
	}

	public enum EProviderType
	{
		None = 0,
		Guest = 1,
		Google = 2,
		Facebook = 3,
	}

	public const int CAMERA_PROJECTION_SIZE = 12;

	public const int MONSTER_SLIME_ID = 202001;
	public const int MONSTER_SPIDER_COMMON_ID = 202002;
	public const int MONSTER_WOOD_COMMON_ID = 202004;
	public const int MONSTER_GOBLIN_ARCHER_ID = 202005;
	public const int MONSTER_BEAR_ID = 202006;

	public const char MAP_TOOL_WALL = '0';
	public const char MAP_TOOL_NONE = '1';
	public const char MAP_TOOL_SEMI_WALL = '2';
}

public static class AnimName
{
	public const string ATTACK_A = "attack";
	public const string ATTACK_B = "attack";
	public const string SKILL_A = "skill";
	public const string SKILL_B = "skill";
	public const string IDLE = "Idle";
	public const string MOVE = "Move";
	public const string DAMAGED = "hit";
	public const string DEAD = "dead";
	public const string EVENT_ATTACK_A = "event_attack";
	public const string EVENT_ATTACK_B = "event_attack";
	public const string EVENT_SKILL_A = "event_attack";
	public const string EVENT_SKILL_B = "event_attack";
}

public static class SortingLayers
{
	public const int SPELL_INDICATOR = 200;
	public const int CREATURE = 300;
	public const int ENV = 300;
	public const int NPC = 310;
	public const int PROJECTILE = 310;
	public const int SKILL_EFFECT = 310;
	public const int DAMAGE_FONT = 410;
}
