using UnityEngine;

public class SoundManager : MonoBehaviour 
{
    private AudioSource mBGM;
    private AudioSource[] mEffects = new AudioSource[MAX_EFFECT_COUNT];

    private int mCurrentEffectId;

    private const int MAX_EFFECT_COUNT = 10;

    public static SoundManager Instance { get; private set; }

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            Init();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Init()
    {
        mBGM = gameObject.AddComponent<AudioSource>();

        for (int i = 0; i < mEffects.Length; i++)
        {
            mEffects[i] = gameObject.AddComponent<AudioSource>();
        }
    }

    public void PlayBGM(string name)
    {
        var clip = ResourceManager.Instance.LoadData<AudioClip>("Sound/BGM/", name);
        if (clip == null)
            return;

        mBGM.Stop();
        mBGM.clip = clip;
        mBGM.Play();
    }

    public void PlaySE(string name)
    {
        var clip = ResourceManager.Instance.LoadData<AudioClip>("Sound/Effect/", name);
        if (clip == null)
            return;

        int id = mCurrentEffectId++ % mEffects.Length;
        mEffects[id].clip = clip;
        mEffects[id].Play();
    }
}
