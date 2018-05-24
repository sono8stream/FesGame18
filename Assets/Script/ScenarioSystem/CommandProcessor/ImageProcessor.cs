using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class ImageProcessor : CommandProcessor
{
    const int fadeLim = 10;

    [SerializeField]
    Image sceneryImage;
    [SerializeField]
    Transform charactersTransform;
    [SerializeField]
    Transform cursorTransform;
    [SerializeField]
    float cursorPosY;

    ResourceLoader resourceLoader;
    Counter fadeCounter;
    int fadeStateNo;
    string spriteName;

    enum FadeStateName
    {
        FadeOut = 0,FadeIn
    }

    public void Initialize(ResourceLoader loader)
    {
        trigger = 'i';

        cursorTransform.localPosition = new Vector2(-1500, cursorPosY);
        resourceLoader = loader;
        fadeCounter = new Counter(fadeLim);

        commandList = new List<Func<bool>>();
        commandList.Add(ChangeCharacterName);
        commandList.Add(ChangeCharacterImage);
        commandList.Add(HighlightCharacter);
        commandList.Add(MoveCharacter);
        commandList.Add(ChangeSceneryImage);
    }

    public override void ProcessBegin(string rawText)
    {
        base.ProcessBegin(rawText);

        if (commandNo == 1 || commandNo == 4)//画像を読み込むとき
        {
            /*if (commandNo == 1)
            {
                string[] keyStrings = keyText.Split(':');
                string spriteName;
                if (keyStrings.Length == 2)
                {
                    spriteName = keyStrings[1]
;                }
            }*/
            fadeStateNo = (int)FadeStateName.FadeOut;
        }
    }

    bool ChangeCharacterName()
    {
        return true;
    }

    bool ChangeCharacterImage()
    {
        string[] keyStrings = keyText.Split(':');
        if (keyStrings.Length != 2) return true;

        int charaIndex;
        if (!(int.TryParse(keyStrings[0], out charaIndex)//総キャラ数はindex:0~3までの4人
            && (0 <= charaIndex && charaIndex <= 3))) return true;

        Image targetImage
            = charactersTransform.GetChild(charaIndex).GetComponent<Image>();

        //Debug.Log("Fading");
        switch (fadeStateNo)
        {
            case (int)FadeStateName.FadeOut:
                Sprite sprite = resourceLoader.GetCharaSprite(keyStrings[1]);
                Debug.Log(sprite);
                if (FadeOut(targetImage) && sprite != null)
                {
                    targetImage.sprite = sprite;
                    fadeStateNo = (int)FadeStateName.FadeIn;
                    fadeCounter.Initialize();
                }
                break;
            case (int)FadeStateName.FadeIn:
                if (FadeIn(targetImage))
                {
                    fadeCounter.Initialize();
                    return true;
                }
                break;
        }
        return false;
    }

    bool HighlightCharacter()
    {
        int charaIndex;
        if (!(int.TryParse(keyText, out charaIndex)//総キャラ数はindex:0~3までの4人
            && (-1 <= charaIndex && charaIndex <= 3))) return true;

        Vector2 pos = Vector2.up * cursorTransform.localPosition.y;
        if (charaIndex == -1)
        {
            pos += Vector2.left * 1000;
        }
        else
        {
            pos += Vector2.right * charactersTransform.GetChild(charaIndex).localPosition.x;
        }
        cursorTransform.localPosition = pos;
        return true;
    }

    bool MoveCharacter()
    {
        string[] keyStrings = keyText.Split(':');
        if (keyStrings.Length != 2) return true;

        return true;
    }

    bool ChangeSceneryImage()
    {
        switch (fadeStateNo)
        {
            case (int)FadeStateName.FadeOut:
                Sprite sprite = resourceLoader.GetSceneSprite(keyText);
                Debug.Log(sprite);
                if (FadeOut(sceneryImage) && sprite != null)
                {
                    sceneryImage.sprite = sprite;
                    fadeStateNo = (int)FadeStateName.FadeIn;
                    fadeCounter.Initialize();
                }
                break;
            case (int)FadeStateName.FadeIn:
                if (FadeIn(sceneryImage))
                {
                    fadeCounter.Initialize();
                    return true;
                }
                break;
        }
        return false;
    }

    bool FadeIn(Image targetImage)
    {
        if (fadeStateNo != (int)FadeStateName.FadeIn) return true;

        fadeCounter.Count();
        targetImage.color
            = Color.white * fadeCounter.Now / fadeCounter.Limit;
        return fadeCounter.OnLimit();
    }

    bool FadeOut(Image targetImage)
    {
        if (fadeStateNo != (int)FadeStateName.FadeOut) return true;

        fadeCounter.Count();
        targetImage.color
            = Color.white * (1 - 1f * fadeCounter.Now / fadeCounter.Limit);
        Debug.Log("Fade out");
        return fadeCounter.OnLimit();
    }
}