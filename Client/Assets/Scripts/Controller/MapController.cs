using UnityEngine;

using System;
using System.Xml;
using System.Collections;
using System.Collections.Generic;

public class MapController : MonoBehaviour
{
    public Tiles mTiles;
    public Transform PlayerSpawnPoint;
    public Transform BossSpawnPoint;

    private Dictionary<int, string> mStrMapDic = new Dictionary<int, string>();

    private const int MAP_SIZE = 10;

    public static readonly Color[] TILE_COLORS = new Color[] {
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
        
        LoadMapDataFromXml();
    }

    public void SetStage(int stage)
    {
        int mapIndex = stage - 1;
        if (mapIndex >= mStrMapDic.Count)
            mapIndex = 0;
        
        mTiles.ChangeTiles(mStrMapDic[mapIndex]);
    }

    public Vector3 GetRandomTilePosition()
    {
        return mTiles.GetRandomTilePosition();
    }

    private void LoadMapDataFromXml()
    {
        mStrMapDic.Clear();

        XmlDocument xmlDoc = ResourceManager.LoadXml("map");

        XmlNodeList mapNodeList = xmlDoc.SelectNodes("MapSet/Map");
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
}
