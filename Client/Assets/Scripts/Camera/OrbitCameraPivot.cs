using UnityEngine;

public class OrbitCameraPivot : MonoBehaviour
{
    private Transform mTm;

    public Vector3 Position
    {
        get { return mTm.position; }
    }

    void Awake()
    {
        mTm = transform;
    }
}
