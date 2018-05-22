using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    [SerializeField]
    int limitSec = 30;

    float secCounter;
    Text leftTimeText;
    bool isWorking;

    // Use this for initialization
    void Start()
    {
        secCounter = 0;
        leftTimeText = GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isWorking)
        {
            return;
        }
        secCounter += Time.deltaTime; //スタートしてからの秒数を格納
        int leftSec = Mathf.RoundToInt(limitSec - secCounter);
        /*leftTimeText.text = string.Format("{0:00} : {1:00}",
            (leftSec / 60).ToString("00"), (leftSec % 60).ToString("00"));*/
        leftTimeText.text = string.Format("{0:00} : {1:00}",
            leftSec / 60, leftSec % 60);
    }

    public void StartTick()
    {

    }
}