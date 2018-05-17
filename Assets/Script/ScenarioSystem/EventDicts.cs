using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using UnityEngine;

public class EventDicts
{
    public OrderedDictionary commandDict;
    public OrderedDictionary variableDict;
    public OrderedDictionary backSpriteDict;
    public OrderedDictionary charaSpriteDict;
    public OrderedDictionary bgmDict;
    public OrderedDictionary seDict;

    public EventDicts()
    {
        InitializeCommandDict();
        InitializeVariableDict();
        InitializeBackDict();
        InitializeCharaDict();
        InitializeBgmDict();
        InitializeSeDict();
    }

    void InitializeCommandDict()
    {
        commandDict = new OrderedDictionary();
    }
    
    void InitializeVariableDict()//変数辞典,[h]変数名:の形で指定可能
    {
        variableDict = new OrderedDictionary();
        variableDict.Add("testParam", 50);
    }

    void InitializeBackDict()//背景画像辞典
    {
        backSpriteDict = new OrderedDictionary();
    }

    void InitializeCharaDict()//キャラ画像辞典
    {
        charaSpriteDict = new OrderedDictionary();
    }

    void InitializeBgmDict()
    {
        bgmDict = new OrderedDictionary();

    }

    void InitializeSeDict()
    {
        seDict = new OrderedDictionary();
        
    }
}