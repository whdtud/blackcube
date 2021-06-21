using UnityEngine;

using Kit.Extend;

public class BombEnemy : EnemyBase
{
    protected override void Awake()
    {
        base.Awake();

        Type = CharacterType.ENMEY_BOMB;
    }

    public override void Init()
    {
        base.Init();

        int currentStage = GameController.Instance.Stage;
        AttackPower = currentStage;
        MoveSpeed = 3f + (currentStage - 1) * 0.2f;
        Hp = 1f;
        Exp = 2f;
        AttackRange = 0.5f;
    }

    protected override void Update()
    {
        base.Update();

        if (State != EnemyState.MOVE)
            return;

        if (GetDistanceVector2() > AttackRange)
        {
            MoveToTarget();
            LookAtTarget();
        }
        else
        {
            TryAttack();
        }
    }

    public override void TryAttack()
    {
        State = EnemyState.ATTACK;
        Animator.SetBool("Attack", true);
    }

    // Call from animation clip (Animation event)
    public void BombEnd()
    {
        CameraController.Instance.Shake();
        GameController.Instance.EmFactory.OnEnemyDead();

        if (GetDistanceVector3() < AttackRange * 1.3f)
        {
            Vector3 dir = Tm.position.Direction(TargetTm.position).normalized;
            GameController.Instance.Player.Character.Jump(dir, 1.5f);
            GameController.Instance.Player.Character.OnDamaged(AttackPower);
        }

        var effect = ResourceManager.Instance.SpawnEffect<ParticleController>(Defines.FX_EXPLOSION);
        effect.Init(Tm.position, Quaternion.identity);

        gameObject.SetActive(false);
    }
}
