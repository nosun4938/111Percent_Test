using Data;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using static Define;

public class Creature : BaseObject
{
	public BaseObject Target { get; protected set; }
    public SkillComponent Skills { get; protected set; }
    public SkillBase PlayingSkill { get; set; }
    public Data.CreatureData CreatureData { get; private set; }
    public Vector3Int TargetCellPos { get; set; }

    public Transform Shield { get; private set; }

    //public EffectComponent Effects { get; set; }

    public bool _onBlock = false;

    #region Stats
    public float Hp { get; set; }
	public float MaxHp;
	public float Atk;
	public float CriRate;
	public float CriDamage;
	#endregion

	protected ECreatureState _creatureState = ECreatureState.None;
	public virtual ECreatureState CreatureState
	{
		get { return _creatureState; }
		set
		{
			if (_creatureState != value)
			{
				_creatureState = value;
			}
		}
	}

	public override bool Init()
	{
		if (base.Init() == false)
			return false;

        // Temp, Effect로 옮겨야 함
        Shield = transform.Find("Shield");

        return true;
	}

	public virtual void SetInfo(int templateID)
	{
		DataTemplateID = templateID;

		if (ObjectType == EObjectType.Hero)
			CreatureData = Managers.Data.HeroDic[templateID];
		else
			CreatureData = Managers.Data.MonsterDic[templateID];

		gameObject.name = $"{CreatureData.DataId}_{CreatureData.DescriptionTextID}";

		// RigidBody
		RigidBody.mass = 0;

        // Stat
        MaxHp = CreatureData.MaxHp;
        Hp = MaxHp;
        Atk = CreatureData.Atk;

        // Skills
        Skills = gameObject.GetOrAddComponent<SkillComponent>();
        Skills.SetInfo(this, CreatureData);

		// Map
		StartCoroutine(CoLerpToCellPos());

        // Temp
        Shield.gameObject.SetActive(false);
    }

	#region Battle

	public override void OnDamaged(BaseObject attacker, SkillBase skill)
	{
		base.OnDamaged(attacker, skill);
		if (CreatureState == ECreatureState.Dead)
			return;

		if (attacker.IsValid() == false)
			return;
	}

	public override void OnDead(BaseObject attacker, SkillBase skill)
	{
		base.OnDead(attacker, skill);
        if (CreatureState == ECreatureState.Dead)
            return;
    }
    #endregion

    #region Misc
    protected bool IsValid(BaseObject bo)
	{
		return bo.IsValid();
	}
    #endregion

    #region Map

    public EFindPathResult MoveOneCellToward(Vector3 dir)
    {
		if (LerpCellPosCompleted == false)
            return EFindPathResult.Fail_LerpCell;

        if (dir == Vector3.zero)
			return EFindPathResult.Fail_NoPath;

		int dirCellPosX = Mathf.Abs(dir.x) < 0.5f ? 0 : Math.Sign(dir.x);
		int dirCellPosY = dir.y < -0.5f ? -1 : 0;

        Vector3Int dirCellPos = new Vector3Int(dirCellPosX, dirCellPosY, 0);
		Vector3Int nextPos = CellPos + dirCellPos;

        BaseObject nextObject = Managers.Map.GetObject(nextPos);
		if (nextObject is Monster monster)
		{
			Target = nextObject;
			TargetCellPos = nextPos;
            return EFindPathResult.Fail_Monster;
        }

        if (Managers.Map.MoveTo(this, nextPos) == false)
            return EFindPathResult.Fail_MoveTo;

		return EFindPathResult.Success;
    }

    public bool MoveToCellPos(Vector3Int destCellPos, bool forceMoveCloser = false)
    {
        if (LerpCellPosCompleted == false)
            return false;

        return Managers.Map.MoveTo(this, destCellPos);
    }

    protected IEnumerator CoLerpToCellPos()
    {
        while (true)
        {
            LerpToCellPos(8);
            yield return null;
        }
    }
    public IEnumerator CoBlock(float seconds)
    {
        _onBlock = true;
        Shield.gameObject.SetActive(true);
        yield return new WaitForSeconds(seconds);
        _onBlock = false;
        Shield.gameObject.SetActive(false);
    }
    #endregion
}
