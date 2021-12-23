using UnityEngine;
using UnityEngine.SceneManagement;

using System.Collections.Generic;

public class SceneLoadManager : STController<SceneLoadManager>
{
    private Dictionary<string, Scene> mLoadedScene = new Dictionary<string, Scene>();

    private readonly string[] UI_SCENE_NAMES = new string[]
    {
        "UIBattleZone",
    };


    void Awake()
    {
        Init();
    }

    void Start()
    {
        SceneSwitchManager.Instance.ClearAndPushPage(UIPageKind.Page_Title, null);
    }

    private void Init()
    {
        var names = GetLoadedSceneNames();

        for (int i = 0; i < names.Length; i++)
        {
            CreateSceneObj(names[i], false);
        }

        for (int i = 0; i < UI_SCENE_NAMES.Length; i++)
        {
            CreateSceneObj(UI_SCENE_NAMES[i]);
        }

        PageSystem.Instance.transform.SetAsFirstSibling();
    }

    private string[] GetLoadedSceneNames()
    {
        var names = new string[SceneManager.sceneCount];

        for (int i = 0; i < SceneManager.sceneCount; i++)
        {
            names[i] = SceneManager.GetSceneAt(i).name;
        }

        return names;
    }

    private Scene CreateSceneObj(string sceneName, bool forceLoadScene = true)
    {
        var obj = new GameObject(sceneName, typeof(Scene));
        
        obj.transform.parent = transform;
        
        Scene scene = obj.GetComponent<Scene>();

        if (forceLoadScene)
            scene.LoadScene();

        mLoadedScene.Add(sceneName, scene);

        return scene;
    }

    public Scene GetScene(string sceneName)
    {
        Scene scene;
        if (mLoadedScene.TryGetValue(sceneName, out scene) == false)
        {
            scene = CreateSceneObj(sceneName);
        }
        return scene;
    }

    public void AddPageToScene(string sceneName, IPageView page)
    {
        Scene scene = GetScene(sceneName);
        scene.AddPage(page.GetPageKind(), page);

        SceneSwitchManager.Instance.AddKeepPage(page);
    }
}
