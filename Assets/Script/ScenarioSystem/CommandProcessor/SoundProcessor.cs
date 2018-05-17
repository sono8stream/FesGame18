using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundProcessor : CommandProcessor
{
    const int loadLim = 8;

    ResourceLoader resourceLoader;
    SoundPlayer soundPlayer;
    Waiter clipLoadWaiter;

    public void Initialize(ResourceLoader loader)
    {
        trigger = 's';

        resourceLoader = loader;
        soundPlayer = GameObject.Find(SoundPlayer.objectName).GetComponent<SoundPlayer>();
        clipLoadWaiter = new Waiter(loadLim);

        commandList = new List<System.Func<bool>>();
        commandList.Add(PlayBGM);
        commandList.Add(StopBGM);
        commandList.Add(RestartBGM);
        commandList.Add(PlaySE);
    }

    public override void ProcessBegin(string rawText)
    {
        base.ProcessBegin(rawText);

    }

    /// <summary>
    /// keyTextは ファイル名(:ループ開始:ループ終了位置) の形式, ()は任意
    /// 
    /// </summary>
    /// <returns></returns>
    bool PlayBGM()
    {
        string[] text = keyText.Split(':');
        AudioClip bgmData = resourceLoader.GetBGM(text[0]);
        if (bgmData == null)//未取得の時
        {
            if (clipLoadWaiter.Wait())
            {
                clipLoadWaiter.Initialize();
                return true;//何も再生しない
            }
            else
            {
                return false;//待機
            }
        }

        clipLoadWaiter.Initialize();
        BGMinfo bgm = new BGMinfo(bgmData);
        soundPlayer.PlayBGM(bgm);
        return true;
    }

    bool StopBGM()
    {
        soundPlayer.StopBGM();
        return true;
    }

    bool RestartBGM()
    {
        soundPlayer.RestartBGM();
        return true;
    }

    bool PlaySE()
    {
        AudioClip seData = resourceLoader.GetSE(keyText);
        if (seData == null)
        {
            if (clipLoadWaiter.Wait())
            {
                clipLoadWaiter.Initialize();
                return true;//何も再生しない
            }
            else
            {
                return false;//待機
            }
        }

        clipLoadWaiter.Initialize();
        soundPlayer.PlaySE(seData);
        return true;
    }
}
