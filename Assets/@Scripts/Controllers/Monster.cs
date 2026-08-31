using Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public class Monster : Creature
{
	public Data.MonsterData MonsterData { get; set; }

	public override ECreatureState CreatureState 
	{
		get { return base.CreatureState; }
		set
		{
			if (_creatureState != value)
			{
				base.CreatureState = value;
			}
		}
	}

	public override bool Init()
	{
		if (base.Init() == false)
			return false;

		ObjectType = EObjectType.Monster;
        
		
		// Map
        Collider.isTrigger = true;
        RigidBody.simulated = false;
        return true;
	}

	public override void SetInfo(int templateID)
	{
		base.SetInfo(templateID);
		MonsterData = CreatureData as Data.MonsterData;

		// State
		CreatureState = ECreatureState.Idle;

		// Sprite
		SpriteRenderer.sprite = Managers.Resource.Load<Sprite>($"{MonsterData.IconImage}");
	}

	void Start()
	{
		
	}

	#region Battle
	public override void OnDamaged(BaseObject attacker, SkillBase skill)
	{
		base.OnDamaged(attacker, skill);
	}

	public override void OnDead(BaseObject attacker, SkillBase skill)
	{
		base.OnDead(attacker, skill);

		// Drop Item
		int dropItemId = MonsterData.DropItemId;

		Managers.Object.Despawn(this);
	}
	#endregion
}
