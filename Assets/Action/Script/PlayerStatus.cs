using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using sonoHelpers;

public class PlayerStatus : MonoBehaviour
{
    const int materialLimit = 5;

    [SerializeField]
    Text moneyText;
    [SerializeField]
    Transform materialsTransform;

    public int money;
    public int[] MaterialCounts { get; private set; }
    public float[] TempStatus { get; private set; }
    public float[] InitStatus { get; private set; }

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
        InitStatus = new float[statusLength];
        InitStatus[(int)StatusNames.speed] = 0.05f;
        InitStatus[(int)StatusNames.jumpLims] = 1;
        InitStatus[(int)StatusNames.hp] = 100;

        TempStatus = new float[statusLength];
        tempStatusTimer = new TimeCounter[statusLength];
        for (int i = 0; i < statusLength; i++)
        {
            TempStatus[i] = InitStatus[i];
            tempStatusTimer[i] = new TimeCounter(0);
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
        moneyText.text = string.Format("￥ {0:#,0}", money);
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

        TempStatus[statusIndex] = InitStatus[statusIndex];
        tempStatusTimer[statusIndex].Pause();
    }

    public void AddMaterial(int materialIndex, int increment)
    {
        if (MaterialCounts[materialIndex] >= materialLimit) return;

        MaterialCounts[materialIndex] += increment;
        if (materialsTransform.childCount <= materialIndex) return;

        UpdateMaterialText(materialIndex);
    }

    public void ReduceMaterial(int materialIndex, int decrement)
    {
        MaterialCounts[materialIndex] -= decrement;
        UpdateMaterialText(materialIndex);
    }

    void UpdateMaterialText(int materialIndex)
    {
        Transform t = materialsTransform.GetChild(materialIndex);
        t.Find("Text").GetComponent<Text>().text
            = MaterialCounts[materialIndex].ToString();
    }
}

public enum StatusNames
{
    speed = 0, jumpLims, hp
}

public enum MaterialNames
{
    Red = 0, Yellow, Blue, Purple, Silver, Orange, Green, Pink, Black, Rainbow
}