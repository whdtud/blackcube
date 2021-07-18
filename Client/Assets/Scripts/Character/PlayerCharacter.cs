using UnityEngine;

using System.Collections;

public class PlayerCharacter : CombatCharacterBase
{
	public Transform ShotPos;
	public ParticleSystem BoostEffect;
	public ParticleSystem LevelUpEffect;

	private Material mSkinMtrl;
	private SkillLaser mLaser;

	public PlayerData Data;

	private float mDashTime;
	private float mSkillTime;
	private float mRegenHpTime;
	private float mLastAttackTime;
	public float DashPercent { get { return mDashTime / Data.DashDelay; } }
	public float SkillPercent { get { return mSkillTime / Data.SkillDelay; } }
	public float AngleY { get; set; }
	public float BuffMoveSpeed { get; set; }
	public Vector3 MoveDir { get; set; }

	private const float DASH_POWER = 2f;

	protected override void Awake()
	{
		base.Awake();

		mSkinMtrl = Tm.Find("Body").GetComponent<Renderer>().material;

		Data = GameController.Instance.Player.Data;

		Animator.Rebind();
	}

    public override void Init()
    {
        base.Init();

		mDashTime = Data.DashDelay;
		mSkillTime = Data.SkillDelay;
		mRegenHpTime = 0f;
	}

    void FixedUpdate()
	{
		if (Rigidbody.velocity != Vector3.zero)
		{
			float vX = Mathf.Clamp(Rigidbody.velocity.x, -3.0f, 3.0f);
			float vY = Mathf.Clamp(Rigidbody.velocity.y, -6.0f, 6.0f);
			float vZ = Mathf.Clamp(Rigidbody.velocity.z, -3.0f, 3.0f);

			Rigidbody.velocity = new Vector3(vX, vY, vZ);
		}
	}

	protected override void Update()
    {
		base.Update();

		if (mDashTime < Data.DashDelay)
			mDashTime += Time.deltaTime;

		if (mSkillTime < Data.SkillDelay)
			mSkillTime += Time.deltaTime;

		mRegenHpTime += Time.deltaTime;
		if (mRegenHpTime >= 1f)
		{
			mRegenHpTime -= 1f;
			Data.Hp += Data.RegenHp;
		}

		Move();

		SelfRotateX();

		ParticleAutoRate();
	}

    public override void OnDamaged(float attackPower)
	{
		SoundManager.Instance.PlaySE(Defines.SE_DAMAGED);

		Data.Hp -= attackPower;

		if (Data.Hp > 0)
		{
			mSkinMtrl.color = Color.red;
			StartCoroutine(Co_BeWhite());
		}
		else
		{
			Animator.SetBool(Defines.ANI_DIE_NAME, true);
			OnDead();
		}
	}

    public override void OnDead(bool getPoint = false)
	{
		GameController.Instance.ChangeState(GameState.OVER);

		PageSystem.Instance.FadeInOutSystem.FadeOut(0.7f, 1f, false,
			() => SceneSwitchManager.Instance.PushAdditivePage(UIPageKind.Page_Pause, null));
	}

	// Call from Animation Clip (Animation Event)
	public void DieExplosion()
	{
		Animator.SetBool(Defines.ANI_DIE_NAME, false);

		Transform[] children = GetComponentsInChildren<Transform>();
		for (int i = 1; i < children.Length; i++)
		{
			children[i].gameObject.AddComponent<BoxCollider>();
			children[i].gameObject.AddComponent<Rigidbody>();
			children[i].parent = null;
			children[i].GetComponent<Rigidbody>().AddForce(new Vector3(Random.Range(0, 100), 0, Random.Range(0, 100)));
		}

		var effect = ResourceManager.Instance.SpawnEffect<ParticleController>(Defines.FX_EXPLOSION);
		effect.Init(Tm.position, Quaternion.identity);

	}

	private void Move()
	{
		float moveSpeed = Data.MoveSpeed * BuffMoveSpeed;

		Tm.position += MoveDir * Time.deltaTime * moveSpeed;
	}

