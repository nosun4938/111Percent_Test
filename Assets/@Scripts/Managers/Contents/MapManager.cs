using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;
using static Define;


public class MapManager
{
	public GameObject Map { get; private set; }
	public string MapName { get; private set; }
	public Grid CellGrid { get; private set; }

	// (CellPos, BaseObject)
	Dictionary<Vector3Int, BaseObject> _cells = new Dictionary<Vector3Int, BaseObject>();
	public StageTransition StageTransition;

	private int MinX;
	private int MaxX;
	private int MinY;
	private int MaxY;

	public Vector3Int World2Cell(Vector3 worldPos) { return CellGrid.WorldToCell(worldPos); }
	public Vector3 Cell2World(Vector3Int cellPos) { return CellGrid.CellToWorld(cellPos); }

	ECellCollisionType[,] _collision;

	public void LoadMap(string mapName)
	{
		DestroyMap();

		GameObject map = Managers.Resource.Instantiate(mapName);
		map.transform.position = Vector3.zero;
		map.name = $"@Map_{mapName}";

		StageTransition = map.GetComponent<StageTransition>();

		Map = map;
		MapName = mapName;
		CellGrid = map.GetComponent<Grid>();

        ParseCollisionData(map, mapName);
	}

	public void DestroyMap()
	{
		ClearObjects();

		if (Map != null)
			Managers.Resource.Destroy(Map);
	}

    void ParseCollisionData(GameObject map, string mapName, string tilemap = "Tilemap_Collision")
    {
        GameObject collision = Util.FindChild(map, tilemap, true);
        if (collision != null)
            collision.SetActive(false);

        // Collision 관련 파일
        TextAsset txt = Managers.Resource.Load<TextAsset>($"{mapName}Collision");
        StringReader reader = new StringReader(txt.text);

        MinX = int.Parse(reader.ReadLine());
        MaxX = int.Parse(reader.ReadLine());
        MinY = int.Parse(reader.ReadLine());
        MaxY = int.Parse(reader.ReadLine());

        int xCount = MaxX - MinX + 1;
        int yCount = MaxY - MinY + 1;
        _collision = new ECellCollisionType[xCount, yCount];

        for (int y = 0; y < yCount; y++)
        {
            string line = reader.ReadLine();
            for (int x = 0; x < xCount; x++)
            {
                switch (line[x])
                {
                    case Define.MAP_TOOL_WALL:
                        _collision[x, y] = ECellCollisionType.Wall;
                        break;
                    case Define.MAP_TOOL_NONE:
                        _collision[x, y] = ECellCollisionType.None;
                        break;
                }
            }
        }
    }

    public bool MoveTo(Creature obj, Vector3Int cellPos, bool forceMove = false)
	{
		if (CanGo(obj, cellPos) == false)
			return false;

		// 기존 좌표에 있던 오브젝트를 밀어준다.
		// (단, 처음 신청했으면 해당 CellPos의 오브젝트가 본인이 아닐 수도 있음)
		RemoveObject(obj);

		// 새 좌표에 오브젝트를 등록한다.
		AddObject(obj, cellPos);

		// 셀 좌표 이동
		obj.SetCellPos(cellPos, forceMove);

		Debug.Log($"Move To {cellPos}");

		return true;
	}

	#region Helpers
	public List<T> GatherObjects<T>(Vector3 pos, float rangeX, float rangeY) where T : BaseObject
	{
		HashSet<T> objects = new HashSet<T>();

		Vector3Int left = World2Cell(pos + new Vector3(-rangeX, 0));
		Vector3Int right = World2Cell(pos + new Vector3(+rangeX, 0));
		Vector3Int bottom = World2Cell(pos + new Vector3(0, -rangeY));
		Vector3Int top = World2Cell(pos + new Vector3(0, +rangeY));
		int minX = left.x;
		int maxX = right.x;
		int minY = bottom.y;
		int maxY = top.y;

		for (int x = minX; x <= maxX; x++)
		{
			for (int y = minY; y <= maxY; y++)
			{
				Vector3Int tilePos = new Vector3Int(x, y, 0);

				// 타입에 맞는 리스트 리턴
				T obj = GetObject(tilePos) as T;
				if (obj == null)
					continue;

				objects.Add(obj);
			}
		}

		return objects.ToList();
	}

	public BaseObject GetObject(Vector3Int cellPos)
	{
		// 없으면 null
		_cells.TryGetValue(cellPos, out BaseObject value);
		return value;
	}

	public BaseObject GetObject(Vector3 worldPos)
	{
		Vector3Int cellPos = World2Cell(worldPos);
		return GetObject(cellPos);
	}

	public void RemoveObject(BaseObject obj)
	{
		// 기존의 좌표 제거
		int extraCells = 0;
		if (obj != null)
			extraCells = obj.ExtraCells;

		Vector3Int cellPos = obj.CellPos;

		for (int dx = -extraCells; dx <= extraCells; dx++)
		{
			for (int dy = -extraCells; dy <= extraCells; dy++)
			{
				Vector3Int newCellPos = new Vector3Int(cellPos.x + dx, cellPos.y + dy);
				BaseObject prev = GetObject(newCellPos);

				if (prev == obj)
					_cells[newCellPos] = null;
			}
		}
	}

	public void AddObject(BaseObject obj, Vector3Int cellPos)
	{
		int extraCells = 0;
		if (obj != null)
			extraCells = obj.ExtraCells;

		for (int dx = -extraCells; dx <= extraCells; dx++)
		{
			for (int dy = -extraCells; dy <= extraCells; dy++)
			{
				Vector3Int newCellPos = new Vector3Int(cellPos.x + dx, cellPos.y + dy);

				BaseObject prev = GetObject(newCellPos);
				if (prev != null && prev != obj)
					Debug.LogWarning($"AddObject 수상함");

				_cells[newCellPos] = obj;
			}
		}
	}

	public bool CanGo(BaseObject self, Vector3 worldPos, bool ignoreObjects = false)
	{
		return CanGo(self, World2Cell(worldPos), ignoreObjects);
	}

	public bool CanGo(BaseObject self, Vector3Int cellPos, bool ignoreObjects = false)
	{
		int extraCells = 0;
		if (self != null)
			extraCells = self.ExtraCells;

		for (int dx = -extraCells; dx <= extraCells; dx++)
		{
			for (int dy = -extraCells; dy <= extraCells; dy++)
			{
				Vector3Int checkPos = new Vector3Int(cellPos.x + dx, cellPos.y + dy);

				if (CanGo_Internal(self, checkPos, ignoreObjects) == false)
					return false;
			}
		}

		return true;
	}

	bool CanGo_Internal(BaseObject self, Vector3Int cellPos, bool ignoreObjects = false)
	{
		if (cellPos.x < MinX || cellPos.x > MaxX)
			return false;
		if (cellPos.y < MinY || cellPos.y > MaxY)
			return false;

		if (ignoreObjects == false)
		{
			BaseObject obj = GetObject(cellPos);
			if (obj != null && obj != self)
				return false;
		}

		int x = cellPos.x - MinX;
		int y = MaxY - cellPos.y;
		ECellCollisionType type = _collision[x, y];
		if (type == ECellCollisionType.None)
			return true;

		return false;
	}

	public void ClearObjects()
	{
		_cells.Clear();
	}

	#endregion
}