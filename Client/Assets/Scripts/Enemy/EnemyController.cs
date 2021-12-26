using UnityEngine;

using System.Collections;
using System.Collections.Generic;

public class EnemyController : STController<EnemyController>, IGameStateListener
{
    private List<EnemyBase> mEnemyList = new List<EnemyBase>();
    public BossEnemy Boss { get; private set; }

    private Coroutine mSpawnEnemiesCoroutine;

    public float SpawnIntervalTime = 0.1f;

    private int mLimitSpawnedCount;
    public int CurrentEnemyCount { get { return mEnemyList.Count; } }

    private const int DEFAULT_SPAWN_AMOUNT = 5;
    private const int SPAWN_HEIGHT = 3;

    void Awake()
    {
        GameController.Instance.GameStateListeners.Add(this);
    }

    public void Init()
    {
        Boss = null;
        mSpawnEnemiesCoroutine = null;

        foreach (var enemy in mEnemyList)
        {
            enemy.gameObject.SetActive(false);
        }
        mEnemyList.Clear();

        SoftInit();
    }

    private void SoftInit()
    {
        mLimitSpawnedCount = DEFAULT_SPAWN_AMOUNT;
    }

    public void OnChangeState(GameState prevState, GameState currentState)
    {
        if (currentState == GameState.PLAY)
        {
            SoftInit();
            mSpawnEnemiesCoroutine = StartCoroutine(Co_SpawnEnemies(GameController.Instance.Stage));
        }
        else if (currentState == GameState.BOSS)
        {
            if (mSpawnEnemiesCoroutine != null)
                StopCoroutine(mSpawnEnemiesCoroutine);

            SpawnBoss();
        }
    }

    public void OnEnemyDead(EnemyBase enemy)
    {
        mEnemyList.Remove(enemy);

        if (GameController.Instance.CurrentState == GameState.BOSS)
            return;

        if (CurrentEnemyCount <= 0)
            mSpawnEnemiesCoroutine = StartCoroutine(Co_SpawnEnemies(GameController.Instance.Stage));
    }

    private IEnumerator Co_SpawnEnemies(int stage)
    {
        var map = MapController.Instance;

        while (CurrentEnemyCount < mLimitSpawnedCount)
        {
            Vector3 position = map.GetRandomTilePosition();
            position.y += SPAWN_HEIGHT;

            string enemyName = EnemySpawnHelper.GetEnemyNameFromStage(stage, CurrentEnemyCount, mLimitSpawnedCount);
            SpawnEnemy(enemyName, position);

            yield return new WaitForSeconds(SpawnIntervalTime);
        }

        mLimitSpawnedCount += DEFAULT_SPAWN_AMOUNT / 2;
    }

    public EnemyBase SpawnEnemy(string name, Vector3 position)
    {
        var enemy = ResourceManager.Instance.SpawnEnemy(name);
        enemy.Init();
        enemy.SetPosition(position, Quaternion.identity);

        mEnemyList.Add(enemy);

        return enemy;
    }

    private void SpawnBoss()
    {
        var boss = ResourceManager.Instance.SpawnEnemy(Defines.ENEMY_BOSS_NAME) as BossEnemy;
        boss.Init();
        boss.SetPosition(MapController.Instance.GetBossSpawnPoint(), Quaternion.identity);

        Boss = boss;
    }
}
