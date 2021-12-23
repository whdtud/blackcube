using UnityEngine;

public class ParticleController : MonoBehaviour {
	
	public float _lifeTime;
	public bool _follow;

	private float mTime;
	private Transform mTm;
	private Transform mOwnerTm;
	private ParticleSystem mParticleSystem;

	void Awake () 
	{
		mTm = transform;
		mOwnerTm = PlayerController.Instance.Character.Tm;;
		mParticleSystem = GetComponent<ParticleSystem>();
	}

    public void Init(Vector3 position, Quaternion rotation)
    {
		mTime = 0f;

		mParticleSystem.Clear();
		mParticleSystem.Play();

		mTm.position = position;
		mTm.rotation = rotation;
    }

	void Update () 
	{
		mTime += Time.deltaTime;

		if(mTime > _lifeTime) 
			gameObject.SetActive(false);

		if(_follow) 
			transform.position = mOwnerTm.position;
	}

}
