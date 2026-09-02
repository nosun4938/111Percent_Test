using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using static Define;

public class BaseObject : InitBase
{
	public int ExtraCells { get; set; } = 0;

	public EObjectType ObjectType { get; protected set; } = EObjectType.None;
    public SpriteRenderer SpriteRenderer { get; private set; }
    public Animator Animator { get; private set; }
    public CircleCollider2D Collider { get; private set; }
	public Rigidbody2D RigidBody { get; private set; }

    public string CurrentAnimName { get; set; }

    public int DataTemplateID { get; set; }

	bool _lookRight = true;
	public bool LookRight
	{
		get { return _lookRight; }
		set
		{
            _lookRight = value;
			Flip(!value);
		}
	}

	public override bool Init()
	{
		if (base.Init() == false)
			return false;

        SpriteRenderer = GetComponent<SpriteRenderer>();
        Animator = GetComponent<Animator>();
        Collider = gameObject.GetOrAddComponent<CircleCollider2D>();
		RigidBody = GetComponent<Rigidbody2D>();

        return true;
	}

	public void LookAtTarget(BaseObject target)
	{
		Vector2 dir = target.transform.position - transform.position;
		if (dir.x > 0)
			LookRight = true;
		else
			LookRight = false;
	}

	public static Vector3 GetLookAtRotation(Vector3 dir)
	{
		// Mathf.Atan2를 사용해 각도를 계산하고, 라디안에서 도로 변환
		float angle = Mathf.Atan2(-dir.x, dir.y) * Mathf.Rad2Deg;

		// Z축을 기준으로 회전하는 Vector3 값을 리턴
		return new Vector3(0, 0, angle);
	}

	#region Battle
	public virtual void OnDamaged(BaseObject attacker, SkillBase skill)
	{
		
	}

	public virtual void OnDead(BaseObject attacker, SkillBase skill)
	{

	}
    #endregion

    #region Animation Helpers
    public void Flip(bool flag)
    {
        if (SpriteRenderer == null)
            return;

        SpriteRenderer.flipX = flag;
    }
    protected virtual void UpdateAnimation()
    {

    }

    public void PlayAnimation(string animName)
    {
        if (CurrentAnimName == animName)
            return;

        CurrentAnimName = animName;
        Animator.Play(animName, 0, 0f);
    }

    public bool IsAnimFinished()
    {
        var info = Animator.GetCurrentAnimatorStateInfo(0);
        return info.IsName(CurrentAnimName) && info.normalizedTime >= 1f;
    }

    public float GetAnimClipLength(string animName)
    {
        float length = 0f;
        foreach (AnimationClip clip in Animator.runtimeAnimatorController.animationClips)
        {
            if (clip.name == animName)
            {
                length = clip.length;
            }
        }
        return length;
    }
    #endregion

    #region Map
    public bool LerpCellPosCompleted { get; protected set; }

	Vector3Int _cellPos;
	public Vector3Int CellPos
	{
		get { return _cellPos; }
		protected set
		{
			_cellPos = value;
			LerpCellPosCompleted = false;
		}
	}

	public void SetCellPos(Vector3Int cellPos, bool forceMove = false)
	{
        CellPos = cellPos;
		LerpCellPosCompleted = false;

		if (forceMove)
		{
			transform.position = Managers.Map.Cell2World(CellPos);
			LerpCellPosCompleted = true;
        }
	}

	public void LerpToCellPos(float moveSpeed)
	{
		if (LerpCellPosCompleted)
			return;

		Vector3 destPos = Managers.Map.Cell2World(CellPos);
		Vector3 dir = destPos - transform.position;
        if (dir.x > 0)
			LookRight = true;
		else
            LookRight = false;

		if (dir.magnitude < 0.01f)
		{
			transform.position = destPos;
			LerpCellPosCompleted = true;
			return;
		}

		float moveDist = Mathf.Min(dir.magnitude, moveSpeed * Time.deltaTime);
		transform.position += dir.normalized * moveDist;
	}
	#endregion
}
