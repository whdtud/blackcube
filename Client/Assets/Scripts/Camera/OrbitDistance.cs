using UnityEngine;

public class OrbitDistance : MonoBehaviour
{
    private Transform mTm;

    public float Slider
    {
        get
        {
            return mTm.localPosition.z;
        }
        set
        {
            mTm.localPosition = new Vector3(0f, 0f, value);
        }
    }

    void Awake()
    {
        mTm = transform;
    }
}
