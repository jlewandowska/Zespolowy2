using UnityEngine;
using System;
using EventEntityNamespace;

public class FogInterruption : MonoBehaviour
{   

    private System.Random rnd = new System.Random();
    private System.Diagnostics.Stopwatch watch = new System.Diagnostics.Stopwatch();
    private bool mFogOn = false;
    private int mFogTimeMin = 30000;
    private int mFogTimeMax = 60000;
    private int mNoFogTimeMin = 90000;
    private int mNoFogTimeMax = 150000;
    private int mCurrentFogTime = 3000;
    private int mCurrentNoFogTime = 60000;
    private string mFogStartedType = "fog_start";
    private string mFogStoppedType = "fog_stop";
    GameFlowManager m_GameFlowManager;

    // Start is called before the first frame update
    void Start()
    {
        m_GameFlowManager = FindObjectOfType<GameFlowManager>();
        DebugUtility.HandleErrorIfNullFindObject<GameFlowManager, FogInterruption>(m_GameFlowManager, this);

        if (m_GameFlowManager.getRoundNumber() != 3 ) // 3rd round
            return;

        watch.Start();
    }

    // Update is called once per frame
    void Update()
    {
        if (m_GameFlowManager.getRoundNumber() != 3) // 3rd round
            return;

        if (!mFogOn)
        {
            if (watch.ElapsedMilliseconds > mCurrentNoFogTime) //20000) 
            {
                RenderSettings.fog = true;
                RenderSettings.fogDensity = 0.15f;
                mFogOn = true;
                mCurrentFogTime = rnd.Next(mFogTimeMin, mFogTimeMax);
                watch.Reset();
                watch.Start();
                GameFlowManager.eventsLog.Add(new EventEntity(mFogStartedType, "id"));
            }
        }
        else
        {
            if (watch.ElapsedMilliseconds > mCurrentFogTime) // 5000)
            {
                RenderSettings.fog = false;
                RenderSettings.fogDensity = 0.0f;
                mFogOn = false;
                mCurrentNoFogTime = rnd.Next(mNoFogTimeMin, mNoFogTimeMax);
                watch.Reset();
                watch.Start();
                GameFlowManager.eventsLog.Add(new EventEntity(mFogStoppedType, "id"));
            }
        }

    }
}

