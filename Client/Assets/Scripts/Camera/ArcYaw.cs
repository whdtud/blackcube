using UnityEngine;

public class ArcYaw : MonoBehaviour, IArc
{
    private Transform mTm;

    public float Degree
    {
        get
        {
            return mTm.localRotation.eulerAngles.y;
        }
        set
        {
            float degree = Mathf.Repeat(value, 360f);
            mTm.localRotation = Quaternion.Euler(0f, degree, 0f);
        }
    }

    void Awake()
    {
        mTm = transform;
    }
}
