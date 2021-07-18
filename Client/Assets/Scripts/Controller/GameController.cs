using UnityEngine;

using System.Collections;
using System.Collections.Generic;

public enum GameState 
{
    TITLE,
	READY,
	PLAY,
	PAUSE,
	OVER,
	BOSS,
	CLEAR,
    REST,
}

public class GameController : MonoBehaviour
{
    public GameState CurrentState { get; private set; }
    public GameState PrevState { get; private set; }
    public int Stage { get; private set; }
    public float GameTime { get; private set; }
    public MapController Map { get; set; }
    public EnemyFactory EmFactory { get; set; }

    public PlayerController Player;
    public BossEnemy Boss;

    public List<IGameStateListener> GameStateListeners = new List<IGameStateListener>();

    private const float STAGE_GOAL_TIME = 60f;
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
            case GameState.READY:
                {
                    GameTime = Mathf.Min(GameTime + Time.deltaTime * STAGE_READY_TIME_SPEED, STAGE_GOAL_TIME);

                    if (GameTime == STAGE_GOAL_TIME)
                        ChangeState(GameState.REST);
                }
                break;
            case GameState.PLAY:
                {
                    GameTime = Mathf.Max(0f, GameTime - Time.deltaTime);

                    if (GameTime == 0)
                        ChangeState(GameState.BOSS);
                }
                break;
        }
    }

    public void ChangeState(GameState state)
    {
        PrevState = CurrentState;
        CurrentState = state;

        if (state == GameState.PAUSE)
            return;

        foreach (var listeners in GameStateListeners)
        {
            listeners.OnChangeState(PrevState, state);
        }
    }

    public void ReturnToPrevState()
    {
        GameState temp = CurrentState;
        CurrentState = PrevState;
        PrevState = temp;
    }

    public void ClearBoss()
    {
        Stage++;
        ChangeState(GameState.READY);
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
        GameStateListeners.Clear();

        ChangeState(GameState.TITLE);
        Player.Despawn();
        ResourceManager.UnloadLevelScene("Map");
        SceneSwitchManager.Instance.ClearAndPushPage(UIPageKind.Page_Title, null);
    }
}
