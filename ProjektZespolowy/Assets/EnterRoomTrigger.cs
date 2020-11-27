using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnterRoomTrigger : MonoBehaviour
{
    GameFlowManager m_GameFlowManager;

    bool wasTriggered = false;

    [Header("Settings")]
    [SerializeField] LayerMask layersToDetect = 0;
    [SerializeField] GameObject doorToClose = null;
    [SerializeField] RoomSpawner spawnerToActivate = null;

    // Start is called before the first frame update
    void Start()
    {
        m_GameFlowManager = FindObjectOfType<GameFlowManager>();
        DebugUtility.HandleErrorIfNullFindObject<GameFlowManager, EnemyController>(m_GameFlowManager, this);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (((1 << other.gameObject.layer) & layersToDetect) == 0) { return; }

        if (!wasTriggered)
        {
            if (doorToClose != null)
            {
                var door = doorToClose.GetComponent<IDoor>();
                door.Close();

                if (spawnerToActivate != null)
                {
                    spawnerToActivate.activate();
                }

            }

            m_GameFlowManager.incRoomNumber();
            wasTriggered = true;
        }
    }
}
