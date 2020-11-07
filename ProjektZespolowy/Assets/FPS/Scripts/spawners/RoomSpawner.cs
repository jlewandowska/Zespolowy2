using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomSpawner : MonoBehaviour
{
    // Start is called before the first frame update

    public int roomEnemies = 3;
    public int spawnedEnemies = 0;
    EnemyManager m_EnemyManager;
    GameFlowManager m_GameFlowManager;
    public GameObject Enemy_HoverBot;
    public List<GameObject> spawners;

    public int getRoomEnemies()
    {
        return roomEnemies;
    }

    public void resetSpawnedEnemies()
    {
        spawnedEnemies = 0;
    }
    void Start()
    {
        m_EnemyManager = FindObjectOfType<EnemyManager>();
        DebugUtility.HandleErrorIfNullFindObject<EnemyManager, EnemyController>(m_EnemyManager, this);
        m_GameFlowManager = FindObjectOfType<GameFlowManager>();
        DebugUtility.HandleErrorIfNullFindObject<GameFlowManager, EnemyController>(m_GameFlowManager, this);
    }

    // Update is called once per frame
    void Update()
    {
        if (m_EnemyManager.getCanSpawnEnemy() && spawnedEnemies < roomEnemies)
        {
            int index = Random.Range(0, spawners.Count);
            GameObject spawner = spawners[index];
            GameObject a = Instantiate(Enemy_HoverBot, new Vector3(spawner.transform.position.x, spawner.transform.position.y, spawner.transform.position.z), Quaternion.identity) as GameObject;
            spawnedEnemies = spawnedEnemies + 1;
        }
    }
}
