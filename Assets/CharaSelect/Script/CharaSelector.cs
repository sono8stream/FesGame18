using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharaSelector : MonoBehaviour
{
    [SerializeField]
    int playerNo;
    [SerializeField]
    CharaList charaList;
    [SerializeField]
    Transform cursorT;
    [SerializeField]
    float firstCursorY;
    [SerializeField]
    float cursorDeltaY;
    [SerializeField]
    GameObject previewObj;
    [SerializeField]
    Text nameText, descriptionText;
    [SerializeField]
    RectTransform[] gaugeTransforms;

    Counter selectCounter;
    Counter pressingCounter;
    float[] targetGaugeX;
    float gaugeWidth;
    bool canSelect;

    // Use this for initialization
    void Start()
    {
        selectCounter = new Counter(charaList.charaCnt);
        pressingCounter = new Counter(10);
        targetGaugeX = new float[gaugeTransforms.Length];
        gaugeWidth = gaugeTransforms[0].sizeDelta.x;
        canSelect = true;
        Instantiate(charaList.GetPreview(selectCounter.Now),
            previewObj.transform, false);
        UpdateTargetStatus();
    }

    // Update is called once per frame
    void Update()
    {
        GetUpTargetStatus();

        if (Input.GetKeyDown(KeyCode.X))
        {
            CancelCharacter();
        }

        if (!canSelect) return;

        if (Input.GetKey(KeyCode.UpArrow)&&pressingCounter.Count())
        {
            pressingCounter.Initialize();
            SelectCharacter(-1);
        }
        if (Input.GetKey(KeyCode.DownArrow) && pressingCounter.Count())
        {
            pressingCounter.Initialize();
            SelectCharacter(1);
        }

        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            pressingCounter.Initialize();
            SelectCharacter(-1);
        }
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            pressingCounter.Initialize();
            SelectCharacter(1);
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            pressingCounter.Initialize();
            DecideCharacter();
        }
    }

    void SelectCharacter(int iterator)
    {
        if (selectCounter.Count(iterator))
        {
            selectCounter.Initialize();
        }
        if (selectCounter.Now < 0)
        {
            selectCounter.Now = selectCounter.Limit - 1;
        }
        Vector3 pos = cursorT.localPosition;
        cursorT.localPosition = new Vector3(pos.x,
            firstCursorY + cursorDeltaY * selectCounter.Now, pos.z);
        UpdateTargetStatus();
        foreach(Transform t in previewObj.transform)
        {
            Destroy(t.gameObject);
        }
        Instantiate(
            charaList.GetPreview(selectCounter.Now), previewObj.transform, false);
    }

    void DecideCharacter()
    {
        if (charaList.isSelected[selectCounter.Now]) return;
        canSelect = false;
        charaList.SwitchSelectState(true, selectCounter.Now,
            GetComponent<Image>().color);
        UserData.instance.characters[playerNo] = charaList[selectCounter.Now];
        Debug.Log(charaList[selectCounter.Now]);
    }

    void CancelCharacter()
    {
        canSelect = true;
        charaList.SwitchSelectState(false, selectCounter.Now, Color.white);
    }

    void UpdateTargetStatus()
    {
        float max = 30;
        CharaStatus charaStatus
            = UserData.instance.charaSet[charaList[selectCounter.Now]];

        nameText.text = charaStatus.name;
        descriptionText.text = charaStatus.description;
        targetGaugeX[1] = charaStatus.power / max * gaugeWidth;
        targetGaugeX[0] = charaStatus.defence / max * gaugeWidth;
        targetGaugeX[2] = charaStatus.speed / max * gaugeWidth;
    }

    void GetUpTargetStatus()
    {
        for (int i = 0; i < gaugeTransforms.Length; i++)
        {
            float x = gaugeTransforms[i].sizeDelta.x;
            gaugeTransforms[i].sizeDelta
                = new Vector2((x + targetGaugeX[i]) * 0.5f,
                gaugeTransforms[i].sizeDelta.y);
        }
    }
}