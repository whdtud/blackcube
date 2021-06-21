using UnityEngine;

public class UICanvas : MonoBehaviour
{
    public string SceneName;

    public static UICanvas Instance { get; private set; }

    void Awake()
    {
        if (Instance == null)
            Instance = this;

        OnSceneContentLoaded();
    }

    void Start()
    {
        if (Instance != this)
            Destroy(gameObject);
    }

    public void OnSceneContentLoaded()
    {
        IPageView[] pages = GetComponentsInChildren<IPageView>(true);
        for (int i = 0; i < pages.Length; i++)
        {
            SceneLoadManager.Instance.AddPageToScene(SceneName, pages[i]);

            if (Instance != this)
                Instance.AddPageObj(pages[i]);
        }
    }

    private void AddPageObj(IPageView page)
    {
        var obj = page.GetGameObject();
        obj.SetActive(false);

        var tempScale = obj.transform.localScale;

        obj.transform.SetParent(Instance.transform);

        obj.transform.localScale = tempScale;

        RectTransform rt = obj.transform as RectTransform;
        rt.offsetMin = Vector2.zero;
        rt.offsetMax = Vector2.zero;
    }
}
