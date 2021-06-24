using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class Tiles : MonoBehaviour
{
    private Tile[] mTiles;


    void Awake()
    {
        mTiles = GetComponentsInChildren<Tile>();

        transform.rotation = Quaternion.Euler(Vector3.zero);

        TileComparer tileComparer = new TileComparer();
        Array.Sort(mTiles, tileComparer);

        transform.rotation = Quaternion.Euler(new Vector3(0, -45, 0));
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

    public void ChangeTiles(string strMapData)
    {
        for (int i = 0; i < mTiles.Length; i++)
        {
            int colorIndex = strMapData[i] - 48;

            mTiles[i].SetFixedColor(MapController.TILE_COLORS[colorIndex]);
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
