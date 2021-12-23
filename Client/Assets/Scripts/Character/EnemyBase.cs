using UnityEngine;

using System.Collections;

using Kit.Extend;

public enum EnemyState
{
    IDLE,
    MOVE,
    ATTACK,
    JUMP,
    DEAD,
}

public abstract class EnemyBase : CombatCharacterBase
{
    protected Transform mBodyTm;
    protected Material mSkinMtrl;
    public Transform TargetTm { get; set; }
    public EnemyState State { get; set; }

    protected override void Awake()
    {
        base.Awake();

        mBodyTm = Tm.Find("Body");
        mSkinMtrl = mBodyTm.GetComponent<Renderer>().material;
    }

    public override void Init()
    {
        base.Init();

        TargetTm = PlayerController.Instance.Character.Tm;

        State = EnemyState.IDLE;

        SetBlackSkin();
    }

    protected override void Update()
    {
        base.Update();
    }

    protected bool IsBattleAble()
    {
        GameState gameState = GameController.Instance.CurrentState;
        if (gameState == GameState.OVER)
            return false;

        return true;
    }

    public override void OnDamaged(float attackPower)
    {
        Hp -= attackPower;

        if (Hp > 0)
        {
            mSkinMtrl.color = Color.gray;

            StartCoroutine(Co_SetBlackSkin());
        }
        else
        {
            OnDead(true);
        }
    }

    public override void OnDead(bool getPoint = false)
    {
        if (getPoint)
        {
            PlayerController.Instance.Data.KillScore += 1;
            PlayerController.Instance.Data.Xp += Exp;
        }

        CameraController.Instance.Shake();

        EnemyController.Instance.OnEnemyDead(this);

        ShowDeadEffect();

        gameObject.SetActive(false);
    }

    private void ShowDeadEffect()
    {
        var effect = ResourceManager.Instance.SpawnEffect<ParticleController>(Defines.FX_FRAGMENT);
        effect.Init(Tm.position, Quaternion.identity);
    }

    public void MoveToTarget()
    {
        MoveToTarget(1f);
    }

    public void MoveToTarget(float buffSpeed)
    {
        Vector3 targetDirection = Tm.position.Direction(TargetTm.position);
        targetDirection.y = 0;
        targetDirection.Normalize();
        Tm.position += targetDirection * MoveSpeed * buffSpeed * Time.deltaTime;
    }

    public void LookAtTarget()
    {
        Vector3 targetDirection = Tm.position.Direction(TargetTm.position);
        targetDirection.y = 0;
        targetDirection.Normalize();

        float rotationAngle = Vector3.Dot(Vector3.forward, targetDirection);
        rotationAngle = Mathf.Acos(rotationAngle);
        if (Tm.position.x > TargetTm.position.x) 
            rotationAngle *= -1;

        Vector3 rotationVec = new Vector3(0, rotationAngle * Mathf.Rad2Deg, 0);
        Tm.rotation = Quaternion.Lerp(Tm.rotation, Quaternion.Euler(rotationVec), 0.1f);
    }


    public float GetDistanceVector2()
    {
        Vector2 position = new Vector2(Tm.position.x, Tm.position.z);
        Vector2 targetPosition = new Vector2(TargetTm.position.x, TargetTm.position.z);

        return Vector2.Distance(position, targetPosition);
    }

    public float GetDistanceVector3()
    {
        return Vector3.Distance(Tm.position, TargetTm.position) - 0.05f;
    }

    public IEnumerator Co_SetBlackSkin()
    {
        yield return new WaitForSeconds(0.1f);

        SetBlackSkin();
    }

    private void SetBlackSkin()
    {
        if (Type == CharacterType.ENMEY_BOSS)
            return;

        mSkinMtrl.color = new Color(0.22f, 0.22f, 0.22f);
    }

    //protected bool 

    protected void SetParts()
    {
        switch (GameController.Instance.Stage)
        {
            case 4:
            case 5:
            case 6:
                {
                    Transform head = mBodyTm.Find("Head");
                    if (head != null)
                        head.gameObject.SetActive(true);
                }
                break;
            case 7:
                {
                    Transform chin = mBodyTm.Find("Chin");
                    if (chin == null)
                        chin = Tm.Find("Chin");

                    if (chin != null)
                        chin.gameObject.SetActive(true);
                }
                break;
            case 8:
                {
                    Transform backHead = mBodyTm.Find("BackHead");
                    if (backHead != null)
                        backHead.gameObject.SetActive(true);
                }
                break;
            default:
                {
                    Transform head = mBodyTm.Find("Head");
                    if (head != null)
                        head.gameObject.SetActive(false);

                    Transform chin = mBodyTm.Find("Chin");
                    if (chin == null)
                        chin = Tm.Find("Chin");

                    if (chin != null)
                        chin.gameObject.SetActive(false);

                    Transform backHead = mBodyTm.Find("BackHead");
                    if (backHead != null)
                        backHead.gameObject.SetActive(false);
                }
                break;
        }
    }

    public virtual void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Tile")
            State = EnemyState.MOVE;
    }
}
