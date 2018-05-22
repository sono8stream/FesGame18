using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

[System.Serializable]
public class MessageProcessor : CommandProcessor
{
    [SerializeField]
    Text messageText;
    [SerializeField]
    int maxLineCount;
    [SerializeField]
    float lineHeight;
    [SerializeField]
    Transform choicesTransform;
    //[SerializeField]
    //Logger logger;
    [SerializeField]
    Image waitImage;

    const int defaultCount = 3;
    const int inputWaitCount = 5;
    const int waitCount = 60;
    const int choicesLim = 5;

    bool onSkip;
    bool choiceSelected;
    bool onTag;
    bool onAuto;
    bool onNewLine;
    int autoWaitLim;
    VariableProcessor varProcessor;
    Waiter messageWaiter;
    Waiter inputWaiter;
    Waiter autoWaiter;
    Counter messageLengthCounter;
    Counter messageBoxCounter;//メッセージbox内トータル文字数
    Counter lineCounter;
    Counter lineUpCounter;
    Counter waitCounter;
    Counter choicesCounter;
    TextLoader loader;

    public void Initialize(TextLoader loader,VariableProcessor vProcessor)
    {
        trigger = 'm';

        commandList = new List<Func<bool>>();
        commandList.Add(WriteMessage);//default command
        commandList.Add(WaitInitialize);
        commandList.Add(Wait);
        commandList.Add(InitializeBox);
        commandList.Add(ChangeSpeed);
        commandList.Add(AddChoice);//[m\5\〇〇]
        commandList.Add(WaitSelect);
        commandList.Add(EnableAuto);
        commandList.Add(DisableAuto);

        waitImage.enabled = false;

        messageWaiter = new Waiter(defaultCount);
        inputWaiter = new Waiter(inputWaitCount);

        messageLengthCounter = new Counter(1, true);
        messageBoxCounter = new Counter(500);

        lineCounter = new Counter(maxLineCount);
        lineUpCounter = new Counter(10);

        waitCounter = new Counter(waitCount);
        autoWaitLim = 30;
        autoWaiter = new Waiter(autoWaitLim);
        
        choicesCounter = new Counter(0);
        FocusChoice(choicesCounter.Now, true);
        foreach (Transform t in choicesTransform)
        {
            EventTrigger trigger = t.gameObject.AddComponent<EventTrigger>();
            trigger.triggers = new List<EventTrigger.Entry>();

            EventTrigger.Entry entry = new EventTrigger.Entry();
            entry.eventID = EventTriggerType.PointerEnter;
            entry.callback.AddListener(x => ChangeChoice(t.GetSiblingIndex()));
            trigger.triggers.Add(entry);

            t.GetComponent<Button>().onClick.AddListener(ClickChoice);
            t.gameObject.SetActive(false);
        }
        this.loader = loader;
        varProcessor = vProcessor;
    }

    public override void ProcessBegin(string rawText)
    {
        base.ProcessBegin(rawText);

        if (commandNo == -1)
        {
            commandNo = 0;//defaultにセット
            keyText= ConvertVariable(rawText);
            PreWriteTags();
            onTag = false;
            messageLengthCounter.Initialize(keyText.Length);
        }
        Debug.Log(commandNo);
        Debug.Log(keyText);
    }

    #region Commands
    bool WriteMessage()//0
    {
        SwitchSkipState();
        
        while(WriteChar(keyText[messageLengthCounter.Now]))
        {
            if (messageLengthCounter.Count())
            {
                messageText.text += "\r\n";
                messageBoxCounter.Count(2);
                if (onNewLine)
                {
                    lineUpCounter.Count(lineUpCounter.Limit);
                    MoveUpLine();
                }
                if (lineCounter.Count())
                {
                    onNewLine = true;
                }
                return true;//メッセージ終端で処理終わり
            }
        }

        MoveUpLine();

        return false;
    }

    bool WriteChar(char character)
    {
        if (character == '<') onTag = true;//htmlタグを検出
        if (onTag)//既に書き込み済みなので飛ばす
        {
            if (character == '>') onTag = false;
            messageBoxCounter.Count();
            return true;
        }

        if (InputEnter() || onSkip || messageWaiter.Wait())
        {
            messageWaiter.Initialize();
            messageText.text
                = messageText.text.Insert(messageBoxCounter.Now, character.ToString());
            if (character == '\n'&& lineCounter.Count())
            {
                onNewLine = true;
            }
            messageBoxCounter.Count();
            Debug.Log(lineCounter.Now);
            return true;
        }
        return false;
    }
    
    bool WaitInitialize()//1
    {
        if (Wait())
        {
            return InitializeBox();
        }
        return false;
    }

    bool Wait()//2
    {
        SwitchSkipState();

        if (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0)
            || (onSkip && inputWaiter.Wait()) || (onAuto && autoWaiter.Wait()))
        {
            inputWaiter.Initialize();
            waitCounter.Initialize();
            autoWaiter.Initialize();
            waitImage.enabled = false;
            waitImage.transform.eulerAngles = Vector3.zero;
            return true;
        }

