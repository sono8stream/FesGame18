using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeCounter
{
    public float Limit { get; private set; }
    public float Now { get {
            return enabled ? Time.fixedTime - startTime : stopTime - startTime;
        } }

    bool enabled;
    float startTime;
    float stopTime;

    public TimeCounter(float lim)
    {
        Limit = lim;
    }

    public bool OnLimit()
    {
        if (!enabled) return false;
        if (Now >= Limit)
        {
            enabled = false;
            return true;
        }
        return false;
    }

    public void Start(float newLim = -1)
    {
        Limit = newLim == -1 ? Limit : newLim;
        startTime = Time.fixedTime;
        enabled = true;
    }

    public void Pause()
    {
        if (!enabled) return;
        enabled = false;
        stopTime = Time.fixedTime;
    }

    public void Restart()
    {
        if (enabled) return;
        startTime += Time.fixedTime - stopTime;//停止時間だけ進める
        enabled = true;
    }
}