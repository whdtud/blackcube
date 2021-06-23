using UnityEngine;

using System;
using System.Xml;
using System.Collections;
using System.Collections.Generic;

public class MapController : MonoBehaviour
{
    private Dictionary<int, string> mStrMapDic = new Dictionary<int, string>();
    private Tile[] mTiles;

    public Transform PlayerSpawnPoint;
    public Vector3 BossSpawnPosition { get; private set; }

    private int mCurrentMapIndex;
    private int mLastMapIndex;

    private const int MAP_SIZE = 10;
    private const int BOSS_SPAWN_INDEX = 44;

    public readonly Color[] TILE_COLORS = new Color[] {
        Color.white,
        Color.red,
        Color.yellow,
        Color.blue,
        Color.green,
        Defines.TILE_PURPLE,
    };

    void Awake()
    {
        GameController.Instance.Map = this;
        
        mTiles = GetComponentsInChildren<Tile>();

        transform.rotation = Quaternion.Euler(Vector3.zero);

        TileComparer tileComparer = new TileComparer();
        Array.Sort(mTiles, tileComparer);

        transform.rotation = Quaternion.Euler(new Vector3(0, -45, 0));

        mCurrentMapIndex = 0;
        BossSpawnPosition = mTiles[BOSS_SPAWN_INDEX].transform.position;

        LoadMapData();
    }

    public void ChangeNextMap()
    {
        ChangeTiles();
    }

    private void ChangeTiles()
    {
        if (++mCurrentMapIndex >= mLastMapIndex)
            mCurrentMapIndex = 0;

        string currentMapData = mStrMapDic[mCurrentMapIndex];

        for (int i = 0; i < mTiles.Length; i++)
        {
            int colorIndex = currentMapData[i] - 48;

            mTiles[i].SetFixedColor(TILE_COLORS[colorIndex]);
        }
    }

    public Tile GetTile(int index)
    {
        if (index < 0 || index >= mTiles.Length)
            return null;

        return mTiles[index];
    }

    public Vector3 GetRandomTilePosition()
    {
        int ranX = UnityEngine.Random.Range(0, 10);
        int ranZ = UnityEngine.Random.Range(0, 10);
        int index = ranZ * 10 + ranX;

        Tile tile = GetTile(index);

        if (tile == null)
            return Vector3.zero;
        
        return tile.transform.position;
    }

    private void LoadMapData()
    {
        mStrMapDic.Clear();

        XmlDocument xmlDoc = ResourceManager.LoadXml("map");

        XmlNodeList mapNodeList = xmlDoc.SelectNodes("MapSet/Map");

        mLastMapIndex = mapNodeList.Count - 1;

        for (int i = 0; i < mapNodeList.Count; i++)
        {
            string strMapData = "";
            for (int j = 0; j < MAP_SIZE; j++)
            {
                strMapData += mapNodeList[i].SelectSingleNode(string.Format("Line{0}", j)).InnerText;
            }

            mStrMapDic.Add(i, strMapData);
        }
    }

    // 타일 정렬 IComparer
    private class TileComparer : IComparer
    {
        int IComparer.Compare(object x, object y)
        {
            Tile xValue = x as Tile;
            Tile yValue = y as Tile;
            int left = (int)(xValue.transform.position.x + xValue.transform.position.z * 10);
            int right = (int)(yValue.transform.position.x + yValue.transform.position.z * 10);
            return left - right;
        }
    }

}
