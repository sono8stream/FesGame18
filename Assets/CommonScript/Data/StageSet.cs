using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageSet
{
    public int selectedIndex;

    List<StageStatus> stageList;

    public StageSet()
    {
        Initialize();
        selectedIndex = 0;
    }

    public StageStatus GetStage(int index)
    {
        return stageList[index];
    }

    public void Initialize()
    {
        stageList = new List<StageStatus>();
        stageList.Add(new StageStatus("北海道大学",
            "夏の北海道大学。" + Environment.NewLine + "ポプラ並木で商売開始！"));
        stageList.Add(new StageStatus("幕張メッセ",
            "TGS開催中の幕張メッセ。" + Environment.NewLine
            + "ステージ上を縦横無尽に駆け巡れ！"));
    }
}