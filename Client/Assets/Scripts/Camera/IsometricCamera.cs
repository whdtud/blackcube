using System.Collections;
using UnityEngine;

public class IsometricCamera : MonoBehaviour
{
    private Transform mTm;
    private Transform mTarget;

    public float mHeight = 3.0f;
    public float mDistance = 1.5f;
    public float mFrequency = 15f;
    public float mAmplitude;
    public float mDuration;

    public float ZoomValue { get; set; }
    public float ShakeTime { get; set; }
    public bool IsShaked { get; set; }

    void Awake()
    {
        mTm = transform;
    }

    void LateUpdate()
    {
        if (mTarget == null)
            return;

        float x = mTarget.position.x;
        float y = mTarget.position.y + mHeight * ZoomValue;
        float z = mTarget.position.z - mDistance * ZoomValue;

        Vector3 targetPosition = new Vector3(x, y, z);

        targetPosition = Vector3.Lerp(mTm.position, targetPosition, 0.05f);

        if (IsShaked)
        {
            ShakeTime += Time.deltaTime * mDuration;

            targetPosition.x += Mathf.Sin(ShakeTime * mFrequency) * Mathf.Pow(0.5f, ShakeTime) * mAmplitude;

            if (ShakeTime >= 6f)
            {
                ShakeTime = 0f;
                IsShaked = false;
            }
        }

        mTm.position = targetPosition;
    }

    public void SetTarget(Transform tm)
    {
        mTarget = tm;
    }

    public void SetPosition(Vector3 position)
    {
        mTm.position = position;
    }

    public void Shake()
    {
        IsShaked = true;
        ShakeTime = 0;
        mDuration = 8f;
        mAmplitude = 0.07f;
    }

    public void Shake(float duration, float amplitude)
    {
        IsShaked = true;
        ShakeTime = 0;
        mDuration = duration;
        mAmplitude = amplitude;
    }

    public void Shake(float duration, float amplitude, float delay)
    {
        StartCoroutine(Co_Shake(duration, amplitude, delay));
    }

    public IEnumerator Co_Shake(float duration, float amplitude, float delay)
    {
        yield return new WaitForSeconds(delay);

        Shake(duration, amplitude);
    }
}
