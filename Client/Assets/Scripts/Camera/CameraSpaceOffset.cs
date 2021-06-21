using UnityEngine;

public class CameraSpaceOffset : MonoBehaviour, IOffset
{
    private Transform mTm;

    public Vector3 Offset
    {
        get
        {
            return mTm.localPosition;
        }

        set
        {
            mTm.localPosition = value;
        }
    }

    void Awake()
    {
        mTm = transform;
    }
}
