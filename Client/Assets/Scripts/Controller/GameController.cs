using UnityEngine;

using System.Collections;

public enum GameState {
    TITLE,
	READY,
	PLAY,
	PAUSE,
	OVER,
	BOSS,
	CLEAR,
    REST,
    QUIT,
}

public class GameController : MonoBehaviour
{
    public GameState CurrentState { get; private set; }
    public GameState PrevState { get; private set; }
    public int Stage { get; private set; }
    public float GameTime { get; private set; }
    public BossEnemy Boss { get; private set; }
    public MapController Map { get; set; }
    public EnemyFactory EmFactory { get; set; }

    public PlayerController Player;

    private const float STAGE_GOAL_TIME = 10f;
    private const float STAGE_READY_TIME_SPEED = 10f;

    public static GameController Instance { get; private set; }

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    void Update() 
    {
        switch (CurrentState)
        {
            case GameState.TITLE:
                {
                    // do nothing.
                }
                break;
            case GameState.READY:
                {
                    GameTime = Mathf.Min(GameTime + Time.deltaTime * STAGE_READY_TIME_SPEED, STAGE_GOAL_TIME);

                    if (GameTime < STAGE_GOAL_TIME)
                        return;

                    ChangeState(GameState.REST);

                    IPageView currentPage = SceneSwitchManager.Instance.GetCurrentPage();
                    PageBattleZone pageBattleZone = currentPage as PageBattleZone;
                    pageBattleZone.TimerAnimation(() =>
                    {
                        ChangeState(GameState.PLAY);

                        Map.SetStage(Stage);
                        EmFactory.Init();
                    });
                }
                break;
            case GameState.PLAY:
                {
                    GameTime = Mathf.Max(0f, Time.deltaTime);

                    SpawnEnemies();

                    SpawnBoss();
                }
                break;
            case GameState.PAUSE:
            case GameState.OVER:
            case GameState.BOSS:
            case GameState.REST:
                {
                    // do nothing.
                }
                break;
            case GameState.CLEAR:
                {
                    Stage++;

                    IPageView currentPage = SceneSwitchManager.Instance.GetCurrentPage();
                    PageBattleZone pageBattleZone = currentPage as PageBattleZone;
                    pageBattleZone.EndBoss();

                    ChangeState(GameState.READY);
                }
                break;
        }
    }

    public void ChangeState(GameState state)
    {
        PrevState = CurrentState;
        CurrentState = state;
    }

    public void ReturnToPrevState()
    {
        GameState temp = CurrentState;
        CurrentState = PrevState;
        PrevState = temp;
    }

    private void SpawnEnemies()
	{
        if (EmFactory.IsEmpty() == false)
            return;

        StartCoroutine(EmFactory.Co_SpawnEnemiesCycle(Map, Stage));
	}

    private void SpawnBoss()
    {
        if (GameTime > 0)
            return;

        GameTime = 0;
        ChangeState(GameState.BOSS);

        Boss = EmFactory.SpawnBoss(Map.BossSpawnPoint.position);

        IPageView currentPage = SceneSwitchManager.Instance.GetCurrentPage();
        PageBattleZone pageBattleZone = currentPage as PageBattleZone;
        pageBattleZone.StartBoss();
    }

    public IEnumerator Co_StartGame()
    {
        ResourceManager.LoadLevelScene("Map");

        yield return null;

        ChangeState(GameState.READY);
        GameTime = 0f;
        Stage = 1;
        Boss = null;

        Player.Spawn(Map.PlayerSpawnPoint.position);

        CameraController.Instance.SetTarget(Player.Character.Tm);

        SceneSwitchManager.Instance.PushPage(UIPageKind.Page_BattleZone, null);
    }

    public void ReturnToTitle()
    {
        ChangeState(GameState.TITLE);
        Player.Despawn();
        ResourceManager.UnloadLevelScene("Map");
        SceneSwitchManager.Instance.ClearAndPushPage(UIPageKind.Page_Title, null);
    }
}
