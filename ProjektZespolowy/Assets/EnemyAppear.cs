using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EventEntityNamespace;

public class EnemyAppear : MonoBehaviour
{
    Renderer m_renderer;
    GameObject player;
    string visibilityStatus;
    bool seen = false;
    string EnemyAppeardType = "enemy_appread";

    // Start is called before the first frame update
    void Start()
    {
        
    }
    void FixedUpdate()
    {
        m_renderer = GetComponent<Renderer>();
        player = GameObject.Find("Player");

        if (m_renderer.isVisible)
        {
            if (!Physics.Linecast(gameObject.transform.position, player.transform.position + new Vector3(0, 2, 0)))
            { 
                if (!seen)
                {
                    seen = true;
                    GameFlowManager.eventsLog.Add(EventEntity.CreateInstance(EnemyAppeardType, "id"));
                }
            }
        }
    }

}
