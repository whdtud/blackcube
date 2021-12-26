using UnityEngine;

public class Map : MonoBehaviour
{
    public Tiles Tiles;
    public Transform PlayerSpawnPoint;
    public Transform BossSpawnPoint;

    public void Init()
    {
        ChangeTiles(Color.white);
    }

    private void ChangeTiles(Color color)
    {
        Tiles.ChangeTiles(color);
    }

    public void ChangeTiles(string strMapData)
    {
        Tiles.ChangeTiles(strMapData);
    }

    public Vector3 GetRandomTilePosition()
    {
        return Tiles.GetRandomTilePosition();
    }
}
