using UnityEngine;

using System;

public class ESCHandler : MonoBehaviour
{
    private string mSceneName;
    private Func<bool> mEscFunc;

    public static void SetESCHandler(GameObject obj, Func<bool> escFunc)
    {
        string sceneName;
        if (obj.GetComponent<IPageView>() == null)
        {
            var parentPage = obj.GetComponentInParent<IPageView>();
            if (parentPage == null)
                return;

            sceneName = parentPage.GetGameObject().name;
        }
        else
        {
            sceneName = obj.name;
        }

        var handler = obj.GetComponent<ESCHandler>();
        if (handler == null)
            handler = obj.AddComponent<ESCHandler>();

        handler.mSceneName = sceneName;
        handler.mEscFunc = escFunc;
        SceneSwitchManager.Instance.AddEscHandler(sceneName, escFunc);
    }

    void OnEnable()
    {
        if (string.IsNullOrEmpty(mSceneName) || mEscFunc == null)
            return;

        SceneSwitchManager.Instance.AddEscHandler(mSceneName, mEscFunc);
    }

    void OnDisable()
    {
        SceneSwitchManager.Instance.RemoveEscHandler(mSceneName);
    }
}
