using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Timers;

using System;
using System.Diagnostics;

public class AlarmBehaviour : MonoBehaviour
{
    public RawImage background;
    static float transparency;
    static bool isAlarm;
    static bool direction;
    public AudioSource audioData;

    private static Timer alarmUpdateTimer;
    private System.Diagnostics.Stopwatch alarmTimer;

    void Start()
    {
        transparency = (float)0.0f;
        isAlarm = false;
        direction = true;

        alarmUpdateTimer = new System.Timers.Timer(15);
        alarmUpdateTimer.Elapsed += new System.Timers.ElapsedEventHandler(t_Elapsed);
        alarmUpdateTimer.AutoReset = true;
        alarmUpdateTimer.Enabled = true;

        alarmTimer = new Stopwatch();
    }
    static void t_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
    {
        if (isAlarm)
        {
            if (direction)
            {
                transparency += 0.01f;
                if (transparency > 0.8)
                    direction = false;
            }
            else
            {
                transparency -= 0.01f;
                if (transparency <= 0.18)
                    direction = true;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (GameFlowManager.s_gameIsEnding == true)
        {
            isAlarm = false;
            transparency = (float)0.0f;
            direction = true;

            alarmTimer.Stop();
            audioData.Stop();

            var tempColor2 = background.color;
            tempColor2.a = transparency;
            background.color = tempColor2;
            return;
        }
          

        if (isAlarm)
        {
            // stop alarm
            if(alarmTimer.ElapsedMilliseconds > 8000)
            {
                isAlarm = false;
                transparency = (float)0.0f;
                direction = true;
                
                alarmTimer.Stop();
                audioData.Stop();
            }
        }

        if (!isAlarm)
        {
            System.Random random = new System.Random();
            int randomNumber = random.Next(0, 500);

            // launch alarm
            if (randomNumber == 100)
            {
                isAlarm = true;
                transparency = (float)0.0f;
                direction = true;
                alarmTimer.Reset();
                alarmTimer.Start();
                audioData.Play();
            }
        }

        var tempColor = background.color;
        tempColor.a = transparency;
        background.color = tempColor;
    }
}
