using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using static Define;

public class ObjectManager
{
	public HashSet<Hero> Heroes { get; } = new HashSet<Hero>();
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
			Heroes.Add(hero);
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
			Heroes.Remove(hero);
		}
		else if (obj.ObjectType == EObjectType.Monster)
		{
			Monster monster = obj.GetComponent<Monster>();
			Monsters.Remove(monster);
		}

		Managers.Resource.Destroy(obj.gameObject);
	}
}
