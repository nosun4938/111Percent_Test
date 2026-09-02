using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Define
{
    public enum EScene
	{
		Unknown,
		LoadScene,
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

    public enum EFindPathResult
    {
        Fail_LerpCell,
        Fail_NoPath,
        Fail_MoveTo,
        Fail_Monster,
        Success,
    }

    public enum EObjectType
	{
		None,
		Hero,
		Monster,
		Boss,
		Effect,
	}

	public enum ECreatureState
	{
		None,
		Idle,
		Move,
		Skill,
		Dead,
		GameOver,
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

	public enum EItemGrade
	{
		None,
		Normal,
		Rare,
		Epic,
		Legendary
	}

	public enum EItemType
	{
		Sword
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

	public enum EBroadcastEventType
	{
		None,
		ChangeHp,
		ChangeGold,
		ScoreUp,
		ChangeAttackPower,
		SkillUsed,
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
	public const string IDLE = "Idle";
	public const string MOVE = "Move";
	public const string DAMAGED = "hit";
	public const string DEAD = "Dead";
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
