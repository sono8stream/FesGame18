using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScriptSwitcher : MonoBehaviour
{
    [SerializeField]
    TimeSpan[] timeSpans;
    [SerializeField]
    MonoBehaviour[] targetScripts;

    TimeCounter timeCounter;
    Counter spanCounter;
    bool on;

    private void Start()
    {
        Array.Sort(timeSpans);
        timeCounter = new TimeCounter(timeSpans[0].beginSec);
        spanCounter = new Counter(timeSpans.Length);
        on = false;
        SwitchScripts();
    }

    // Update is called once per frame
    void Update()
    {
        if (timeCounter.OnLimit())
        {
            on = !on;
            SwitchScripts();
            if (on)
            {
                
            }
            else
            {
                if (spanCounter.Count())
                {
                    this.enabled = false;
                }
            }
        }
    }

    void SwitchScripts()
    {
        for (int i = 0; i < targetScripts.Length; i++)
        {
            targetScripts[i].enabled = on;
        }
    }
}

[Serializable]
public class TimeSpan :IComparable<TimeSpan>
{
    public float beginSec { get; }
    public float endSec { get; }

    public TimeSpan(float begin,float end)
    {
        beginSec = begin;
        endSec = end;
    }

    public int CompareTo(TimeSpan otherSpan)
    {
        if (beginSec < otherSpan.beginSec)
        {
            return -1;
        }
        else if (beginSec == otherSpan.beginSec)
        {
            return 0;
        }
        else
        {
            return 1;
        }
    }
}