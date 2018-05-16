using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using sonoHelpers;

public class PlayerStatus : MonoBehaviour
{
    public int[] MaterialCounts { get; private set; }
    public int Money { get; private set; }
    public float[] TempStatus { get; private set; }

    float[] initStatus;
    TimeCounter[] tempStatusCounter;

    int materialNamesLength;
    int statusLength;

    // Use this for initialization
    void Start()
    {
        //素材取得数の配列の初期化
        materialNamesLength = EnumHelper.GetTypeLength<MaterialNames>();
        MaterialCounts = new int[materialNamesLength];
        for (int i = 0; i < materialNamesLength; i++)
        {
            MaterialCounts[i] = 0;
        }
        Money = 0;

        //ステータスの配列の初期化
        statusLength = EnumHelper.GetTypeLength<StatusNames>();
        initStatus = new float[statusLength];
        initStatus[(int)StatusNames.speed] = 0.05f;
        TempStatus = new float[statusLength];
        tempStatusCounter = new TimeCounter[statusLength];
        for (int i = 0; i < statusLength; i++)
        {
            TempStatus[i] = initStatus[i];
            tempStatusCounter[i] = new TimeCounter();
        }
    }

    // Update is called once per frame
    void Update()
    {
        for(int i = 0; i < statusLength; i++)
        {
            if (tempStatusCounter[i].OnLimit())
            {
                ResetStatus(i);
                Debug.Log("End Item Power!");
            }
        }
    }

    public void ChangeStatus(int statusIndex, float val, float lastSec = -1)
    {
        if (statusIndex < 0 || MaterialCounts.Length <= statusIndex) return;

        ResetStatus(statusIndex);

        TempStatus[statusIndex] = val;
        if (lastSec == -1) return;

        tempStatusCounter[statusIndex].Start(lastSec);
    }

    void ResetStatus(int statusIndex)
    {
        if (statusIndex < 0 || MaterialCounts.Length<=statusIndex) return;

        TempStatus[statusIndex] = initStatus[statusIndex];
        tempStatusCounter[statusIndex].Stop();
    }
}

public enum StatusNames
{
    speed, jumpLims
}

public enum MaterialNames
{
    Red = 0, Blue, Green
}