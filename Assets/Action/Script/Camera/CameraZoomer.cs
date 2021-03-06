﻿using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraZoomer : MonoBehaviour
{
    [SerializeField]
    List<Transform> viewTargets;
    [SerializeField]
    float widthMargin, heightMargin;
    [SerializeField]
    float minWidth = 3;
    [SerializeField]
    Rect maxRect = new Rect(-3, -2, 6, 4);
    [SerializeField]
    float followSpeedRate = 0.1f;//0~1、1なら即座に追従

    float heightRatio;
    Camera mainCamera;

    // Use this for initialization
    void Start()
    {
        heightRatio = 1.0f * Screen.height / Screen.width;
        Debug.Log(heightRatio);
        mainCamera = GetComponent<Camera>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        UpdateView();
    }

    void UpdateView()
    {
        followSpeedRate = Mathf.Clamp(followSpeedRate, 0.01f, 1);

        Rect viewRect = CalcViewRect();
        transform.position
            = (Vector2)transform.position * (1 - followSpeedRate)
            + viewRect.center * followSpeedRate;
        transform.position -= Vector3.forward * 10;
        mainCamera.orthographicSize = viewRect.height * 0.5f;
    }

    Rect CalcViewRect()
    {
        Vector2 minPos = viewTargets[0].position;
        Vector2 maxPos = viewTargets[0].position;

        int targetsCount = viewTargets.Count;
        for (int i = 1; i < targetsCount; i++)
        {
            if (viewTargets[i] == null)
            {
                viewTargets.RemoveAt(i);
                i--;
                continue;
            }
            if (!viewTargets[i].gameObject.activeSelf) continue;
            Vector2 targetPos = viewTargets[i].position;
            minPos = Vector2.Min(minPos, targetPos);
            maxPos = Vector2.Max(maxPos, targetPos);
        }

        minPos.x -= widthMargin;
        minPos.y -= heightMargin;
        maxPos.x += widthMargin;
        maxPos.y += heightMargin;

        Rect viewRect = new Rect(minPos, maxPos - minPos);
        return FitRectToScreen(viewRect);
    }

    Rect FitRectToScreen(Rect rawRect)
    {
        if (rawRect.width * heightRatio < rawRect.height)//縦長のとき
        {
            float newWidth = rawRect.height / heightRatio;
            rawRect.x = rawRect.center.x - newWidth * 0.5f;
            rawRect.width = newWidth;
        }
        else//横長のとき
        {
            float newHeight = rawRect.width * heightRatio;
            rawRect.y = rawRect.center.y - newHeight * 0.5f;
            rawRect.height = newHeight;
        }
        if (rawRect.width < minWidth)//最小以下
        {
            rawRect.x = rawRect.center.x - minWidth * 0.5f;
            rawRect.y = rawRect.center.y - minWidth * heightRatio * 0.5f;
            rawRect.width = minWidth;
            rawRect.height = minWidth * heightRatio;
        }

        LapInMaxRect(ref rawRect);

        return rawRect;
    }

    void LapInMaxRect(ref Rect rawRect)//最大範囲内に収める
    {
        if (rawRect.xMin < maxRect.xMin)
        {
            rawRect.x = maxRect.xMin;
        }
        if (rawRect.xMax > maxRect.xMax)
        {
            rawRect.x = maxRect.xMax - rawRect.width;
        }
        if (rawRect.xMin < maxRect.xMin)//縮小
        {
            rawRect.xMin = maxRect.xMin;
            float newHeight = rawRect.width * heightRatio;
            rawRect.y = rawRect.center.y - newHeight * 0.5f;
            rawRect.height = newHeight;
        }

        if (rawRect.yMin < maxRect.yMin)
        {
            rawRect.y = maxRect.yMin;
        }
        if (rawRect.yMax > maxRect.yMax)
        {
            rawRect.y = maxRect.yMax - rawRect.height;
        }
        if (rawRect.yMin < maxRect.yMin)//縮小
        {
            rawRect.yMin = maxRect.yMin;
            float newWidth = rawRect.height / heightRatio;
            rawRect.x = rawRect.center.x - newWidth * 0.5f;
            rawRect.width = newWidth;
        }
    }
}