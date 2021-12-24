using UnityEngine;

public class PlayerData
{
	public int Level = 1;
	public float MaxHp = 10f;
	public float mHp = 10f;
	public float RegenHp = 0f;
	public float mMaxXp = 10f;
	public float mXp = 0f;
	public float AttackPower = 1f;
	public float AttackDelay = 0.8f;
	public float AttackRange = 0.4f;
	public int KillScore = 0;
	public float MoveSpeed = 1f;
	public float DashDelay = 2f;
	public float SkillDelay = 8f;
	public float LaserCharge = 0.5f;
	public bool LaserRecoilless = false;

	public float HpPercent { get { return mHp / MaxHp; } }
	public float XpPercent { get { return mXp / mMaxXp; } }

	public float Hp
	{
		get { return mHp; }
		set { mHp = Mathf.Clamp(value, 0f, MaxHp); }
	}

	public float Xp
	{
		get { return mXp; }
		set
		{
			if (Level >= 20)
				return;

			mXp = value;
			if (mXp >= mMaxXp)
			{
				mXp = 0;
				Level++;
				mHp = Mathf.Min(mHp + (MaxHp * 0.5f), MaxHp);
				mMaxXp += mMaxXp * 0.5f;

				PlayerController.Instance.OnLevelUp();
			}
		}
	}

}

public class PlayerController : STController<PlayerController>
{
	public PlayerData Data { get; set; }
	public PlayerCharacter Character { get; set; }

	void Awake()
    {
		Data = new PlayerData();
    }

    public void Init()
    {
		Data = new PlayerData();

		Character.Init();
    }

	public void OnStartGame()
    {
		Spawn(MapController.Instance.GetPlayerSpawnPoint());
	}

    void Update()
    {
	}

	public void OnLevelUp()
	{
		GameController.Instance.ChangeState(GameState.PAUSE);

		Character.OnLevelUp();

		PageSystem.Instance.FadeInOutSystem.FadeOut(0.7f, 1f, true, 
			() => SceneSwitchManager.Instance.PushAdditivePage(UIPageKind.Page_Skill, null));
    }

	public void Spawn(Vector3 position)
    {
		GameObject go = ResourceManager.Instance.SpawnCharacter("Player");
		Character = go.GetComponent<PlayerCharacter>();
		Character.transform.position = position;
		Character.transform.rotation = Quaternion.identity;
		Character.Init();
	}

	public void Despawn()
    {
		Character.gameObject.SetActive(false);

		Character = null;
    }
}
