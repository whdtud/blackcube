using UnityEngine;

public class PlayerController : STController<PlayerController>
{
	public PlayerData Data { get; set; }
	public PlayerCharacter Character { get; set; }

    public void Init()
    {
		Data = new PlayerData();
		Data.OnLevelUpAction = OnLevelUp;

		if (Character == null)
        {
			Spawn(MapController.Instance.GetPlayerSpawnPoint());
        }
		else
        {
			Character.SetPosition(MapController.Instance.GetPlayerSpawnPoint(), Quaternion.identity);
			Character.Init();
        }
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
		Character.SetPosition(position, Quaternion.identity);
		Character.Init();
	}

	public void Despawn()
    {
		Character.gameObject.SetActive(false);

		Character = null;
    }
}
