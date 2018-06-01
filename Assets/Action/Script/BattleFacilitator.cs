using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class BattleFacilitator : MonoBehaviour
{
    [SerializeField]
    Text leftTimeText;
    [SerializeField]
    float timerLim;
    [SerializeField]
    Player[] players;
    [SerializeField]
    BGMinfo bgm;
    [SerializeField]
    AudioClip startSE, stopSE;

    TimeCounter timer;
    bool onBattle;

    // Use this for initialization
    void Start()
    {
        leftTimeText.enabled = false;
        timer = new TimeCounter(timerLim);
        SoundPlayer.Find().PlayBGM(bgm);
        foreach (Player player in players)
        {
            player.playerController.enabled = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!onBattle) return;

        float leftSec = timerLim - timer.Now;
        leftTimeText.text = string.Format("{0:00} : {1:00.00}",
            leftSec / 60, leftSec - (int)(leftSec / 60) * 60);

        if (timer.OnLimit())
        {
            EndGame();
        }
    }

    public void StartBattle()
    {
        timer.Start();
        onBattle = true;
        foreach (Player player in players)
        {
            player.playerController.enabled = true;
        }
        leftTimeText.enabled = true;
    }

    public void CallResultScene()
    {
        LoadManager.Find().LoadScene(4, 1.5f);
    }

    public void PlayStartSE()
    {
        SoundPlayer.Find().PlaySE(startSE);
    }

    void EndGame()
    {
        timer.Pause();
        onBattle = false;
        foreach (Player player in players)
        {
            player.keyInput.isPlayable = false;
        }
        GetComponent<Animator>().SetTrigger("EndTrigger");
        leftTimeText.enabled = false;
        JudgeWinner();
        SoundPlayer s = SoundPlayer.Find();
        s.StopBGM();
        s.PlaySE(stopSE);
    }

    void JudgeWinner()
    {
        int winnerIndex = 0;
        int winnerMoney = 0;
        for (int i = 0; i < players.Length; i++)
        {
            PlayerStatus status = players[i].GetComponent<PlayerStatus>();
            Debug.Log(status.money);
            if (status.money > winnerMoney)
            {
                winnerMoney = status.money;
                winnerIndex = i;
            }
        }
        UserData.instance.winnerIndex = winnerIndex;
        UserData.instance.winnerMoney = winnerMoney;
        Debug.Log(winnerMoney);
    }
}