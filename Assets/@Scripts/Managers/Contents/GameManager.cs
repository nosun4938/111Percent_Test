using Data;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using static Define;
using static TMPro.SpriteAssetUtilities.TexturePacker_JsonArray;
using static UnityEngine.Rendering.DebugUI;
using Random = UnityEngine.Random;

[Serializable]
public class GameSaveData
{
    public int HighScore = 0;
    public int Score = 0;
    public int Gold = 0;

    public int ItemDbIdGenerator = 1;
    public List<ItemSaveData> Items = new List<ItemSaveData>();
}

[Serializable]
public class ItemSaveData
{
    public int InstanceID;
    public int TemplateID;
    public int Count;
    public int EquipSlot; // 장착 + 인벤 + 창고
    public int EnchantCount;
}

public class GameManager
{
    #region GameData
    GameSaveData _saveData = new GameSaveData();
    public GameSaveData SaveData { get { return _saveData; } set { _saveData = value; } }

    public int Gold
    {
        get { return _saveData.Gold; }
        private set
        {
            int diff = _saveData.Gold - value;
            _saveData.Gold = value;
            OnBroadcastEvent?.Invoke(EBroadcastEventType.ChangeGold, diff);
        }
    }

    public void EarnGold(int amount)
    {
        Gold += amount;
    }

    public int Score
    {
        get { return _saveData.Score; }
        private set
        {
            _saveData.Score = value;
            if (value > _saveData.HighScore)
                _saveData.HighScore = value;
            OnBroadcastEvent?.Invoke(EBroadcastEventType.ScoreUp, value);
        }
    }

    public void EarnScore(int amount)
    {
        Score += amount;
    }

    public int GenerateItemDbId()
    {
        int itemDbId = _saveData.ItemDbIdGenerator;
        _saveData.ItemDbIdGenerator++;
        return itemDbId;
    }
    #endregion

    #region Hero
    private Vector2 _moveDir;
	public Vector2 MoveDir
	{
		get { return _moveDir; }
		set
		{
			_moveDir = value;
			OnMoveDirChanged?.Invoke(value);
		}
	}

	private float _hpRatio;
	public float HpRatio
	{
		get { return _hpRatio; }
		set
		{
            _hpRatio = value;
			BroadcastEvent(EBroadcastEventType.ChangeHp, _hpRatio);
		}
	}

    private float _attackPower;
    public float AttackPower
    {
        get { return _attackPower; }
        set
        {
            _attackPower = value;
            BroadcastEvent(EBroadcastEventType.ChangeAttackPower, _attackPower);
        }
    }
    public void ChangeWeapon()
    {
        Sword sword = Managers.Inventory.GetEquippedItem(EEquipSlotType.Weapon) as Sword;
        if (sword == null)
            return;
        AttackPower = sword.Damage;
    }
    public void AddPlayerPower(float power)
    {
        AttackPower += power;
    }

	private EJoystickState _joystickState;
	public EJoystickState JoystickState
	{
		get { return _joystickState; }
		set
		{
			_joystickState = value;
			OnJoystickStateChanged?.Invoke(_joystickState);
		}
	}

	private ESkillSlot _skillSlot;
	public ESkillSlot SkillSlot
	{
		get { return _skillSlot; }
		set
		{
			_skillSlot = value;
			OnSkillSlotChanged?.Invoke(_skillSlot);
		}
	}
    
    public float GetSkillCooldownRatio(ESkillSlot slot)
    {
        Hero player = Managers.Object.Player;
        if (player == null)
            return 0f;
        SkillBase skill = player.SlotToSkillBase(slot);

        return skill.CooldownRatio;
    }

    public void BroadcastEvent(EBroadcastEventType eventType, float value)
    {
        OnBroadcastEvent?.Invoke(eventType, value);
    }
    #endregion

    #region Save & Load	
    public string Path { get { return Application.persistentDataPath + "/SaveData.json"; } }

    public void InitGame()
    {
        if (File.Exists(Path))
            return;

        Gold = 10000; // Test용
    }

    public void SaveGame()
    {

        // Item
        {
            SaveData.Items.Clear();
            foreach (var item in Managers.Inventory.AllItems)
                SaveData.Items.Add(item.SaveData);
        }

        string jsonStr = JsonUtility.ToJson(Managers.Game.SaveData);
        File.WriteAllText(Path, jsonStr);
        Debug.Log($"Save Game Completed : {Path}");
    }

    public bool LoadGame()
    {
        if (File.Exists(Path) == false)
            return false;

        string fileStr = File.ReadAllText(Path);
        GameSaveData data = JsonUtility.FromJson<GameSaveData>(fileStr);

        if (data != null)
            Managers.Game.SaveData = data;

        // Item
        {
            Managers.Inventory.Clear();

            foreach (ItemSaveData itemSaveData in data.Items)
            {
                Managers.Inventory.AddItem(itemSaveData);
            }
        }

        Debug.Log($"Save Game Loaded : {Path}");
        return true;
    }
    #endregion

    #region Item Event
    public void EquippedItemChange(EEquipSlotType equipSlot)
    {
        OnEEquippedItemChanged?.Invoke(equipSlot);
    }
    #endregion

    #region Action
    public event Action<Vector2> OnMoveDirChanged;
	public event Action<EJoystickState> OnJoystickStateChanged;
	public event Action<ESkillSlot> OnSkillSlotChanged;
	public event Action<EBroadcastEventType, float> OnBroadcastEvent;
    public event Action<EEquipSlotType> OnEEquippedItemChanged;
	#endregion
}
