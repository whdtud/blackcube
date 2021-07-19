using UnityEngine;

using System.Collections;
using System.Collections.Generic;

public class EnemyFactory : MonoBehaviour, IGameStateListener
{
    public float SpawnIntervalTime = 0.1f;

    private int mSpawnedCount;
    private int mSpawnAmount;
    private int mEnemyCount;

    private List<EnemyBase> mEnemyList = new List<EnemyBase>();

    private const int DEFAULT_SPAWN_AMOUNT = 5;
    private const int SPAWN_HEIGHT = 3;

    void Awake()
    {
        GameController.Instance.EmFactory = this;
        GameController.Instance.GameStateListeners.Add(this);
    }

    public void OnChangeState(GameState prevState, GameState currentState)
    {
        var gameController = GameController.Instance;

        if (currentState == GameState.PLAY)
        {
            mEnemyCount = 0;
            mSpawnedCount = 0;
            mSpawnAmount = DEFAULT_SPAWN_AMOUNT;
        }
        else if (currentState == GameState.BOSS)
        {
            StopCoroutine(Co_SpawnEnemiesCycle(gameController.Map, gameController.Stage));
            gameController.Boss = SpawnBoss(gameController.Map.BossSpawnPoint.position);
        }
    }

    public void ClearEnemies()
    {
        foreach (var enemy in mEnemyList)
        {
            enemy.gameObject.SetActive(false);
        }

        mEnemyList.Clear();
    }

    public void OnEnemyDead(EnemyBase enemy)
    {
        mEnemyCount -= 1;

        mEnemyList.Remove(enemy);
    }

    void Update()
    {
        var gameController = GameController.Instance;

        if (gameController.CurrentState != GameState.PLAY)
            return;

        if (mEnemyCount <= 0)
            StartCoroutine(Co_SpawnEnemiesCycle(gameController.Map, gameController.Stage));
    }

    public IEnumerator Co_SpawnEnemiesCycle(MapController map, int stage)
    {
        while (mSpawnedCount < mSpawnAmount)
        {
            Vector3 position = map.GetRandomTilePosition();
            position.y += SPAWN_HEIGHT;

            SpawnEnemy(stage, position);

            yield return new WaitForSeconds(SpawnIntervalTime);
        }

        mSpawnedCount = 0;
        mSpawnAmount += DEFAULT_SPAWN_AMOUNT / 2;
    }

    private void SpawnEnemy(int stage, Vector3 position)
    {
        string name = CalcEnemyNameFromStage(stage);

        SpawnEnemy(name, position);
    }

    public EnemyBase SpawnEnemy(string name, Vector3 position)
    {
        var enemy = ResourceManager.Instance.SpawnEnemy(name);
        enemy.Init();
        enemy.SetPosition(position, Quaternion.identity);

        mEnemyCount++;
        mSpawnedCount++;

        mEnemyList.Add(enemy);

        return enemy;
    }

    private string CalcEnemyNameFromStage(int stage)
    {
        string name;

        switch (stage)
        {
            case 1:
                name = Defines.ENEMY_BASIC_NAME;
                break;
            case 2:
                name = Defines.ENEMY_BOMB_NAME;
                break;
            case 3:
                name = Defines.ENEMY_GIANT_NAME;
                break;
            case 4:
                if (mSpawnedCount < mSpawnAmount * 0.1f)
                    name = Defines.ENEMY_SHOOT_NAME;
                else
                    name = Defines.ENEMY_BASIC_NAME;
                break;
            case 5:
                if (mSpawnedCount < mSpawnAmount * 0.1f)
                    name = Defines.ENEMY_SHOOT_NAME;
                else if (mSpawnedCount < mSpawnAmount * 0.7f)
                    name = Defines.ENEMY_BASIC_NAME;
                else
                    name = Defines.ENEMY_BOMB_NAME;
                break;
            case 6:
                if (mSpawnedCount < mSpawnAmount * 0.1f)
                    name = Defines.ENEMY_SHOOT_NAME;
                else if (mSpawnedCount < mSpawnAmount * 0.7f)
                    name = Defines.ENEMY_BASIC_NAME;
                else
                    name = Defines.ENEMY_GIANT_NAME;
                break;
            case 7:
                if (mSpawnedCount < mSpawnAmount * 0.1f)
                    name = Defines.ENEMY_SHOOT_NAME;
                else if (mSpawnedCount < mSpawnAmount * 0.5f)
                    name = Defines.ENEMY_BASIC_NAME;
                else if (mSpawnedCount < mSpawnAmount * 0.8f)
                    name = Defines.ENEMY_BOMB_NAME;
                else
                    name = Defines.ENEMY_GIANT_NAME;
                break;
            case 8:
                if (mSpawnedCount < mSpawnAmount * 0.1f)
                    name = Defines.ENEMY_SHOOT_NAME;
                else if (mSpawnedCount < mSpawnAmount * 0.4f)
                    name = Defines.ENEMY_BASIC_NAME;
                else if (mSpawnedCount < mSpawnAmount * 0.7f)
                    name = Defines.ENEMY_BOMB_NAME;
                else
                    name = Defines.ENEMY_GIANT_NAME;
                break;
            default:
                name = Defines.ENEMY_BASIC_NAME;
                break;
        }

        return name;
    }

    private BossEnemy SpawnBoss(Vector3 position)
    {
        Vector3 spawnPosition = position;

        var boss = ResourceManager.Instance.SpawnEnemy(Defines.ENEMY_BOSS_NAME) as BossEnemy;
        boss.Init();
        boss.SetPosition(spawnPosition, Quaternion.identity);

        return boss;
    }
}
