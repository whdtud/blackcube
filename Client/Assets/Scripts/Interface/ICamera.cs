using UnityEngine;

public interface ICameraParts
{
    void Init(OrbitCameraData cameraData);
    void ResetToInit();
}

public interface IArc
{
    float Degree { get; set; }
}

public interface IOffset
{
    Vector3 Offset { get; set; }
}