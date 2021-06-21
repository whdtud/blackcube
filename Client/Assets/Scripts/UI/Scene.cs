using UnityEngine;

using System.Collections.Generic;

public class Scene : MonoBehaviour
{
    private Dictionary<UIPageKind, IPageView> mGameObjectDic = new Dictionary<UIPageKind, IPageView>();

    public void LoadScene()
    {
        ResourceManager.LoadUIScene(gameObject.name);
    }

    public void AddPage(UIPageKind kind, IPageView page)
    {
        if (mGameObjectDic.ContainsKey(kind) == false)
            mGameObjectDic.Add(kind, page);
    }
}
