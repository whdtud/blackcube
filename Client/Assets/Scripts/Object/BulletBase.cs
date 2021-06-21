using UnityEngine;

using System.Collections;

using Kit.Extend;

public class BulletBase : MonoBehaviour
{
    protected Transform mTm;
    protected CombatCharacterBase mOwner;

    protected float mPower;
    protected float mSpeed;
    protected float mLifeTime;

    protected virtual string GetCollisionFXName()
    {
        return Defines.FX_BULLET_FRAGMENT;
    }

    void Awake()
    {
        mTm = transform;
    }

    public void Init(CombatCharacterBase owner, Vector3 position, Quaternion rotation)
    {
        mOwner = owner;
        SetAbility(mOwner.Type);

        mTm.position = position;
        mTm.rotation = rotation;

        StartCoroutine(Co_OutRange());
    }

    void Update()
    {
        mTm.Translate(Vector3.forward * Time.deltaTime * mSpeed);
    }

    private void SetAbility(CharacterType ownerType)
    {
        switch (ownerType)
        {
            case CharacterType.PLAYER:
                {
                    mPower = 1f;
                    mSpeed = 6f;
                    mLifeTime = 0.4f;
                }
                break;
            case CharacterType.ENMEY_SHOOT:
                {
                    mPower = GameController.Instance.Stage * 0.2f;
                    mSpeed = 5f;
                    mLifeTime = 3f;
                }
                break;
            case CharacterType.ENMEY_BOSS:
                {
                    mPower = 1f;
                    mSpeed = 3f;
                    mLifeTime = 7f;
                }
                break;
        }
    }

    private void OutRange()
    {
        var effect = ResourceManager.Instance.SpawnEffect<ParticleController>(GetCollisionFXName());
        effect.Init(mTm.position, Quaternion.identity);
        gameObject.SetActive(false);
    }

    private IEnumerator Co_OutRange()
    {
        yield return new WaitForSeconds(mLifeTime);

        OutRange();
    }

    void OnTriggerEnter(Collider other)
    {
        var target = other.GetComponent<CombatCharacterBase>();
        if (target == null)
            return;

        if (target.Type == mOwner.Type)
            return;

        Vector3 dir = mTm.position.Direction(other.transform.position).normalized;
        target.Knockback(dir, CalcKnockbackPower(target.Type));
        target.OnDamaged(mPower);

        OutRange();
    }

    private float CalcKnockbackPower(CharacterType targetType)
    {
        float power = 0f;

        switch (targetType)
        {
            case CharacterType.PLAYER:
                {
                    if (mOwner.Type == CharacterType.ENMEY_BOSS)
                        power = 2f;
                    else
                        power = 1f;
                }
                break;
            case CharacterType.ENMEY_BASIC:
            case CharacterType.ENMEY_BOMB:
            case CharacterType.ENMEY_GIANT:
            case CharacterType.ENMEY_SHOOT:
                {
                    power = 2f;
                }
                break;
            case CharacterType.ENMEY_BOSS:
                {
                    power = 0f;
                }
                break;
        }

        return power;
    }
}
