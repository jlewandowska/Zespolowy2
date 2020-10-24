using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EnemyManager : MonoBehaviour
{ PlayerCharacterController m_PlayerController;

    public List<EnemyController> enemies { get; private set; }
    public int numberOfEnemiesTotal { get; private set; } = 4;
    public int numberOfEnemiesRemaining => enemies.Count;
    public int killedEnemies = 0;

    public bool canSpawnEnemy = true;
    public UnityAction<EnemyController, int> onRemoveEnemy;

    private void Awake()
    {
        m_PlayerController = FindObjectOfType<PlayerCharacterController>();
        DebugUtility.HandleErrorIfNullFindObject<PlayerCharacterController, EnemyManager>(m_PlayerController, this);

        enemies = new List<EnemyController>();
    }

    public void RegisterEnemy(EnemyController enemy)
    {
        enemies.Add(enemy);

        canSpawnEnemy = false;
    }

    public void UnregisterEnemy(EnemyController enemyKilled)
    {
        int enemiesRemainingNotification = numberOfEnemiesTotal - killedEnemies - 1;

        if (onRemoveEnemy != null)
        {
            onRemoveEnemy.Invoke(enemyKilled, enemiesRemainingNotification);
        }

        // removes the enemy from the list, so that we can keep track of how many are left on the map
        enemies.Remove(enemyKilled);
        canSpawnEnemy = true;
        killedEnemies = killedEnemies + 1;
    }

        public int getNumberOfTotalEnemies()
        {
            return numberOfEnemiesTotal;
        }
}
