using UnityEngine;
using UnityEngine.SceneManagement;

using System.Xml;
using System.Collections.Generic;

public class ResourceManager : STController<ResourceManager>
{
    private Dictionary<string, Object> mCachedDic = new Dictionary<string, Object>();
    private Dictionary<string, List<GameObject>> mObjectPool = new Dictionary<string, List<GameObject>>();

    void Awake()
    {
        if (WasLoaded())
            Destroy(gameObject);
        else
            DontDestroyOnLoad(gameObject);
    }

    private Object GetCachedData(string name)
    {
        Object obj;

        if (mCachedDic.TryGetValue(name, out obj))
            return obj;

        return null;
    }

    private void AddCachedData(string name, Object obj)
    {
        mCachedDic.Add(name, obj);
    }

    public T LoadData<T>(string dirPath, string name) where T : Object
    {
        return LoadData(dirPath, name) as T;
    }

    private Object LoadData(string dirPath, string name)
    {
        Object obj = GetCachedData(name);

        if (obj == null)
        {
            obj = Resources.Load(dirPath + name);

            AddCachedData(name, obj);
        }

        return obj;
    }

    private List<GameObject> PrepareObjectPool(string name)
    {
        List<GameObject> pool;

        if (mObjectPool.TryGetValue(name, out pool) == false)
        {
            pool = new List<GameObject>();
            mObjectPool.Add(name, pool);
        }

        return pool;
    }

    public GameObject SpawnObject(string dirPath, string name)
    {
        List<GameObject> pool = PrepareObjectPool(name);

        for (int i = 0; i < pool.Count; i++)
        {
            GameObject go = pool[i];

            if (go.activeSelf == true)
                continue;

            go.SetActive(true);

            return go;
        }

        var data = LoadData<GameObject>(dirPath, name);

        GameObject newGo = Instantiate(data);
        newGo.transform.SetParent(transform);
        pool.Add(newGo);

        return newGo;
    }

    public T SpawnObject<T>(string name)
    {
        return SpawnObject<T>("Object/", name);
    }

    public T SpawnObject<T>(string dir, string name)
    {
        GameObject go = SpawnObject(dir, name);

        return go.GetComponent<T>();
    }    

    public GameObject SpawnCharacter(string name)
    {
        return SpawnObject("Characters/", name);
    }

    public EnemyBase SpawnEnemy(string name)
    {
        return SpawnObject<EnemyBase>("Enemy/", name);
    }

    public GameObject SpawnEffect(string name)
    {
        return SpawnObject("Effect/", name);
    }

    public T SpawnEffect<T>(string name)
    {
        var effect = SpawnEffect(name);

        return effect.GetComponent<T>();
    }

    public static XmlDocument LoadXml(string name)
    {
        TextAsset ta = Resources.Load<TextAsset>(name);
        XmlDocument xmlDoc = new XmlDocument();
        xmlDoc.LoadXml(ta.text);

        return xmlDoc;
    }

    public static void LoadUIScene(string name)
    {
        SceneManager.LoadScene(name, LoadSceneMode.Additive);
    }

    public static void LoadLevelScene(string name)
    {
        SceneManager.LoadScene(name, LoadSceneMode.Additive);
    }

    public static void UnloadLevelScene(string name)
    {
        SceneManager.UnloadSceneAsync(name);
    }
}
