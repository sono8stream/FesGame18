using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using sonoHelpers;

public class PlayerStatus : MonoBehaviour
{
    [SerializeField]
    Text moneyText;

    public int money;
    public int[] MaterialCounts { get; private set; }
    public float[] TempStatus { get; private set; }

    float[] initStatus;
    TimeCounter[] tempStatusTimer;

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
        money = 0;

        //ステータスの配列の初期化
        statusLength = EnumHelper.GetTypeLength<StatusNames>();
        initStatus = new float[statusLength];
        initStatus[(int)StatusNames.speed] = 0.05f;
        initStatus[(int)StatusNames.jumpLims] = 1;
        initStatus[(int)StatusNames.hp] = 100;

        TempStatus = new float[statusLength];
        tempStatusTimer = new TimeCounter[statusLength];
        for (int i = 0; i < statusLength; i++)
        {
            TempStatus[i] = initStatus[i];
            tempStatusTimer[i] = new TimeCounter();
        }
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < statusLength; i++)
        {
            if (tempStatusTimer[i].OnLimit())
            {
                ResetStatus(i);
                Debug.Log("End Item Power!");
            }
        }
        moneyText.text = string.Format("￥{0:#,0}", money);
    }

    public void ChangeStatus(int statusIndex, float val, float lastSec = -1)
    {
        if (statusIndex < 0 || MaterialCounts.Length <= statusIndex) return;


        ResetStatus(statusIndex);

        TempStatus[statusIndex] = val;
        if (Mathf.RoundToInt(lastSec) == -1) return;

        tempStatusTimer[statusIndex].Start(lastSec);
    }

    void ResetStatus(int statusIndex)
    {
        if (statusIndex < 0 || MaterialCounts.Length <= statusIndex) return;

        TempStatus[statusIndex] = initStatus[statusIndex];
        tempStatusTimer[statusIndex].Stop();
    }

    public void AddMaterial(int materialIndex, int increment)
    {
        MaterialCounts[materialIndex] += increment;
    }

    public void ReduceMaterial(int materialIndex, int decrement)
    {
        MaterialCounts[materialIndex] -= decrement;
    }
}

public enum StatusNames
{
    speed = 0, jumpLims, hp
}

public enum MaterialNames
{
    Red = 0, Blue, Green
}