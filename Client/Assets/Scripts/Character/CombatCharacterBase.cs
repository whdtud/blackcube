using UnityEngine;

public abstract class CombatCharacterBase : CharacterBase
{
    public float MoveSpeed { get; set; }
    public float AttackSpeed { get; set; }
    public float AttackRange { get; set; }
    public float AttackPower { get; set; }
    public float AttackDelay { get; set; }
    public float NextAvailableAttackTime { get; set; }
    public float Hp { get; set; }
    public float Exp { get; set; }
    public float MaxHp { get; set; }
    public float HpPercent { get { return Hp / MaxHp; } }

    protected override void Awake()
    {
        base.Awake();

    }

    protected override void Update()
    {
        base.Update();

        if (Rigidbody.IsSleeping())
            Rigidbody.WakeUp();
    }

    public abstract void TryAttack();
    public abstract void OnDamaged(float attackPower);
    public abstract void OnDead(bool getPoint = false);

    public void Knockback(Vector3 dir, float power)
    {
        if (power <= 0f)
            return;

        Vector3 direction = new Vector3(dir.x, 0, dir.z);
        Rigidbody.velocity = Vector3.zero;
        Rigidbody.AddForce(direction * power, ForceMode.VelocityChange);
    }
}
