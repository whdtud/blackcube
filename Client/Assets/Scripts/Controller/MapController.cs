using UnityEngine;

using System.Xml;
using System.Collections.Generic;

public class MapController : MonoBehaviour, IGameStateListener
{
    public Tiles mTiles;
    public Transform PlayerSpawnPoint;
    public Transform BossSpawnPoint;

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
        GameController.Instance.Map = this;
        GameController.Instance.GameStateListeners.Add(this);

        mCurrentMapIndex = -1;

        LoadMapDataFromXml();
    }

    public void OnChangeState(GameState currentState)
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
        
        mTiles.ChangeTiles(mStrMapDic[mCurrentMapIndex]);
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
