using UnityEngine;

public class BossFSMSystem : FSMSystem<BossFSMSystem>
{
    public BossEnemy Owner { get; private set; }

    public void Init(BossEnemy boss)
    {
        Owner = boss;

        AddState(new BossIdleState());
        AddState(new BossMoveState());
        AddState(new BossAttackState());
        AddState(new BossJumpState());
        AddState(new BossDeadState());
    }
}

public class BossIdleState : FSMState<BossFSMSystem>
{
    public override int GetID()
    {
        return (int)EnemyState.IDLE;
    }

    public override void OnEnter(BossFSMSystem fsm, FSMState<BossFSMSystem> prevState) {}

    public override void OnExit(BossFSMSystem fsm, FSMState<BossFSMSystem> nextState)
    {
        var effect = ResourceManager.Instance.SpawnEffect<ParticleController>(Defines.FX_SHOCKWAVE);
        effect.Init(fsm.Owner.Tm.position, Quaternion.identity);

        CameraController.Instance.Shake(4f, 1f);
    }

    public override void OnUpdate(BossFSMSystem fsm) {}
}

public class BossMoveState : FSMState<BossFSMSystem>
{
    public override int GetID()
    {
        return (int)EnemyState.MOVE;
    }

    public override void OnEnter(BossFSMSystem fsm, FSMState<BossFSMSystem> prevState) 
    {
        fsm.Owner.Animator.Rebind();
    }

    public override void OnExit(BossFSMSystem fsm, FSMState<BossFSMSystem> nextState) {}

    public override void OnUpdate(BossFSMSystem fsm)
    {
        BossEnemy owner = fsm.Owner;

        owner.LookAtTarget();

        if (owner.GetDistanceVector2() > owner.AttackRange)
        {
            owner.MoveToTarget(owner.BuffMoveSpeed);
        }
        else
        {
            if (owner.GetDistanceVector3() < owner.AttackRange)
                fsm.ChangeState((int)EnemyState.ATTACK);
        }
    }
}

public class BossAttackState : FSMState<BossFSMSystem>
{
    public override int GetID()
    {
        return (int)EnemyState.ATTACK;
    }

    public override void OnEnter(BossFSMSystem fsm, FSMState<BossFSMSystem> prevState) {}
    public override void OnExit(BossFSMSystem fsm, FSMState<BossFSMSystem> nextState) {}

    public override void OnUpdate(BossFSMSystem fsm)
    {
        BossEnemy owner = fsm.Owner;

        owner.LookAtTarget();

        switch (owner.BossType)
        {
            case BossType.BERSERKER:
            case BossType.JUMPER:
                {
                    owner.MoveToTarget(owner.BuffMoveSpeed);
                }
                break;
            case BossType.SHOOTER:
            case BossType.SUMMONER:
                {
                    if (owner.GetDistanceVector3() > owner.AttackRange)
                    {
                        fsm.ChangeState((int)EnemyState.MOVE);
                    }
                    else
                    {
                        if (owner.NextAvailableAttackTime < Time.time)
                        {
                            owner.NextAvailableAttackTime = Time.time + (owner.AttackDelay * owner.AttackSpeed);
                            owner.Animator.SetBool(Defines.ANI_ATTACK_NAME, true);

                        }
                    }
                }
                break;
        }
    }
}

public class BossJumpState : FSMState<BossFSMSystem>
{
    private const float SMOOTH = 0.06f;

    public override int GetID()
    {
        return (int)EnemyState.JUMP;
    }

    public override void OnEnter(BossFSMSystem fsm, FSMState<BossFSMSystem> prevState) 
    {
    }

    public override void OnExit(BossFSMSystem fsm, FSMState<BossFSMSystem> nextState) { }

    public override void OnUpdate(BossFSMSystem fsm)
    {
        BossEnemy owner = fsm.Owner;

        owner.Tm.position = Vector3.Lerp(owner.Tm.position, owner.JumpDest, SMOOTH);

        if (owner.NextAvailableAttackTime < Time.time)
        {
            owner.NextAvailableAttackTime = Time.time + owner.AttackDelay;
            owner.JumpDest = owner.TargetTm.position;
            owner.Rigidbody.AddForce(new Vector3(0, 1200.0f, 0));
        }
    }
}

public class BossDeadState : FSMState<BossFSMSystem>
{
    public override int GetID()
    {
        return (int)EnemyState.DEAD;
    }

    public override void OnEnter(BossFSMSystem fsm, FSMState<BossFSMSystem> prevState)
    {
        BossEnemy owner = fsm.Owner;

        PlayerController.Instance.Character.Data.Xp = 100000;
        PlayerController.Instance.Character.Data.KillScore += 1;

        var effect = ResourceManager.Instance.SpawnEffect<ParticleController>(Defines.FX_FRAGMENT);
        effect.Init(owner.Tm.position, Quaternion.identity);

        CameraController.Instance.Shake(4f, 1f, 2.5f);

        owner.Animator.SetTrigger(Defines.ANI_DIE_NAME);
    }

    public override void OnExit(BossFSMSystem fsm, FSMState<BossFSMSystem> nextState) {}
    public override void OnUpdate(BossFSMSystem fsm) {}
}