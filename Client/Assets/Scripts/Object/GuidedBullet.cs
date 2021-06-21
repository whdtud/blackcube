using UnityEngine;

public class GuidedBullet : BulletBase
{
	private Transform mTargetTm;
	private Vector3 mLastDestPosition;

	private float mGuidedElapsedTime = 0f;

	private const float GUIDED_INTERVAL = 0.3f;

	protected override string GetCollisionFXName()
	{
		return Defines.FX_BOSS_BULLET_FRAGMENT;
	}

	public void SetTarget(Transform targetTm)
    {
		mTargetTm = targetTm;
		mLastDestPosition = mTargetTm.position;
	}

	void Update()
	{
		LookAtTarget();

		mTm.Translate(Vector3.forward * Time.deltaTime * mSpeed);

		mGuidedElapsedTime += Time.deltaTime;

		if (mGuidedElapsedTime > GUIDED_INTERVAL)
		{
			mLastDestPosition = mTargetTm.position;

			mGuidedElapsedTime = 0f;
		}
	}

	void LookAtTarget()
	{
		Vector3 targetDirection = mLastDestPosition - mTm.position;
		targetDirection.y = 0;
		targetDirection.Normalize();

		float rotationAngle = Vector3.Dot(Vector3.forward, targetDirection);
		rotationAngle = Mathf.Acos(rotationAngle);
		if (mTm.position.x > mLastDestPosition.x) 
			rotationAngle *= -1;

		Vector2 playerVector2 = new Vector2(mLastDestPosition.x, mLastDestPosition.z);
		Vector2 enemyVector2 = new Vector2(mTm.position.x, mTm.position.z);

		float x = Vector2.Distance(playerVector2, enemyVector2);
		float y = mLastDestPosition.y - mTm.position.y;
		float angleX = -Mathf.Atan2(y, x);

		Vector3 rotationVec = new Vector3(angleX * Mathf.Rad2Deg, rotationAngle * Mathf.Rad2Deg, 0);

		mTm.rotation = Quaternion.Lerp(mTm.rotation, Quaternion.Euler(rotationVec), 0.04f);
	}

	void OnTriggerEnter(Collider other)
	{
		var target = other.GetComponent<PlayerCharacter>();
		if (target == null)
			return;

		if (target == mOwner)
			return;

		Vector3 dir = (mTargetTm.position - mTm.position).normalized;
		target.Jump(dir, 2.0f);
		target.OnDamaged(mPower);

		var effect = ResourceManager.Instance.SpawnEffect<ParticleController>(GetCollisionFXName());
		effect.Init(mTm.position, Quaternion.identity);
		gameObject.SetActive(false);
	}
}
