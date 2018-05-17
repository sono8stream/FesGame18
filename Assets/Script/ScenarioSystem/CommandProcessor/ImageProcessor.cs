using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class ImageProcessor : CommandProcessor
{
    [SerializeField]
    Image sceneryImage;
    [SerializeField]
    Transform charactersTransform;
    [SerializeField]
    Transform cursorTransform;
    [SerializeField]
    float cursorPosY;

    ResourceLoader resourceLoader;

    public void Initialize(ResourceLoader loader)
    {
        trigger = 'i';

        cursorTransform.localPosition = new Vector2(-1500, cursorPosY);
        resourceLoader = loader;

        commandList = new List<Func<bool>>();
        commandList.Add(ChangeCharacterName);
        commandList.Add(ChangeCharacterImage);
        commandList.Add(HighlightCharacter);
        commandList.Add(MoveCharacter);
        commandList.Add(ChangeSceneryImage);
    }

    bool ChangeCharacterName()
    {
        return true;
    }

    bool ChangeCharacterImage()
    {
        Debug.Log(keyText);
        string[] keyStrings = keyText.Split(':');
        if (keyStrings.Length != 2) return true;

        int charaIndex;
        if (!(int.TryParse(keyStrings[0], out charaIndex)//総キャラ数はindex:0~3までの4人
            && (0 <= charaIndex && charaIndex <= 3))) return true;

        Sprite sprite = resourceLoader.GetCharaSprite(keyStrings[1]);
        if (sprite == null) return false;

        Image targetImage = charactersTransform.GetChild(charaIndex).GetComponent<Image>();
        targetImage.sprite = sprite;
        return true;
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
        Sprite sprite = resourceLoader.GetSceneSprite(keyText);
        if (sprite == null) return false;
        
        sceneryImage.sprite = sprite;
        return true;
    }
}