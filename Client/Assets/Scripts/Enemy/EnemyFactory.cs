using UnityEngine;

using System.Collections;

public class EnemyFactory : MonoBehaviour
{
    public float SpawnIntervalTime = 0.1f;

    private float mSpawnTime;
    private int mSpawnedCount;
    private int mSpawnAmount;
    private int mEnemyCount;

    private const int DEFAULT_SPAWN_AMOUNT = 5;
    private const int SPAWN_HEIGHT = 3;
    private const int SPAWN_BOSS_HEIGHT = 8;

    void Awake()
    {
        GameController.Instance.EmFactory = this;
    }

    public void Init()
    {
        mEnemyCount = 0;
        mSpawnedCount = 0;
        mSpawnAmount = DEFAULT_SPAWN_AMOUNT;
    }

    public bool IsEmpty()
    {
        return mEnemyCount <= 0;
    }

    public void OnEnemyDead()
    {
        mEnemyCount -= 1;
    }

    public IEnumerator Co_SpawnEnemiesCycle(MapController map, int stage)
    {
        while (mSpawnedCount < mSpawnAmount)
        {
            mSpawnTime += Time.deltaTime;

            if (mSpawnTime >= SpawnIntervalTime)
            {
                mSpawnTime = 0f;

                Vector3 position = map.GetRandomTilePosition();
                position.y += SPAWN_HEIGHT;

                SpawnEnemy(stage, position);
            }
        }

        mSpawnedCount = 0;
        mSpawnAmount += DEFAULT_SPAWN_AMOUNT;

        yield break;
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

    public BossEnemy SpawnBoss(Vector3 position)
    {
        Vector3 spawnPosition = position;
        spawnPosition.y += SPAWN_BOSS_HEIGHT;

        var boss = ResourceManager.Instance.SpawnEnemy(Defines.ENEMY_BOSS_NAME) as BossEnemy;
        boss.Init();
        boss.SetPosition(spawnPosition, Quaternion.identity);

        return boss;
    }
}
