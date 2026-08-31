using UnityEngine;
using UnityEngine.Tilemaps;

#if UNITY_EDITOR
using System.IO;
using UnityEditor;
#endif

public class CustomTile : Tile
{
    public Define.EObjectType ObjectType;
    public int DataId;
    public string Name;
    public bool isStartPos = false;
}