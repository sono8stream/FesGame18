using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StageSelector : MonoBehaviour
{
    [SerializeField]
    float selectRadius;
    [SerializeField]
    int playerNo;
    [SerializeField]
    Transform statusWin;
    [SerializeField]
    Sprite[] stageSprites;
    [SerializeField]
    Text nameText;
    [SerializeField]
    Text descriptionText;

    Counter selectCounter;
    int stageCount;
    float nowAngle, targetAngle;
    float anglePerStage;
    bool onRotate;

    float[] targetGaugeX;
    float gaugeWidth;
    int selectNo;//武器何個選択したか
    bool canSelect;

    // Use this for initialization
    void Start()
    {
        stageCount = transform.childCount;
        selectCounter = new Counter(stageCount);
        anglePerStage = 360f / stageCount;

        for (int i = 0; i < stageCount; i++)//再配置
        {
            Transform t = transform.GetChild(i);
            transform.GetChild(i).eulerAngles = Vector3.zero;
            float rad = (anglePerStage * i - 90) * Mathf.PI / 180;
            t.localPosition = new Vector3(
                Mathf.Cos(rad), 0, Mathf.Sin(rad)) * selectRadius;
            //t.GetComponent<SpriteRenderer>().sprite = stageSprites[i];
        }
        nowAngle = 0;
        targetAngle = 0;

        selectNo = 0;
        canSelect = true;

        SelectStage(1);
    }

    // Update is called once per frame
    void Update()
    {
        if (onRotate)
        {
            if (Mathf.Abs(nowAngle - targetAngle) < 0.1f)
            {
                onRotate = false;
            }
            nowAngle = (nowAngle*3 + targetAngle) * 0.25f;

            RotateList(nowAngle);

            if (Input.GetKeyDown(
                UserData.instance.playersKeySet[playerNo].GetKey(KeyName.Left)))
            {
                SelectStage(-1);
            }
            if (Input.GetKeyDown(
                UserData.instance.playersKeySet[playerNo].GetKey(KeyName.Right)))
            {
                SelectStage(1);
            }
            return;
        }

        if (Input.GetKeyDown(UserData.instance.playersKeySet[playerNo].GetKey(KeyName.Cancel)))
        {
            CancelStage();
        }
        if (!canSelect) return;
        if (Input.GetKeyDown(UserData.instance.playersKeySet[playerNo].GetKey(KeyName.Enter)))
        {
            ChooseStage();
            Debug.Log(canSelect);
        }

        if (Input.GetKey(UserData.instance.playersKeySet[playerNo].GetKey(KeyName.Left)))
        {
            SelectStage(-1);
        }
        if (Input.GetKey(UserData.instance.playersKeySet[playerNo].GetKey(KeyName.Right)))
        {
            SelectStage(1);
        }
    }

    void SelectStage(int iterator)
    {
        do
        {
            if (selectCounter.Count(iterator))
            {
                selectCounter.Initialize();
                nowAngle = -anglePerStage;
            }
            if (selectCounter.Now < 0)
            {
                selectCounter.Now = selectCounter.Limit - 1;
                nowAngle = 360;
            }
        }
        while (transform.GetChild(selectCounter.Now)
        .GetComponent<SpriteRenderer>().color == Color.black);

        onRotate = true;
        targetAngle = anglePerStage * selectCounter.Now;
        UpdateTargetStatus();
    }

    void RotateList(float angle)
    {
        transform.eulerAngles = Vector3.up * angle;

        foreach (Transform child in transform)
        {
            child.eulerAngles = Vector3.zero;
        }
    }

    void UpdateTargetStatus()
    {
        float max = 30;
        StageStatus stageStatus
            = UserData.instance.stages.GetStage(selectCounter.Now);

        nameText.text = stageStatus.name;
        descriptionText.text = stageStatus.description;
    }

    void ChooseStage()
    {
        int selectWeaponIndex = selectCounter.Now;
        UserData.instance.playersWeapon[playerNo].SetWeaponStatus(
            selectNo, selectWeaponIndex);
        transform.GetChild(selectCounter.Now).GetComponent<SpriteRenderer>().color
            = Color.black;

        selectNo++;
        canSelect = selectNo < WeaponSet.weaponCount;
        SelectStage(1);
    }

    void CancelStage()
    {
        if (selectNo == 0) return;

        selectNo--;
        int weaponIndex
            = UserData.instance.playersWeapon[playerNo].GetWeaponIndexByIndex(selectNo);
        transform.GetChild(weaponIndex)
            .GetComponent<SpriteRenderer>().color = Color.white;
        canSelect = true;
    }
}