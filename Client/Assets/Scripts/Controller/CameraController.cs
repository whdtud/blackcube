using UnityEngine;

using System.Collections;

public class CameraController : MonoBehaviour, IGameStateListener
{
    private Transform mTm;
	private Transform mTarget;

    public float mHeight = 3.0f;
    public float mDistance = 1.5f;
    public float mFrequency = 15f;
    public float mAmplitude;
    public float mDuration;

    private float mZoomValue;
    private float mShakeTime;
    private bool mIsShaked;

    public static CameraController Instance { get; private set; }

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

        mTm = transform;
        GameController.Instance.GameStateListeners.Add(this);
    }

    public void OnChangeState(GameState prevState, GameState currentState)
    {
        if (currentState == GameState.READY)
            mZoomValue = 1f;
        else if (currentState == GameState.OVER)
            mZoomValue = 0.3f;
    }

    void LateUpdate() 
    {
        if (mTarget == null)
            return;

        float x = mTarget.position.x;
        float y = mTarget.position.y + mHeight * mZoomValue;
        float z = mTarget.position.z - mDistance * mZoomValue;

        Vector3 targetPosition = new Vector3(x, y, z);

        targetPosition = Vector3.Lerp(mTm.position, targetPosition, 0.05f);

        if (mIsShaked)
        {
            mShakeTime += Time.deltaTime * mDuration;

            targetPosition.x += Mathf.Sin(mShakeTime * mFrequency) * Mathf.Pow(0.5f, mShakeTime) * mAmplitude;

            if (mShakeTime >= 6f)
            {
                mShakeTime = 0f;
                mIsShaked = false;
            }
        }

        mTm.position = targetPosition;
	}

    public void SetTarget(Transform tm)
    {
        mTarget = tm;
    }

    public void Shake()
    {
        mIsShaked = true;
        mShakeTime = 0;
        mDuration = 8f;
        mAmplitude = 0.07f;
	}

    public void Shake(float duration, float amplitude)
    {
        mIsShaked = true;
        mShakeTime = 0;
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
