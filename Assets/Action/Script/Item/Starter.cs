using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Starter : MonoBehaviour
{

    [SerializeField]
    MonoBehaviour targetScript;
    [SerializeField]
    float startSec;
    [SerializeField]
    bool enableOrNot = true;

    TimeCounter timeCounter;
    bool onEnd;

    // Use this for initialization
    void Awake()
    {
        TimeCounter timeCounter = new TimeCounter(startSec);
        timeCounter.Start();
        targetScript.enabled = !enableOrNot;
    }

    // Update is called once per frame
    void Update()
    {
        if (onEnd) return;
        //Debug.Log(timeCounter);
        if (timeCounter == null)
        {
            timeCounter = new TimeCounter(startSec);
            timeCounter.Start();
            //Debug.Log("ReStart");
        }
        if (timeCounter.OnLimit())
        {
            targetScript.enabled = enableOrNot;
            onEnd = true;
        }
    }
}