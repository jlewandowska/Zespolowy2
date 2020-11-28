using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EnemyManager : MonoBehaviour
{
    PlayerCharacterController m_PlayerController;

    public List<EnemyController> enemies { get; private set; }
    public int numberOfEnemiesTotal  = 1;
    public int numberOfEnemiesRemaining => enemies.Count;
    public int enemiesOnMap = 0;
    public int killedEnemies = 0;
    public bool canSpawnEnemy = true;
    public GameObject[] roomRespawns;
    
    public UnityAction<EnemyController, int> onRemoveEnemy;

    private void Awake()
    {
        m_PlayerController = FindObjectOfType<PlayerCharacterController>();
        DebugUtility.HandleErrorIfNullFindObject<PlayerCharacterController, EnemyManager>(m_PlayerController, this);

        enemies = new List<EnemyController>();
        roomRespawns = GameObject.FindGameObjectsWithTag("RoomSpawner");

        int enemiesTotal = 0;
        foreach ( GameObject respawn in roomRespawns)
        {
          enemiesTotal = enemiesTotal + respawn.GetComponent<RoomSpawner>().getRoomEnemies();
        }

        if(enemiesTotal != 0){
            numberOfEnemiesTotal = enemiesTotal;
        }
    }

    public void RegisterEnemy(EnemyController enemy)
    {
        if (enemy.shouldAddToEnemiesCount())
        {
             numberOfEnemiesTotal = numberOfEnemiesTotal + 1;
        }
        enemies.Add(enemy);
        if (enemy.shouldBlockSpawn()) {
        canSpawnEnemy = false;
        }

    }

    public void UnregisterEnemy(EnemyController enemyKilled)
    {
        int enemiesRemainingNotification = numberOfEnemiesTotal - killedEnemies - 1;

        if (onRemoveEnemy != null)
        {
            onRemoveEnemy.Invoke(enemyKilled, enemiesRemainingNotification);
        }
        enemiesOnMap = enemiesOnMap - 1;
        // removes the enemy from the list, so that we can keep track of how many are left on the map
        enemies.Remove(enemyKilled);
        if (enemyKilled.shouldBlockSpawn()) {
        canSpawnEnemy = true;
        }
        killedEnemies = killedEnemies + 1;
    }

    public int getEnemiesOnMapAmount(){
        return enemiesOnMap;
    }

    public bool getCanSpawnEnemy(){
        return canSpawnEnemy;
    }
}
