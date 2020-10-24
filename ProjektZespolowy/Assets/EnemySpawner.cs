using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
   
    EnemyManager m_EnemyManager;
    GameFlowManager m_GameFlowManager;
    public GameObject Enemy_HoverBot;
    public List<Vector3> spawners = new List<Vector3>();
    private Vector3 spawn1 = new Vector3(63, 2, -218);
private Vector3 spawn2 = new Vector3(58, 0, -187);
    private Vector3 spawn3 = new Vector3(96, 0, -156);

    void Start()
    {
        m_EnemyManager = FindObjectOfType<EnemyManager>();
        DebugUtility.HandleErrorIfNullFindObject<EnemyManager, EnemyController>(m_EnemyManager, this);
        m_GameFlowManager = FindObjectOfType<GameFlowManager>();
        DebugUtility.HandleErrorIfNullFindObject<GameFlowManager, EnemyController>(m_GameFlowManager, this);
        spawners.Add(spawn1);
        spawners.Add(spawn2);
        spawners.Add(spawn3);
    }

    void Update()
    {
        if (m_EnemyManager.canSpawnEnemy && m_EnemyManager.getNumberOfTotalEnemies() != (m_EnemyManager.numberOfEnemiesRemaining + m_EnemyManager.killedEnemies))
        {
            Vector3 v = spawners[Random.Range(0, spawners.Count)];
            Debug.Log("INSTANTIATE: " + v);
            GameObject a = Instantiate(Enemy_HoverBot, new Vector3(v.x, v.y, v.z), Quaternion.identity) as GameObject;
            //a.transform.position = v;
        }
    }

    private void spawnEnemy()
    {
        GameObject a = Instantiate(Enemy_HoverBot) as GameObject;
        a.transform.position = spawners[Random.Range(0, spawners.Count)];
    }

    IEnumerator enemyWave()
    {
        while (m_EnemyManager.getNumberOfTotalEnemies() < 4)
        {
            yield return new WaitForSeconds(0);
            spawnEnemy();
        }
    }
}
