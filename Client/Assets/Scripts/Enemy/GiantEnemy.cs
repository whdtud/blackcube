using UnityEngine;

using Kit.Extend;

public class GiantEnemy : EnemyBase
{
    protected override void Awake()
    {
        base.Awake();

        Type = CharacterType.ENMEY_GIANT;
    }

    public override void Init()
    {
        base.Init();

        int currentStage = GameController.Instance.Stage;
        Hp = 8 + currentStage * 2f;
        AttackPower = currentStage * 2f;
        Exp = 1f;
        MoveSpeed = 0f;
        AttackRange = 0f;

        SetParts();
    }

    protected override void Update()
    {
        base.Update();

        if (IsBattleAble() == false)
            return;

        if (State != EnemyState.MOVE)
            return;

        LookAtTarget();
    }

    public override void TryAttack()
    {
        // do nothing.
    }

    public override void OnCollisionEnter(Collision collision)
    {
        base.OnCollisionEnter(collision);

        if (collision.transform.tag == "Player")
        {
            Vector3 dir = Tm.position.Direction(TargetTm.position).normalized;
            PlayerController.Instance.Character.Jump(dir, 2f);
            PlayerController.Instance.Character.OnDamaged(AttackPower);
        }
    }
}
