using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundPlayer : MonoBehaviour
{
    public const string objectName="SoundPlayer";

    const int maxSEcnt = 5;

    AudioSource audioSource;
    BGMinfo currentBGM;
    List<float> SElenList;
    float interruptBGMpos;

    // Use this for initialization
    void Awake()
    {
        gameObject.name = objectName;
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.loop = true;
        SElenList = new List<float>();
        interruptBGMpos = 0;
    }

    // Update is called once per frame
    void Update()
    {   //Update SElenList
        UpdateSElist();
        LoopBGM();
    }

    public void PlayBGM(BGMinfo bgm, float delay = 0)
    {
        currentBGM = bgm;
        Debug.Log(audioSource.clip);
        audioSource.clip = currentBGM.clip;
        audioSource.PlayDelayed(delay);
        interruptBGMpos = 0;
    }

    public void StopBGM()
    {
        interruptBGMpos = audioSource.time;
        audioSource.Stop();
    }

    public void RestartBGM()
    {
        if (currentBGM.clip == null) return;
        if (currentBGM.clip.length < interruptBGMpos) interruptBGMpos = 0;
        audioSource.time = interruptBGMpos;
        audioSource.Play();
    }

    public void PlaySE(AudioClip se)
    {
        if (SElenList.Count > maxSEcnt) return;

        Debug.Log(se.name);

        AudioClip clip = se;
        audioSource.PlayOneShot(clip);
        SElenList.Add(clip.length);
    }

    void UpdateSElist()
    {
        List<float> tempLenList = new List<float>();
        foreach (float len in SElenList)
        {
            float newLen = len - Time.deltaTime;
            if (newLen > 0)
            {
                tempLenList.Add(newLen);
            }
        }
        SElenList = tempLenList;
    }

    void LoopBGM()
    {
        if (audioSource.clip == null) return;

        if (currentBGM.loopEndSec > 0 && audioSource.time >= currentBGM.loopEndSec)
        {
            audioSource.time = currentBGM.loopBeginSec;
        }
    }

    ///enum名からresources.loadしたかったけど遅そうなのでやめた
    /*void LoadBGMs()
    {
        int len = BGMname.Wait.GetLength();
        AudioClip[] tempArray = new AudioClip[len];
        for(int i = 0; i < len; i++)
        {
            tempArray[i]=resource
        }
    }

    void LoadSEs()
    {

    }*/
}

public enum BGMname
{
    Wait,Main
}

public enum SEname
{
    Enter, Turn, Message
}