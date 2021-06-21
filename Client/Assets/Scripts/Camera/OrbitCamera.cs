using UnityEngine;

public class OrbitCamera : MonoBehaviour
{
    public Transform CameraTm;
    public Transform ChaseTargetTm;

    public OrbitCameraData CameraData;

    public static OrbitCamera Instance;

    private const float Speed = 10f;

    void Awake()
    {
        Instance = this;
        Init();
    }

    private void Init()
    {
        
    }

    public void SetChaseTarget(Transform target)
    {
        ChaseTargetTm = target;
    }

    void LateUpdate()
    {
        UpdateCameraTransformByChaseTarget();
        UpdateCameraTransform();
    }

    private void UpdateCameraTransformByChaseTarget()
    {
        if (CameraTm == null || ChaseTargetTm == null)
            return;

        CameraData.Tm.position = ChaseTargetTm.position;
        CameraData.Tm.rotation = ChaseTargetTm.rotation;
    }

    private void UpdateCameraTransform()
    {
        if (CameraTm == null)
            return;

        CameraTm.position = CameraData.CameraPivot.Position;

        //float time = Time.deltaTime * Speed;
        //CameraTm.position = Vector3.Slerp(CameraTm.position, CameraData.CameraPivot.Position, time);
        //CameraTm.rotation = Quaternion.Slerp(CameraTm.rotation, CameraData.CameraPivot.
    }
}
