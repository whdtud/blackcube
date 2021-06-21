using UnityEngine;

public enum CharacterType
{
    PLAYER,
    ENMEY_BASIC,
    ENMEY_BOMB,
    ENMEY_GIANT,
    ENMEY_SHOOT,
    ENMEY_BOSS,
}

public abstract class CharacterBase : MonoBehaviour
{
    public Transform Tm { get; set; }
    public Rigidbody Rigidbody { get; set; }
    public Collider Collider { get; set; }
    public Animator Animator { get; set; }
    public CharacterType Type {get; protected set;}

    protected virtual void Awake()
    {
        Tm = transform;
        Rigidbody = GetComponent<Rigidbody>();
        Collider = GetComponent<Collider>();
        Animator = GetComponent<Animator>();
    }

    public virtual void Init()
    {
        Animator.Rebind();
    }

    protected virtual void Update()
    {
    }

    public void SetPosition(Vector3 position, Quaternion rotation)
    {
        Tm.position = position;
        Tm.rotation = rotation;
    }
}
