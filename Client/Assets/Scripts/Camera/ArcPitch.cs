using UnityEngine;

public class ArcPitch : MonoBehaviour, IArc
{
    private Transform mTm;

    public float Degree
    {
        get
        {
            return mTm.localRotation.eulerAngles.x;
        }
        set
        {
            float degree = Mathf.Repeat(value, 360f);
            mTm.localRotation = Quaternion.Euler(degree, 0f, 0f);
        }
    }

    void Awake()
    {
        mTm = transform;
    }
}
