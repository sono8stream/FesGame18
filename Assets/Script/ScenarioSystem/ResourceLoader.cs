using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class ResourceLoader
{
    [SerializeField]
    Sprite defaultSprite;

    const string defaultName = "none";

    Dictionary<string, Sprite> charaSpriteDict;//読み込んだスプライトの実参照
    Dictionary<string, Sprite> sceneSpriteDict;//読み込んだスプライトの実参照
    Dictionary<string, AudioClip> bgmDict;//読み込んだスプライトの実参照
    Dictionary<string, AudioClip> seDict;//読み込んだスプライトの実参照
    Dictionary<string, TextAsset> scriptDict;

    bool loadFailed;
    ScenarioProcessor scenarioProcessor;
    int loadStateNo;

    enum LoadStateName
    {
        Unload = 0, Loading, Complete
    }

    public void Initialize(ScenarioProcessor processor)
    {
        scenarioProcessor = processor;
        SetDefault();
    }

    void SetDefault()
    {
        charaSpriteDict = new Dictionary<string, Sprite>();
        charaSpriteDict.Add(defaultName, defaultSprite);

        sceneSpriteDict = new Dictionary<string, Sprite>();
        sceneSpriteDict.Add(defaultName, defaultSprite);

        bgmDict = new Dictionary<string, AudioClip>();
        bgmDict.Add(defaultName, null);

        seDict = new Dictionary<string, AudioClip>();
        seDict.Add(defaultName, null);

        scriptDict = new Dictionary<string, TextAsset>();
        scriptDict.Add(defaultName, null);
    }

    /// <summary>
    /// codeに一致するリソースを読み込む
    /// ロード済みならそのまま返し、ロードしていないときロード、存在していないときnullを返す
    /// </summary>
    /// <param name="code"></param>
    /// <returns></returns>
    public Sprite GetCharaSprite(string name)
    {
        return GetResource(name, defaultName, charaSpriteDict, "Portrait");
    }

    public Sprite GetSceneSprite(string name)
    {
        return GetResource(name, defaultName, sceneSpriteDict, "Scenery");
    }

    public AudioClip GetBGM(string name)
    {
        return GetResource(name,defaultName, bgmDict, "BGM");
    }

    public AudioClip GetSE(string name)
    {
        return GetResource(name, defaultName, seDict, "SE");
    }

    public TextAsset GetScript(string name)
    {
        return GetResource(name, defaultName, scriptDict, "ScenarioScript");
    }

    Type GetResource<Type>(string name, string defaultName,
        Dictionary<string, Type> resourceDict, string folderName)
        where Type : class
    {
        switch (loadStateNo)
        {
            case (int)LoadStateName.Unload:
                if (resourceDict.ContainsKey(name))
                {
                    Debug.Log("Successfully Loaded");
                    return resourceDict[name];
                }
                //ロード処理
                loadStateNo = (int)LoadStateName.Loading;
                string path = folderName + "/" + name;
                ResourceRequest request = Resources.LoadAsync(path, typeof(Type));
                scenarioProcessor.StartCoroutine(CheckLoadDone(
                    name, request, resourceDict));
                break;

            case (int)LoadStateName.Complete:
                loadStateNo = (int)LoadStateName.Unload;
                return loadFailed ? 
                    resourceDict[defaultName] : resourceDict[name];
        }
        return default(Type);
    }

    IEnumerator CheckLoadDone<Type>(string name, ResourceRequest request,
        Dictionary<string, Type> resourceDict)
        where Type : class
    {
        while (!request.isDone) yield return null;

        Type resource = request.asset as Type;
        if (resource == null)
        {
            Debug.Log("Failed to load");
            loadFailed = true;
        }
        else
        {
            Debug.Log("load succeed");
            resourceDict.Add(name, resource);
            loadFailed = false;
        }
        Debug.Log("load done");
        loadStateNo = (int)LoadStateName.Complete;
    }
}