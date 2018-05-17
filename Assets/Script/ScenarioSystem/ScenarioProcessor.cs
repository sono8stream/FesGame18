using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScenarioProcessor : MonoBehaviour
{
    [SerializeField]
    TextAsset testText;
    [SerializeField]
    ResourceLoader resourceLoader;
    [SerializeField]
    MessageProcessor messenger;
    [SerializeField]
    ImageProcessor imager;
    //[SerializeField]
    //Logger logger;
    SoundProcessor sounder;
    VariableProcessor varProcessor;
    SceneProcessor sceneProcessor;

    TextLoader textLoader;
    List<CommandProcessor> processorList;
    int processIndex;

    bool onEnd;

    public bool OnEnd { get { return onEnd; } }

    // Use this for initialization
    void Start()
    {
        Debug.Log(testText.text);
        textLoader = new TextLoader(testText.text);
        resourceLoader.Initialize(this);

        sounder = new SoundProcessor();
        sounder.Initialize(resourceLoader);
        varProcessor = new VariableProcessor();
        varProcessor.Initialize();
        sceneProcessor = new SceneProcessor();
        sceneProcessor.Initialize(this, resourceLoader);
        messenger.Initialize(textLoader, varProcessor);
        imager.Initialize(resourceLoader);

        processorList = new List<CommandProcessor>();
        processorList.Add(messenger);
        processorList.Add(imager);
        processorList.Add(sounder);
        processorList.Add(varProcessor);
        processorList.Add(sceneProcessor);
        processIndex = -1;

        onEnd = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (onEnd) return;

        if (processIndex == -1 && !SelectProcessor())
        {
            onEnd = true;
            return;//もう読み込めない
        }

        if (processorList[processIndex].Process())
        {
            processIndex = -1;
        }
    }

    bool SelectProcessor()
    {
        string targetText = textLoader.ReadLine();
        if (targetText == null) return false;

        int index = 0;
        if (targetText.StartsWith("[")
            && targetText.Split('\\').Length == 3)//特殊コマンドを処理
        {
            index = processorList.FindIndex(x => x.Trigger == targetText[1]);
        }
        if (index == -1) return false;

        processIndex = index;
        processorList[processIndex].ProcessBegin(targetText);
        Debug.Log(processIndex);
        return true;
    }

    public void ChangeScript(TextAsset newScript)
    {
        testText = newScript;
        textLoader = new TextLoader(testText.text);
        varProcessor.Initialize();
        onEnd = false;
    }
}