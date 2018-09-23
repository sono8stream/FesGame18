using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using Anima2D;
using KoitanLib;

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
    Material[] borderMaterial;

    [SerializeField]
    Animator[] staggerAnimCharas;
    [SerializeField]
    Animator[] resultAnimCharas;

    Counter counter;
    bool toNextTransition, onRetryTransition;
    TimeCounter autoTransitTimer;

    // Use this for initialization
    void Start()
    {
        scoreText.text = string.Format("￥ {0:#,0}", UserData.instance.winnerMoney);
        SoundPlayer.Find().PlayBGM(bgm, 0.5f);
        int winnerMatID = UserData.instance.winnerIndex;
        if (winnerMatID == -1) winnerMatID = 1;
        DecideWinner(winnerMatID);
        int looserMatID = winnerMatID == 0 ? 1 : 0;
        winnerText.text = string.Format("Player {0} WIN !!",
            UserData.instance.winnerIndex + 1);
        counter = new Counter(60);
        autoTransitTimer = new TimeCounter(5);
        autoTransitTimer.Start();
    }

    // Update is called once per frame
    void Update()
    {
        if (toNextTransition || onRetryTransition || autoTransitTimer.OnLimit())
        {
            if (counter.Count())
            {
                //LoadManager.Find().LoadScene(5);
                //int sceneIndex = toNextTransition ? 0 : 8;
                int sceneIndex = 6;
                LoadManager.Find().LoadScene(sceneIndex);
                counter.Initialize();
            }
            return;
        }

        if (KoitanInput.GetButtonDown(ButtonID.A))
        {
            toNextTransition = true;
            SoundPlayer.Find().PlaySE(enterSE);
        }
        if (KoitanInput.GetButtonDown(ButtonID.Y))
        {
            onRetryTransition = true;
            SoundPlayer.Find().PlaySE(enterSE);
        }
    }

    void DecideWinner(int winnerIndex)
    {
        int charaIndex = (int)UserData.instance.characters[winnerIndex];
        for(int i = 0; i < resultAnimCharas.Length; i++)
        {
            if (charaIndex == i)
            {
                Transform parent = resultAnimCharas[i].transform.Find("mesh");
                for (int j = 0; j < parent.childCount - 5; j++)
                {
                    parent.GetChild(j).GetComponent<SpriteMeshInstance>().sharedMaterial 
                        = borderMaterial[winnerIndex];
                }
                resultAnimCharas[i].Play("boy_1_win");
            }
            else
            {
                resultAnimCharas[i].gameObject.SetActive(false);
            }
        }
    }
}