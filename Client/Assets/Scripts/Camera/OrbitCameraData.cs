using UnityEngine;

public class OrbitCameraData : MonoBehaviour
{
    public ArcYaw Yaw;
    public ArcPitch Pitch;
    public OrbitDistance Distance;
    public OrbitCameraPivot CameraPivot;

    [HideInInspector]
    public Transform Tm;

    void Awake()
    {
        Tm = transform;
    }

    void Start()
    {
        Pitch.Degree = 15f;
        Distance.Slider = -10f;
    }
}
