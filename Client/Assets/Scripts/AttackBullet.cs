using UnityEngine;
using System.Collections;

using Kit.Extend;

public class AttackBullet : MonoBehaviour {
	
	private Transform mTm;

	private float mLifeTime = 0.4f;

	private const float SPEED = 6.0f;
	private const float POWER = 1.0f;

	void Awake () 
	{
		mTm = transform;
	}
	
	public void Init(Vector3 position, Quaternion rotation)
    {
		StopAllCoroutines();
		
		mTm.position = position;
		mTm.rotation = rotation;

		mLifeTime = PlayerController.Instance.Data.AttackRange;

		StartCoroutine(Co_OutRange());
	}

	void Update () 
	{
		mTm.Translate(Vector3.forward * Time.deltaTime * SPEED);
	}

	void OnTriggerEnter(Collider collider) 
	{
		var other = collider.GetComponent<EnemyBase>();
		if (other == null)
			return;

		var effect = ResourceManager.Instance.SpawnEffect<ParticleController>(Defines.FX_BULLET_FRAGMENT);
		effect.Init(mTm.position, Quaternion.identity);
		gameObject.SetActive(false);

		other.OnDamaged(POWER);

		if (other.Type == CharacterType.ENMEY_BOSS)
			return;

        Vector3 dir = mTm.position.Direction(collider.transform.position).normalized;
        other.Knockback(dir, 2.0f);
	}

	private IEnumerator Co_OutRange()
    {
		yield return new WaitForSeconds(mLifeTime);

		var effect = ResourceManager.Instance.SpawnEffect<ParticleController>(Defines.FX_BULLET_FRAGMENT);
		effect.Init(mTm.position, Quaternion.identity);
		gameObject.SetActive(false);
	}
}