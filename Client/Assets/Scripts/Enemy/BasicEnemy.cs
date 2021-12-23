using UnityEngine;

using Kit.Extend;

public class BasicEnemy : EnemyBase
{
    protected override void Awake()
    {
        base.Awake();

        Type = CharacterType.ENMEY_BASIC;
    }

    public override void Init()
    {
        base.Init();

        int currentStage = GameController.Instance.Stage;
        Hp = currentStage;
        AttackPower = 1 + (currentStage - 1) * 4;
        AttackDelay = 2f;
        Exp = 1f;
        MoveSpeed = 1f;
        AttackRange = 0.6f;
        NextAvailableAttackTime = 0f;

        SetParts();
    }

    protected override void Update()
    {
        base.Update();

        if (IsBattleAble() == false)
            return;

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

        NextAvailableAttackTime = Time.time + AttackDelay;

        State = EnemyState.ATTACK;
        Animator.SetBool(Defines.ANI_ATTACK_NAME, true);
    }

    // Call from animation clip (Animation event)
    public void AttackEnd()
    {
        State = EnemyState.MOVE;

        Animator.SetBool(Defines.ANI_ATTACK_NAME, false);
    }

    public override void OnCollisionEnter(Collision collision)
    {
        base.OnCollisionEnter(collision);

        if (State == EnemyState.ATTACK && collision.transform.tag == "Player")
        {
            Vector3 dir = Tm.position.Direction(TargetTm.position).normalized;
            PlayerController.Instance.Character.Jump(dir, 1f);
            PlayerController.Instance.Character.OnDamaged(AttackPower);
        }
    }
}
