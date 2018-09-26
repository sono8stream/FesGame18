using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using KoitanLib;

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
    [SerializeField]
    AudioClip selectSE, decideSE, cancelSE;

    Counter selectCounter;
    Counter pressingCounter;
    float[] targetGaugeX;
    float gaugeWidth;
    bool canSelect;
    bool isPlayable;

    Animator previewAnimator;

    // Use this for initialization
    void Start()
    {
        int cnt = KoitanInput.ControllerCount();

        selectCounter = new Counter(charaList.charaCnt);
        pressingCounter = new Counter(10);
        targetGaugeX = new float[gaugeTransforms.Length];
        gaugeWidth = gaugeTransforms[0].sizeDelta.x;
        canSelect = true;
        Instantiate(charaList.GetPreview(selectCounter.Now),
            previewObj.transform, false);
        UpdateTargetStatus();

        isPlayable = false;
        foreach (Transform child in transform)
        {
            child.gameObject.SetActive(false);
        }
        previewObj.SetActive(false);
        cursorT.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (playerNo < KoitanInput.ControllerCount() && !isPlayable)
        {
            isPlayable = true;
            foreach (Transform child in transform)
            {
                child.gameObject.SetActive(true);
            }
            previewObj.SetActive(true);
            cursorT.gameObject.SetActive(true);
            previewAnimator = previewObj.transform.GetChild(0).GetComponent<Animator>();
            previewAnimator.Play("taiki");
        }
        else if (playerNo >= KoitanInput.ControllerCount() && isPlayable)
        {
            isPlayable = false;
            foreach (Transform child in transform)
            {
                child.gameObject.SetActive(false);
            }
            previewObj.SetActive(false);
            cursorT.gameObject.SetActive(false);
        }

        if (!isPlayable) return;

        GetUpTargetStatus();

        if (!canSelect && KoitanInput.GetButtonDown(ButtonID.B, playerNo))
        {
            CancelCharacter();
        }

        if (!canSelect) return;

        if ((KoitanInput.GetAxis(Axis.L_Vertical, playerNo) == -1
            || KoitanInput.GetAxis(Axis.Cross_Vertical, playerNo) == -1)
            && pressingCounter.Count())
        {
            pressingCounter.Initialize();
            SelectCharacter(-1);
        }
        if ((KoitanInput.GetAxis(Axis.L_Vertical, playerNo) == 1
            || KoitanInput.GetAxis(Axis.Cross_Vertical, playerNo) == 1)
            && pressingCounter.Count())
        {
            pressingCounter.Initialize();
            SelectCharacter(1);
        }

        if (KoitanInput.GetAxisDown(Axis.L_Vertical, playerNo) < 0
            || KoitanInput.GetAxisDown(Axis.Cross_Vertical, playerNo) < 0)
        {
            pressingCounter.Initialize();
            SelectCharacter(-1);
        }
        if (KoitanInput.GetAxisDown(Axis.L_Vertical, playerNo) > 0
            || KoitanInput.GetAxisDown(Axis.Cross_Vertical, playerNo) > 0)
        {
            pressingCounter.Initialize();
            SelectCharacter(1);
        }

        if (KoitanInput.GetButtonDown(ButtonID.A, playerNo))
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
        GameObject obj= Instantiate(
            charaList.GetPreview(selectCounter.Now), previewObj.transform, false);
        previewAnimator = obj.GetComponent<Animator>();
        previewAnimator.Play("taiki");
        SoundPlayer.Find().PlaySE(selectSE);
    }

    void DecideCharacter()
    {
        if (charaList.isSelected[selectCounter.Now]) return;
        canSelect = false;
        charaList.SwitchSelectState(true, selectCounter.Now,
            GetComponent<Image>().color);
        UserData.instance.characters[playerNo] = charaList[selectCounter.Now];
        Debug.Log(charaList[selectCounter.Now]);
        previewAnimator.SetBool("selected", true);
        SoundPlayer.Find().PlaySE(decideSE);
    }

    void CancelCharacter()
    {
        canSelect = true;
        charaList.SwitchSelectState(false, selectCounter.Now, Color.white);
        previewAnimator.SetBool("selected", false);
        SoundPlayer.Find().PlaySE(cancelSE);
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