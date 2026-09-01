using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using static Define;

public class ObjectManager
{
	public Hero Player { get; set; } = new Hero();
	public HashSet<Monster> Monsters { get; } = new HashSet<Monster>();

	#region Roots
	public Transform GetRootTransform(string name)
	{
		GameObject root = GameObject.Find(name);
		if (root == null)
			root = new GameObject { name = name };

		return root.transform;
	}

	public Transform HeroRoot { get { return GetRootTransform("@Heroes"); } }
	public Transform MonsterRoot { get { return GetRootTransform("@Monsters"); } }
	#endregion

	public void ShowDamageFont(Vector2 position, float damage, Transform parent, bool isCritical = false)
	{
		GameObject go = Managers.Resource.Instantiate("DamageFont", pooling: true);
		DamageFont damageText = go.GetComponent<DamageFont>();
		damageText.SetInfo(position, damage, parent, isCritical);
	}

	public GameObject SpawnGameObject(Vector3 position, string prefabName)
	{
		GameObject go = Managers.Resource.Instantiate(prefabName, pooling: true);
		go.transform.position = position;
		return go;
	}

	public T Spawn<T>(Vector3Int cellPos, int templateID) where T : BaseObject
	{
		Vector3 spawnPos = Managers.Map.Cell2World(cellPos);
		return Spawn<T>(spawnPos, templateID);
	}

	public T Spawn<T>(Vector3 position, int templateID) where T : BaseObject
	{
		string prefabName = typeof(T).Name;

		GameObject go = Managers.Resource.Instantiate(prefabName);
		go.name = prefabName;
		go.transform.position = position;

		BaseObject obj = go.GetComponent<BaseObject>();

		if (obj.ObjectType == EObjectType.Hero)
		{
			obj.transform.parent = HeroRoot;
			Hero hero = go.GetComponent<Hero>();
			Player = hero;
			hero.SetInfo(templateID);
		}
		else if (obj.ObjectType == EObjectType.Monster)
		{
			obj.transform.parent = MonsterRoot;
			Monster monster = go.GetComponent<Monster>();
			Monsters.Add(monster);
			monster.SetInfo(templateID);
		}
		return obj as T;
	}

	public void Despawn<T>(T obj) where T : BaseObject
	{
		EObjectType objectType = obj.ObjectType;

		if (obj.ObjectType == EObjectType.Hero)
		{
			Hero hero = obj.GetComponent<Hero>();
			Player = null;
		}
		else if (obj.ObjectType == EObjectType.Monster)
		{
			Monster monster = obj.GetComponent<Monster>();
			Monsters.Remove(monster);
		}

		Managers.Map.RemoveObject(obj);
		Managers.Resource.Destroy(obj.gameObject);
	}

    #region Skill 판정
    public List<Monster> FindLineTargets(Hero owner, float range)
    {
        HashSet<Monster> targets = new HashSet<Monster>();
        HashSet<Monster> ret = new HashSet<Monster>();

        List<Monster> objs = Managers.Map.GatherObjects<Monster>(owner.transform.position, 0, range);
        targets.AddRange(objs);

        foreach (Monster target in targets)
        {
            // 1. 거리안에 있는지 확인
            var targetPos = target.transform.position;
            float distance = Vector3.Distance(targetPos, owner.transform.position);

            if (distance > range)
                continue;

			// 2. Hero보다 아래에 있는지 확인
			if (targetPos.y > owner.transform.position.y)
				continue;

            ret.Add(target);
        }

        return ret.ToList();
    }

    public List<Monster> FindCircleTargets(Hero owner, Vector3 startPos, float range)
    {
        HashSet<Monster> targets = new HashSet<Monster>();
        HashSet<Monster> ret = new HashSet<Monster>();

        List<Monster> objs = Managers.Map.GatherObjects<Monster>(owner.transform.position, range, range);
        targets.AddRange(objs);

        foreach (var target in targets)
        {
            // 1. 거리안에 있는지 확인
            var targetPos = target.transform.position;
            float distSqr = (targetPos - startPos).sqrMagnitude;

            if (distSqr < range * range)
                ret.Add(target);
        }

        return ret.ToList();
    }
    #endregion
}
