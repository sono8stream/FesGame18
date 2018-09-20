﻿using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using KoitanLib;

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
    [SerializeField]
    SpriteRenderer backRenderer, backFadeRenderer;
    [SerializeField]
    int fadeFrame;
    [SerializeField]
    ArrowAnimation rightArrowAnimation, leftArrowAnimation;
    [SerializeField]
    float boundScale;
    [SerializeField]
    int boundPeriod;

    Counter selectCounter;
    int stageCount;
    float nowAngle, targetAngle;
    float anglePerStage;
    bool onRotate;

    float[] targetGaugeX;
    float gaugeWidth;
    bool canSelect;

    Counter fadeCounter;
    bool onFade;
    Counter transitionCounter;
    bool onTransition;

    // Use this for initialization
    void Start()
    {
        stageCount = transform.childCount;
        selectCounter = new Counter(stageCount, true);
        anglePerStage = 360f / stageCount;

        for (int i = 0; i < stageCount; i++)//再配置
        {
            Transform t = transform.GetChild(i);
            transform.GetChild(i).eulerAngles = Vector3.zero;
            float rad = (anglePerStage * i - 90) * Mathf.PI / 180;
            t.localPosition = new Vector3(
                Mathf.Cos(rad), 0, Mathf.Sin(rad)) * selectRadius;
        }
        nowAngle = 0;
        targetAngle = 0;
        
        canSelect = true;

        SelectStage(1);

        fadeCounter = new Counter(fadeFrame);
        transitionCounter = new Counter(50);
    }

    // Update is called once per frame
    void Update()
    {
        if (onFade) FadeBackground();

        if (onTransition)
        {
            transitionCounter.Count();
            if (transitionCounter.Now == transitionCounter.Limit / 2)
            {
                if (selectCounter.Now == 0)
                {
                    LoadManager.Find().LoadScene(3);
                }
                else
                {
                    LoadManager.Find().LoadScene(7);
                }
            }
            transform.GetChild(selectCounter.Now).localScale
                = Vector3.one * (transitionCounter.Now % 2);
            return;
        }

        if (onRotate)
        {
            if (Mathf.Abs(nowAngle - targetAngle) < 0.1f)
            {
                onRotate = false;
            }
            nowAngle = (nowAngle * 3 + targetAngle) * 0.25f;

            RotateList(nowAngle);

            if (KoitanInput.GetAxisDown(Axis.Cross_Horizontal) == -1
            || KoitanInput.GetAxisDown(Axis.L_Horizontal) == -1)
            {
                SelectStage(-1);
                leftArrowAnimation.BigBound(boundScale, boundPeriod);
            }
            if (KoitanInput.GetAxisDown(Axis.Cross_Horizontal) == 1
            || KoitanInput.GetAxisDown(Axis.L_Horizontal) == 1)
            {
                SelectStage(1);
                rightArrowAnimation.BigBound(boundScale, boundPeriod);
            }
            return;
        }

        if (!canSelect) return;
        if (KoitanInput.GetButtonDown(ButtonID.A))
        {
            ChooseStage();
            Debug.Log(canSelect);
        }

        if (KoitanInput.GetAxisDown(Axis.Cross_Horizontal) == -1
            || KoitanInput.GetAxisDown(Axis.L_Horizontal) == -1)
        {
            SelectStage(-1);
            leftArrowAnimation.BigBound(boundScale, boundPeriod);
        }
        if (KoitanInput.GetAxisDown(Axis.Cross_Horizontal) == 1
            || KoitanInput.GetAxisDown(Axis.L_Horizontal) == 1)
        {
            SelectStage(1);
            rightArrowAnimation.BigBound(boundScale, boundPeriod);
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
        backFadeRenderer.sprite = backRenderer.sprite;
        backRenderer.sprite = transform.GetChild(selectCounter.Now)
            .GetComponent<SpriteRenderer>().sprite;
        onFade = true;
    }

    void ChooseStage()
    {
        UserData.instance.selectedStageNo = selectCounter.Now;
        canSelect = false;
        onTransition = true;
    }

    void FadeBackground()
    {
        if (fadeCounter.Count())
        {
            fadeCounter.Initialize();
            onFade = false;
            backFadeRenderer.color = Color.white;
            backFadeRenderer.sprite = null;
            return;
        }

        float alpha = 1f * fadeCounter.Now / fadeCounter.Limit;
        backFadeRenderer.color = new Color(1, 1, 1, 1 - alpha);
    }
}