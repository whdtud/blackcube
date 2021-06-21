using UnityEngine;

using Kit.Extend;

public enum BossType
{
    BERSERKER,
    SUMMONER,
    SHOOTER,
    JUMPER,
    MAX,
}

public class BossEnemy : EnemyBase
{
    private BossFSMSystem mFsm;

    public BossType BossType { get; set; }
    public Vector3 JumpDest { get; set; }
    public float BuffAttackPower { get; set; }
    public float BuffMoveSpeed { get; set; }

    private float mSuperModeEndTime;
    private float mNextSuperModeTime;

    private const int MIN_SUMMON_COUNT = 2;
    private const int MAX_SUMMON_COUNT = 6;
    private const float MAX_BUFF_POWER = 6f;
    private const float MAX_BUFF_SPEED = 3f;
    private const float SUPER_MODE_DELAY = 6f;
    private const float SUPER_MODE_KEEP_TIME = 3f;

    protected override void Awake()
    {
        base.Awake();

        Type = CharacterType.ENMEY_BOSS;

        mFsm = gameObject.AddComponent<BossFSMSystem>();
        mFsm.Init(this);
    }

    public override void Init()
    {
        base.Init();

        mFsm.ChangeState((int)EnemyState.IDLE);

        int stageValue = GameController.Instance.Stage - 1;

        BossType = (BossType)(stageValue % (int)BossType.MAX);

        switch (BossType)
        {
            case BossType.BERSERKER:
                {
                    Hp = 50 + stageValue * 10;
                    MoveSpeed = 0.5f;
                    AttackRange = 0.6f;
                    AttackPower = 1f;
                    AttackDelay = 1f;
                }
                break;
            case BossType.SUMMONER:
                {
                    Hp = 40 + stageValue * 10;
                    MoveSpeed = 0.5f;
                    AttackRange = 6f;
                    AttackPower = 1f;
                    AttackDelay = 2f;
                }
                break;
            case BossType.SHOOTER:
                {
                    Hp = 40 + stageValue * 10;
                    MoveSpeed = 1f;
                    AttackRange = 5f;
                    AttackPower = 1f;
                    AttackDelay = 1.5f;
                }
                break;
            case BossType.JUMPER:
                {
                    Hp = 40 + stageValue * 10;
                    MoveSpeed = 1f;
                    AttackRange = 12f;
                    AttackPower = 3f;
                    AttackDelay = 2f;
                }
                break;
            default:
                break;
        }

        MaxHp = Hp;
        Exp = 9999f;
        AttackSpeed = 1f;
        BuffMoveSpeed = 1f;
        NextAvailableAttackTime = 0f;
        mSuperModeEndTime = 0f;
        mNextSuperModeTime = Time.time + SUPER_MODE_DELAY;

        SetParts();
    }

    protected override void Update()
    {
        base.Update();

        if (mNextSuperModeTime < Time.time)
        {
            mSuperModeEndTime = Time.time + SUPER_MODE_KEEP_TIME;
            mNextSuperModeTime = mSuperModeEndTime + SUPER_MODE_DELAY;

            switch (BossType)
            {
                case BossType.BERSERKER:
                    break;
                case BossType.SUMMONER:
                case BossType.SHOOTER:
                    AttackSpeed = 0.5f;
                    break;
                case BossType.JUMPER:
                    AttackSpeed = 1f;
                    break;
            }
        }

        if (mSuperModeEndTime < Time.time)
        {
            mNextSuperModeTime = Time.time + SUPER_MODE_DELAY;
            AttackSpeed = 1f;
        }
    }

    public override void TryAttack()
    {
    }

    public override void OnDamaged(float attackPower)
    {
        base.OnDamaged(attackPower);

        if (BossType != BossType.BERSERKER)
            return;

        if (BuffAttackPower < MAX_BUFF_POWER)
            BuffAttackPower += MAX_BUFF_POWER / 40f;
        if (BuffMoveSpeed < MAX_BUFF_SPEED)
            BuffMoveSpeed += MAX_BUFF_SPEED / 40f;
    }

    public override void OnDead(bool getPoint = false)
    {
        mFsm.ChangeState((int)EnemyState.DEAD);
    }

    public override void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Tile")
        {
            if (mFsm.GetCurrentState() == (int)EnemyState.IDLE ||
                mFsm.GetCurrentState() == (int)EnemyState.JUMP)
            {
                mFsm.ChangeState((int)EnemyState.MOVE);
            }
        }

        var player = collision.gameObject.GetComponent<PlayerCharacter>();
        if (player == null)
            return;

        float cameraDuration = 4f;
        float cameraAmplitude = 0.25f;
        float knockBackPower = 2f;
        switch (BossType)
        {
            case BossType.BERSERKER:
                {
                    knockBackPower = Mathf.Clamp((AttackPower + BuffAttackPower) * 0.5f, 2f, 5f);
                    cameraAmplitude = knockBackPower * 0.15f;
                }
                break;
            case BossType.JUMPER:
                {
                    knockBackPower = 3f;
                    cameraAmplitude = 0.5f;
                }
                break;
        }

        CameraController.Instance.Shake(cameraDuration, cameraAmplitude);
        SoundManager.Instance.PlaySE(Defines.SE_EXPLOSION_2);

        Vector3 dir = Tm.position.Direction(player.Tm.position).normalized;

        player.Jump(dir, knockBackPower);
        player.OnDamaged(AttackPower + BuffAttackPower);

        BuffAttackPower = 0f;
        BuffMoveSpeed = 1f;
    }

    // Call from the animation clip (Animation Event)
    public void Shoot()
    {
        switch (BossType)
        {
            case BossType.SHOOTER:
                {
                    var bullet = ResourceManager.Instance.SpawnObject<GuidedBullet>(Defines.OBJ_BULLET_BOSS);
                    bullet.Init(this, Tm.position, Quaternion.identity);
                    bullet.SetTarget(GameController.Instance.Player.Character.Tm);
                }
                break;
            case BossType.SUMMONER:
                {
                    int summonCount = Random.Range(MIN_SUMMON_COUNT, MAX_SUMMON_COUNT + 1);

                    for (int i = 0; i < summonCount; i++)
                    {
                        float x = Random.Range(-0.5f, 0.5f);
                        float y = Random.Range(-0.5f, 0.5f);
                        float z = Random.Range(-0.5f, 0.5f);
                        Vector3 position = Tm.position + new Vector3(x, y, z);

                        x = Random.Range(-1.0f, 1.0f);
                        y = Random.Range(3.0f, 5.0f);
                        z = Random.Range(-1.0f, 1.0f);
                        Vector3 direction = Tm.rotation * new Vector3(x, y, z);

                        var bombEnemy = GameController.Instance.EmFactory.SpawnEnemy(Defines.ENEMY_BOMB_NAME, position);
                        bombEnemy.Rigidbody.AddForce(direction * 2.0f, ForceMode.VelocityChange);
                    }
                }
                break;
        }
    }

    // Call from the animaion clip (Animation event)
    public void ShootEnd()
    {

    }

    // Call from the animaion clip (Animation event)
    public void DestroyBoss()
    {
        GameController.Instance.ChangeState(GameState.CLEAR);

        Animator.Rebind();

        var effect = ResourceManager.Instance.SpawnEffect<ParticleController>(Defines.FX_BOSS_EXPLOSION);
        effect.Init(Tm.position, Quaternion.identity);

        gameObject.SetActive(false);
    }
}
