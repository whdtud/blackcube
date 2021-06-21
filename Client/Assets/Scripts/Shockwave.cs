using UnityEngine;

public class Shockwave : MonoBehaviour 
{
	private SphereCollider mCollider;

	private const int LIMIT_SIZE = 8;
	private const float SPEED = 0.1f;

	void Awake () 
	{
		mCollider = gameObject.GetComponent<SphereCollider>();
	}
	
	void Update () 
	{
		if (mCollider.radius < LIMIT_SIZE)
			mCollider.radius += SPEED;
	}

    public void OnTriggerEnter(Collider other)
    {
		var enemy = other.GetComponent<EnemyBase>();
		if (enemy == null)
			return;

		if (enemy.Type == CharacterType.ENMEY_BOSS)
			return;

		enemy.OnDead();
    }
}
