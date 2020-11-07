using UnityEngine;
using System;

public class FogInterruption : MonoBehaviour
{
    public GameFlowManager GLManager;

    public System.Random rnd = new System.Random();
    public System.Diagnostics.Stopwatch watch = new System.Diagnostics.Stopwatch();
    public bool mFogOn = false;
    public int mFogTimeMin = 30000;
    public int mFogTimeMax = 60000;
    public int mNoFogTimeMin = 120000;
    public int mNoFogTimeMax = 180000;
    public int mCurrentFogTime = 3000;
    public int mCurrentNoFogTime = 12000;


    // Start is called before the first frame update
    void Start()
    {
        watch.Start();
    }

    // Update is called once per frame
    void Update()
    {
        if (!mFogOn)
        {
            if (watch.ElapsedMilliseconds > mCurrentNoFogTime)
            {
                RenderSettings.fog = true;
                RenderSettings.fogDensity = 0.15f;
                mFogOn = true;
                mCurrentFogTime = rnd.Next(mFogTimeMin, mFogTimeMax);
                watch.Reset();
                watch.Start();
            }
        }
        else
        {
            if (watch.ElapsedMilliseconds > mCurrentFogTime)
            {
                RenderSettings.fog = false;
                RenderSettings.fogDensity = 0.0f;
                mFogOn = false;
                mCurrentNoFogTime = rnd.Next(mNoFogTimeMin, mNoFogTimeMax);
                watch.Reset();
                watch.Start();
            }
        }

    }
}