	public void Dash()
	{
		mDashTime = 0f;

		Rigidbody.velocity = Vector3.zero;

		Rigidbody.AddForce(new Vector3(MoveDir.x * DASH_POWER, 2.5f, MoveDir.z * DASH_POWER), ForceMode.VelocityChange);

		SoundManager.Instance.PlaySE(Defines.SE_DASH_2);
	}

    public override void TryAttack()
    {
		PrepareAttack();

		mLastAttackTime += Time.deltaTime;

		if (mLastAttackTime > Data.AttackDelay)
		{
			mLastAttackTime = 0f;
			Attack();
		}
	}

    public void PrepareAttack()
    {
		Animator.SetBool(Defines.ANI_ATTACK_NAME, true);
	}

	public void Attack()
	{
		if (mLaser != null && mLaser.IsActive())
			return;

		float rx = Random.Range(-5.0f, 5.0f);
		float ry = Random.Range(-5.0f, 5.0f);
		Quaternion at = Tm.rotation * Quaternion.Euler(rx, ry, 0);

		SoundManager.Instance.PlaySE(Defines.SE_GUN_2);

		for (int i = 0; i < Data.AttackPower; i++)
		{
			Quaternion angle = at * Quaternion.Euler(0, i * 16 - 8 * (Data.AttackPower - 1), 0);
			var bullet = ResourceManager.Instance.SpawnObject<BulletBase>(Defines.OBJ_BULLET);
			bullet.Init(this, ShotPos.position, angle);
		}
	}

	public void AttackEnded()
	{
		Animator.SetBool(Defines.ANI_ATTACK_NAME, false);
	}

	public void BeginSkill()
	{
		Animator.SetBool(Defines.ANI_ATTACK_NAME, true);

		mLaser = ResourceManager.Instance.SpawnEffect<SkillLaser>(Defines.FX_LASER);
		mLaser.Init();
		SoundManager.Instance.PlaySE(Defines.SE_LASER_FORCE);
	}

	public void SkillFire()
	{
		mSkillTime = 0f;

		mLaser.Fire();

		SoundManager.Instance.PlaySE(Defines.SE_LASER_SHOT);

		Animator.SetBool(Defines.ANI_ATTACK_NAME, false);
	}

	public void AddRebound(float velocity)
    {
		Rigidbody.velocity = Vector3.zero;
		Rigidbody.AddRelativeForce(Vector3.back * velocity, ForceMode.VelocityChange);
	}

	public void JumpOnTile()
	{
		Vector3 direction = Tm.rotation * new Vector3(0, 3, 1.5f);
		Rigidbody.velocity = Vector3.zero;
		Rigidbody.AddForce(direction * 2.0f, ForceMode.VelocityChange);
	}

	public void Jump(Vector3 dir, float power)
	{
		Vector3 direction = new Vector3(dir.x, 1.5f, dir.z);
		Rigidbody.velocity = Vector3.zero;
		Rigidbody.AddForce(direction * power, ForceMode.VelocityChange);
	}

	private void SelfRotateX()
	{
		float angleX = Tm.position.y * 20.0f;

		if (Tm.position.y > 0 && Tm.position.y < 0.5f)
		{
			angleX = 0;
		}

		Tm.rotation = Quaternion.Euler(angleX, AngleY, 0);
	}

	private void ParticleAutoRate()
	{
		float velocity = Vector3.Distance(Vector3.zero, Rigidbody.velocity);
		float rate = Mathf.Min(velocity * 60.0f, 300f);

		var emission = BoostEffect.emission;
		emission.rateOverTime = rate;
	}

	public void OnLevelUp()
    {
		var effect = ResourceManager.Instance.SpawnEffect<ParticleController>(Defines.FX_LEVEL_UP);
		effect.Init(Tm.position, Quaternion.identity);
	}

	private IEnumerator Co_BeWhite()
	{
		yield return new WaitForSeconds(0.1f);

		mSkinMtrl.color = Color.white;
	}
}
