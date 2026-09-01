using Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AdaptivePerformance.Provider;
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
		
		float scale = MonsterData.Scale;
		transform.localScale = new Vector3(scale, scale, scale);

		// Sprite
		SpriteRenderer.sprite = Managers.Resource.Load<Sprite>($"{MonsterData.IconImage}");
	}

	#region Battle
	public override void OnDamaged(BaseObject attacker, SkillBase skill)
	{
		base.OnDamaged(attacker, skill);

        Creature creature = attacker as Creature;
        if (creature == null)
            return;
		Target = creature;
		Debug.Log($"Target on {Target}");

        float finalDamage = creature.Atk * skill.SkillData.DamageMultiplier;
        Hp = Mathf.Clamp(Hp - finalDamage, 0, MaxHp);

        Managers.Object.ShowDamageFont(transform.position, finalDamage, transform, false);

        if (Hp <= 0)
        {
            OnDead(attacker, skill);
            CreatureState = ECreatureState.Dead;
            return;
        }

        // 스킬에 따른 Effect 적용
        //if (skill.SkillData.EffectIds != null)
        //Effects.GenerateEffects(skill.SkillData.EffectIds.ToArray(), EEffectSpawnType.Skill, skill);
    }

    public override void OnDead(BaseObject attacker, SkillBase skill)
	{
		base.OnDead(attacker, skill);

        // Drop Gold
        Managers.Game.EarnGold(Random.Range(1, 20));

        Managers.Object.Despawn(this);
	}
	#endregion
}
