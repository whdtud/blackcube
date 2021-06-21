using UnityEngine;

public class TitleBackgroundTiles : MonoBehaviour {

    private TitleBackgroundTile[] mTiles;

    void Awake()
    {
        mTiles = GetComponentsInChildren<TitleBackgroundTile>();
    }

    public void Reset()
    {
        for (int i = 0; i < mTiles.Length; i++)
        {
            mTiles[i].Reset();
        }
    }

    public void PlayAnimation()
    {
        for (int i = 0; i < mTiles.Length; i++)
        {
            mTiles[i].PlayAnimation();
        }
    }
}
