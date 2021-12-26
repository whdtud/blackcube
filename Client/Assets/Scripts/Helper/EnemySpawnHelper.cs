using UnityEngine;

public class EnemySpawnHelper : MonoBehaviour
{
    public static string GetEnemyNameFromStage(int stage, int spawnedCount, int limitSpawnedCount)
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
                if (spawnedCount < limitSpawnedCount * 0.1f)
                    name = Defines.ENEMY_SHOOT_NAME;
                else
                    name = Defines.ENEMY_BASIC_NAME;
                break;
            case 5:
                if (spawnedCount < limitSpawnedCount * 0.1f)
                    name = Defines.ENEMY_SHOOT_NAME;
                else if (spawnedCount < limitSpawnedCount * 0.7f)
                    name = Defines.ENEMY_BASIC_NAME;
                else
                    name = Defines.ENEMY_BOMB_NAME;
                break;
            case 6:
                if (spawnedCount < limitSpawnedCount * 0.1f)
                    name = Defines.ENEMY_SHOOT_NAME;
                else if (spawnedCount < limitSpawnedCount * 0.7f)
                    name = Defines.ENEMY_BASIC_NAME;
                else
                    name = Defines.ENEMY_GIANT_NAME;
                break;
            case 7:
                if (spawnedCount < limitSpawnedCount * 0.1f)
                    name = Defines.ENEMY_SHOOT_NAME;
                else if (spawnedCount < limitSpawnedCount * 0.5f)
                    name = Defines.ENEMY_BASIC_NAME;
                else if (spawnedCount < limitSpawnedCount * 0.8f)
                    name = Defines.ENEMY_BOMB_NAME;
                else
                    name = Defines.ENEMY_GIANT_NAME;
                break;
            case 8:
                if (spawnedCount < limitSpawnedCount * 0.1f)
                    name = Defines.ENEMY_SHOOT_NAME;
                else if (spawnedCount < limitSpawnedCount * 0.4f)
                    name = Defines.ENEMY_BASIC_NAME;
                else if (spawnedCount < limitSpawnedCount * 0.7f)
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
}
