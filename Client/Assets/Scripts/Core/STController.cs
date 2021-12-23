using UnityEngine;

public class STController<T> : MonoBehaviour where T : Component
{
    private static string[] mRootObjNames =
    {
        "GameManager",
        "ResourceManager"
    };

    private static T mInstance;
    public static T Instance
    {
        get 
        {
            if (mInstance == null)
            {
                T component = FindObjectOfType<T>();
                if (component == null)
                {
                    var go = new GameObject();
                    component = go.AddComponent<T>();

                    component.name = component.GetType().Name;
                    if (IsRootObj(component.name))
                        DontDestroyOnLoad(go);
                    else
                        component.transform.SetParent(GameManager.Instance.transform);
                }

                mInstance = component;
            }

            return mInstance;
        }
        private set { }
    }

    private static bool IsRootObj(string name)
    {
        for (int i = 0; i < mRootObjNames.Length; i++)
        {
            if (name == mRootObjNames[i])
                return true;
        }
        return false;
    }

    protected bool WasLoaded()
    {
        var components = FindObjectsOfType<T>();
        if (components.Length >= 2)
            return true;

        return false;
    }
}
