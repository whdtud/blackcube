using UnityEngine;

using System.Collections;

public class CameraController : MonoBehaviour {

    private Transform mTm;
	private Transform mTarget;

	public float mHeight = 3.0f;
	public float mDistance = 1.5f;
    public float mFrequency = 15f;
    public float mAmplitude;
    public float mDuration;

    private float mShakeTime;
    private bool mIsShaked;

    public static CameraController Instance { get; private set; }

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        mTm = transform;
	}

	void LateUpdate() 
    {
        if (mTarget == null)
            return;

        GameState gameState = GameController.Instance.CurrentState;

		if(gameState == GameState.OVER || gameState == GameState.QUIT)
        {
			if(mTarget.position.y <= -5f) {
				return;
			}		

            float targetX = mTarget.position.x;
            float targetY = mTarget.position.y + mHeight * 0.3f;
            float targetZ = mTarget.position.z - mDistance * 0.3f;

            float thisX = Mathf.Lerp(mTm.position.x, targetX, 0.05f);
            float thisY = Mathf.Lerp(mTm.position.z, targetZ, 0.05f);
            float thisZ = Mathf.Lerp(mTm.position.y, targetY, 0.05f);

            mTm.position = new Vector3(thisX, thisZ, thisY);
        }
        else
        {
			float targetX = mTarget.position.x;
			float targetY = mTarget.position.y + mHeight;
			float targetZ = mTarget.position.z - mDistance;

			float thisX = Mathf.Lerp(mTm.position.x, targetX, 0.05f);
			float thisY = Mathf.Lerp(mTm.position.y, targetY, 0.05f);
			float thisZ = Mathf.Lerp(mTm.position.z, targetZ, 0.05f);

            if (mIsShaked)
            {
                mShakeTime += Time.deltaTime * mDuration;

                thisX += Mathf.Sin(mShakeTime * mFrequency) * Mathf.Pow(0.5f, mShakeTime) * mAmplitude;

                if (mShakeTime >= 6f)
                {
                    mShakeTime = 0f;
                    mIsShaked = false;
                }
            }

            mTm.position = new Vector3(thisX, thisY, thisZ);
        }
	}

    public void SetTarget()
    {
        mTarget = GameController.Instance.Player.Character.transform;
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
