using UnityEngine;

public class PageSystem : MonoBehaviour {

    public FadeInOutSystem FadeInOutSystem;
    public GameObject IgnoreTouchArea;

    public static PageSystem Instance;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void SetIgnoreTouch(bool active)
    {
        IgnoreTouchArea.SetActive(active);
    }

	// Update is called once per frame
	void Update () {
		
	}
}
