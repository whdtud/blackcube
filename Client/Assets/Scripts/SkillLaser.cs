using UnityEngine;
using System.Collections;

public class SkillLaser : MonoBehaviour {

	public ParticleSystem Energy;
	public Transform Ray;

	private Transform mTm;
	private Transform mShotPos;
	private BoxCollider mCollider;
	private PlayerCharacter mOwner;

	private float mSize = 1.0f;
	private float mMaxParticleSize;
	private float mPercent;
	private float mPower;
	private bool mUseRecoilless;
	private float mEnergySize;

	private const float DURTATION = 15.0f;

	private float EnergySize 
	{
		get
		{
			return mEnergySize;
		}
		set
        {
			mEnergySize = value;

			var main = Energy.main;
			main.startSize = mEnergySize;
        }
	}

	void Awake () 
	{
		mTm = transform;
		mCollider = mTm.GetComponent<BoxCollider>();

		var gameController = GameController.Instance.Player;
		mMaxParticleSize = gameController.Data.LaserCharge;
		mUseRecoilless = gameController.Data.LaserRecoilless;
	}

	public void Init()
    {
		mOwner = GameController.Instance.Player.Character;
		mShotPos = mOwner.ShotPos;
		mCollider.enabled = false;
		mSize = 1f;
		mPower = 0f;
		mPercent = 0f;
		EnergySize = 0.15f;
	}

	public bool IsActive()
    {
		if (gameObject.activeSelf == false)
			return false;

		if (mSize <= 0f)
			return false;

		return true;
    }

	public void Fire()
	{
		Ray.gameObject.SetActive(true);

		mCollider.enabled = true;

		if (mUseRecoilless == false)
			mOwner.AddRebound(mSize * 0.5f);
	}

    void Update () 
	{
		mTm.position = mShotPos.position;
		mTm.rotation = mOwner.Tm.rotation;

		if(Ray.gameObject.activeSelf == false)
		{
			if (EnergySize < mMaxParticleSize)
			{
				EnergySize += mMaxParticleSize * 0.01f;
				mSize = EnergySize * 15.0f;
				mPower = mSize * 8f;
				mPercent = mSize / DURTATION;
			}
		} 
		else
		{
			mSize -= mPercent;
			Ray.localScale = new Vector3(mSize * 0.25f, 10, 1);
			mCollider.size = new Vector3(mSize * 0.1f, 0.5f, 10);
			EnergySize -= mPercent * 0.05f;

			if (mSize <= 0)
            {
				Ray.gameObject.SetActive(false);
				gameObject.SetActive(false);
			}
		}
	}
	
	public void OnTriggerStay(Collider collider) 
	{
		var enemy = collider.GetComponent<EnemyBase>();
		
		if(enemy != null)
			enemy.OnDamaged(mPower * Time.deltaTime * 1000f);
	}

}
