using UnityEngine;

using System.Xml;
using System.Collections.Generic;

public class MapController : STController<MapController>, IGameStateListener
{
    public Map Map { get; private set; }

    private Dictionary<int, string> mStrMapDic = new Dictionary<int, string>();

    private int mCurrentMapIndex;

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
        GameController.Instance.GameStateListeners.Add(this);

        mCurrentMapIndex = -1;

        LoadMapDataFromXml();
    }

    public void OnStartGame()
    {
        Map = FindObjectOfType<Map>();
    }

    public void ResetMap()
    {
        mCurrentMapIndex = -1;

        Map.ChangeTiles(Color.white);
    }

    public void OnChangeState(GameState prevState, GameState currentState)
    {
        if (currentState == GameState.PLAY ||
            currentState == GameState.BOSS)
        {
            ChangeNextMap();
        }
    }

    private void ChangeNextMap()
    {
        mCurrentMapIndex++;

        if (mCurrentMapIndex >= mStrMapDic.Count)
            mCurrentMapIndex = 0;
        
        Map.ChangeTiles(mStrMapDic[mCurrentMapIndex]);
    }

    public Vector3 GetRandomTilePosition()
    {
        return Map.GetRandomTilePosition();
    }

    public Vector3 GetPlayerSpawnPoint()
    {
        return Map.PlayerSpawnPoint.position;
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