        if (waitCounter.Now == 0)
        {
            waitImage.enabled = true;
            waitImage.transform.eulerAngles = Vector3.zero;
        }
        else if (waitCounter.Now == waitCount / 2)
        {
            waitImage.transform.eulerAngles = Vector3.forward * 30;
        }
        if (waitCounter.Count()) waitCounter.Initialize();
        return false;
    }

    bool InitializeBox()//3
    {
        messageText.text = "";
        messageBoxCounter.Initialize();
        lineCounter.Initialize();
        return true;
    }
    
    bool ChangeSpeed()//4数値で文字速度変更, 大きいほど遅い(def=3)
    {
        int lim = keyText[0] == 'd' ? defaultCount : int.Parse(keyText);
        messageWaiter.Initialize(lim);
        return true;
    }

    bool AddChoice()//5
    {
        if (choicesCounter.Limit >= choicesLim) return true;

        Transform t = choicesTransform.GetChild(choicesCounter.Limit);
        t.gameObject.SetActive(true);
        t.Find("Text").GetComponent<Text>().text = keyText;
        choicesCounter.Initialize(choicesCounter.Limit + 1);
        return true;
    }

    bool WaitSelect()//6
    {
        SwitchSkipState();

        SelectChoice();

        if (!(Input.GetKeyDown(KeyCode.Space)||choiceSelected)) return false;

        loader.JumpLabel(choicesTransform.GetChild(
               choicesCounter.Now).GetChild(0).GetComponent<Text>().text);
        foreach (Transform t in choicesTransform)
        {
            t.gameObject.SetActive(false);
        }
        FocusChoice(choicesCounter.Now, false);
        choicesCounter.Initialize(0);
        FocusChoice(choicesCounter.Now, true);
        choiceSelected = false;
        return true;
    }

    bool EnableAuto()
    {
        onAuto = true;
        int counter;
        if(int.TryParse(keyText,out counter))
        {
            autoWaiter.Initialize(counter);
        }
        return true;
    }

    bool DisableAuto()
    {
        onAuto = false;
        return true;
    }
    #endregion

    void MoveUpLine()
    {
        if (!onNewLine) return;

        if (lineUpCounter.Count())
        {
            onNewLine = false;
            lineUpCounter.Initialize();
            int firstLineLength = messageText.text.IndexOf('\n')+1;
            messageText.text = messageText.text.Substring(firstLineLength);
            messageBoxCounter.Now -= firstLineLength;
        }
        messageText.GetComponent<RectTransform>().offsetMax
            = Vector2.up * lineHeight * lineUpCounter.Now / lineUpCounter.Limit;
    }

    void PreWriteTags()
    {
        foreach (char c in keyText)
        {
            if (c == '<') onTag = true;//htmlタグを検出、先に表示
            if (onTag)
            {
                if (c == '>') onTag = false;
                messageText.text += c;
            }
        }
    }

    string ConvertVariable(string rawText)//_test_のようなテキストの場合、対応する変数を返す
    {
        string convertedText = rawText;
        /*MatchCollection matches = new Regex("(_.*?_)").Matches(rawText);
        for (int i = 0; i < matches.Count; i++)
        {
            string rawVariable = matches[i].Value;
            string variableText = rawVariable.Trim('_');//前後の_つぶす
            convertedText = convertedText.Replace(rawVariable, GetVariable(variableText));
        }*/

        MatchCollection matches = new Regex("(_.*?_)|(@.*?@)").Matches(rawText);
        for(int i = 0; i < matches.Count; i++)
        {
            string rawName = matches[i].Value;
            Debug.Log(rawName);
            convertedText = convertedText.Replace(
                rawName, varProcessor.GetVariableValue(rawName).ToString());
        }
        return convertedText;
    }

    string GetVariable(string name)
    {
        if (!UserData.instance.variableDict.ContainsKey(name)) return name;
        return UserData.instance.variableDict[name].ToString();
    }

    bool InputEnter()
    {
        return Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0);
    }

    void SwitchSkipState()
    {
        if (Input.GetKeyDown(KeyCode.X) || Input.GetMouseButtonDown(1)) onSkip = true;
        if (Input.GetKeyUp(KeyCode.X) || Input.GetMouseButtonUp(1)) onSkip = false;
    }

    void FocusChoice(int index,bool on)
    {
        Color c = on ? Color.red : Color.white;
        //choicesTransform.GetChild(index).GetComponent<Image>().color = c;
        choicesTransform.GetChild(index).GetComponent<RectTransform>().localScale
            = on ? Vector3.one * 1.2f : Vector3.one;
    }

    void SelectChoice()
    {
        int index = choicesCounter.Now;
        if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.UpArrow))
        {
            index = 0 < choicesCounter.Now
                ? choicesCounter.Now - 1 : choicesCounter.Limit - 1;
        }
        if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.DownArrow))
        {
            index = choicesCounter.Now == choicesCounter.Limit - 1 ? 0 : choicesCounter.Now + 1;
        }
        ChangeChoice(index);
        return;
    }

    public void ChangeChoice(int index)
    {
        if (choicesCounter.Now == index) return;

        FocusChoice(choicesCounter.Now, false);
        FocusChoice(index, true);
        choicesCounter.Now = index;
    }

    public void ClickChoice()
    {
        choiceSelected = true;
    }    
}
