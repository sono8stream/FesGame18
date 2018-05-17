using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SceneProcessor : CommandProcessor
{
    const int loadLim = 8;

    ScenarioProcessor scenarioProcessor;
    ResourceLoader resourceLoader;
    Waiter scriptLoadWaiter;
    Counter fadeCounter;
    LoadManager sceneLoader;
    float fadeSpeed;

    public void Initialize(ScenarioProcessor processor, ResourceLoader loader)
    {
        trigger = 'e';

        commandList = new List<System.Func<bool>>();
        commandList.Add(ChangeScenarioScript);
        commandList.Add(ChangeScene);
        commandList.Add(FadeIn);
        commandList.Add(FadeOut);

        scenarioProcessor = processor;
        resourceLoader = loader;
        scriptLoadWaiter = new Waiter(loadLim);
        fadeCounter = new Counter(1, true);
        sceneLoader = LoadManager.Find();
    }

    bool ChangeScenarioScript()
    {
        TextAsset script = resourceLoader.GetScript(keyText);
        if (script == null)//未取得の時
        {
            if (scriptLoadWaiter.Wait())
            {
                scriptLoadWaiter.Initialize();
                return true;//何も呼び出さない
            }
            else
            {
                return false;//待機
            }
        }

        scriptLoadWaiter.Initialize();
        scenarioProcessor.ChangeScript(script);
        return true;
    }

    bool ChangeScene()
    {
        int index;
        if (int.TryParse(keyText, out index))
        {
            sceneLoader.LoadScene(index);
        }
        return true;
    }

    bool FadeIn()
    {
        if (fadeCounter.OnLimit())//最初に呼び出し
        {
            int lim;
            if (!int.TryParse(keyText, out lim)) return true;

            fadeCounter.Initialize(lim);
            fadeSpeed = 1.0f / lim;
            sceneLoader.FadeImage.enabled = true;
        }

        sceneLoader.FadeImage.color += Color.black * fadeSpeed;
        return fadeCounter.Count();
    }

    bool FadeOut()
    {
        if (fadeCounter.OnLimit())//最初に呼び出し
        {
            int lim;
            if (!int.TryParse(keyText, out lim)) return true;

            fadeCounter.Initialize(lim);
            fadeSpeed = 1.0f / lim;
        }

        sceneLoader.FadeImage.color -= Color.black * fadeSpeed;
        if (fadeCounter.Count())
        {
            sceneLoader.FadeImage.enabled = false;
        }
        return fadeCounter.OnLimit();
    }
}
