using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeCounter
{
    public float Limit { get; private set; }
    public float Now { get { return Time.fixedTime - startTime; } }

    bool enabled;
    float startTime;

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

    public void Stop()
    {
        enabled = false;
    }
}