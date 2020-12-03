using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Timers;
using EventEntityNamespace;
using System;
using System.Diagnostics;

public class AlarmBehaviour : MonoBehaviour
{
    public RawImage background;
    static float transparency;
    static bool isAlarm;
    static bool direction;
    public AudioSource audioData;
    string AlarmStartedType = "alarm_start";
    string AlarmStopedType = "alarm_stop";

    private static Timer alarmUpdateTimer;
    private System.Diagnostics.Stopwatch alarmDurationTimer;
    private System.Diagnostics.Stopwatch alarmWatch;
    private const int alarmDuration = 6000;
    private int interval;

    void Start()
    {
        transparency = (float)0.0f;
        isAlarm = false;
        direction = true;

        alarmUpdateTimer = new System.Timers.Timer(15);
        alarmUpdateTimer.Elapsed += new System.Timers.ElapsedEventHandler(t_Elapsed);
        alarmUpdateTimer.AutoReset = true;
        alarmUpdateTimer.Enabled = true;

        alarmDurationTimer = new Stopwatch();
        alarmWatch = new Stopwatch();
        alarmWatch.Start();
        System.Random random = new System.Random();
        interval = random.Next(30 ,  50) * 1000;
        audioData.volume = 0.2f;
    }
    static void t_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
    {
        if (isAlarm)
        {
            if (direction)
            {
                transparency += 0.002f;
                if (transparency > 0.1)
                    direction = false;
            }
            else
            {
                transparency -= 0.002f;
                if (transparency <= 0.0)
                    direction = true;
            }
        }
    }


    // Update is called once per frame
    void Update()
    {
        if (GameFlowManager.s_gameIsEnding == true)
        {
            if (isAlarm)
            {
                GameFlowManager.eventsLog.Add(EventEntity.CreateInstance(AlarmStopedType, "id"));
            }

            isAlarm = false;
            transparency = (float)0.0f;
            direction = true;

            alarmDurationTimer.Stop();
            audioData.Stop();

            var tempColor2 = background.color;
            tempColor2.a = transparency;
            background.color = tempColor2;
            return;
        }
          

        if (isAlarm)
        {
            // stop alarm
            if(alarmDurationTimer.ElapsedMilliseconds > alarmDuration)
            {
                isAlarm = false;
                transparency = (float)0.0f;
                direction = true;

                alarmDurationTimer.Stop();
                alarmWatch.Reset();
                alarmWatch.Start();


                audioData.Stop();

                GameFlowManager.eventsLog.Add(EventEntity.CreateInstance(AlarmStopedType, "id"));
            }
        }

        if (!isAlarm)
        {
            // launch alarm
            if (alarmWatch.ElapsedMilliseconds > interval)
            {
                isAlarm = true;
                transparency = (float)0.0f;
                direction = true;
                alarmDurationTimer.Reset();
                alarmDurationTimer.Start();
                audioData.Play();

                //GameFlowManager.eventsLog.Add(new EventEntity(AlarmStartedType, "id"));
                GameFlowManager.eventsLog.Add(EventEntity.CreateInstance(AlarmStartedType, "id"));
            }
        }

        var tempColor = background.color;
        tempColor.a = transparency;
        background.color = tempColor;
    }
}
