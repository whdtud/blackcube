using UnityEngine;

public class GameManager : STController<GameManager> {

    void Awake()
    {
        if (WasLoaded())
            Destroy(gameObject);
        else
            DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        PlatformHelper.Activate();
    }
}
