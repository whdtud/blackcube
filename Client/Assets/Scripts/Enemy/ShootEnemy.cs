using UnityEngine;

public class ShootEnemy : EnemyBase
{
    protected override void Awake()
    {
        base.Awake();
    }

    public override void Init()
    {
        base.Init();

        int currentStage = GameController.Instance.Stage;
        Hp = 1 + (currentStage - 1) * 0.5f;
        AttackPower = 1 + (currentStage - 1) * 2f;
        Exp = 1f;
        MoveSpeed = 1f;
        AttackRange = 3f;
        NextAvailableAttackTime = 0f;

        SetParts();
    }

    protected override void Update()
    {
        base.Update();

        if (State == EnemyState.ATTACK)
            return;

        LookAtTarget();

        if (GetDistanceVector2() > AttackRange)
        {
            MoveToTarget();
        }
        else
        {
            if (GetDistanceVector3() > AttackRange)
                return;

            TryAttack();
        }
    }

    public override void TryAttack()
    {
        if (Time.time < NextAvailableAttackTime)
            return;

        State = EnemyState.ATTACK;
        NextAvailableAttackTime = Time.time + AttackDelay;
        Animator.SetBool(Defines.ANI_ATTACK_NAME, true);
    }

    // Call from animation clip (Animation event)
    public void Shoot()
    {
        var bullet = ResourceManager.Instance.SpawnObject<BulletBase>(Defines.OBJ_BULLET_BASIC);
        bullet.Init(this, Tm.position, Tm.rotation);
    }

    // Call from animation clip (Animation event)
    public void ShootEnd()
    {
        State = EnemyState.MOVE;

        Animator.SetBool(Defines.ANI_ATTACK_NAME, false);
    }
}
