using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResultStarter : MonoBehaviour
{
    [SerializeField]
    Text scoreText;
    [SerializeField]
    Text winnerText;
    [SerializeField]
    BGMinfo bgm;
    [SerializeField]
    AudioClip enterSE;

    [SerializeField]
    Animator[] staggerAnimCharas;
    [SerializeField]
    Animator[] resultAnimCharas;

    Counter counter;
    bool onTransition;

    // Use this for initialization
    void Start()
    {
        scoreText.text = string.Format("￥ {0:#,0}", UserData.instance.winnerMoney);
        SoundPlayer.Find().PlayBGM(bgm, 0.5f);
        int winnerMatID = UserData.instance.winnerIndex;
        if (winnerMatID == -1) winnerMatID = 0;
        int looserMatID = winnerMatID == 0 ? 1 : 0;
        winnerText.text = string.Format("Player {0} WIN !!",
            UserData.instance.winnerIndex + 1);
        staggerAnimCharas[looserMatID].Play("stagger");
        Destroy(staggerAnimCharas[winnerMatID].gameObject);
        resultAnimCharas[winnerMatID].Play("result");
        Destroy(resultAnimCharas[looserMatID].gameObject);
        counter = new Counter(60);
    }

    // Update is called once per frame
    void Update()
    {
        if (onTransition && counter.Count())
        {
            LoadManager.Find().LoadScene(5);
            counter.Initialize();
        }


        if (Input.GetKeyDown(KeyCode.Space))
        {
            onTransition = true;
            SoundPlayer.Find().PlaySE(enterSE);
        }
    }
}