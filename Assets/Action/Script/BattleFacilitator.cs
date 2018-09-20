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
    [SerializeField]
    TeamColor[] teamColors;
    [SerializeField]
    Material[] borderMaterials;
    [SerializeField]
    Transform[] statusUIs;

    TimeCounter timer;
    bool onBattle;

    private void Awake()
    {
        var indexes = new Queue<int>();
        for(int i = 0; i < players.Length; i++)
        {
            bool isSelected = false;
            for(int j = 0; j < UserData.instance.playerCount; j++)
            {
                int index = (int)UserData.instance.characters[j];
                if (index == i)
                {
                    Debug.Log(j);
                    players[i].SetTeam(teamColors[j], borderMaterials[j], statusUIs[j], j);
                    isSelected = true;
                    indexes.Enqueue(j);
                    break;
                }
            }
            if (!isSelected)
            {
                players[i].gameObject.SetActive(false);
            }
        }

        for(int i = 0; i < players.Length; i++)
        {
            if (indexes.Contains(i)) continue;

            statusUIs[i].gameObject.SetActive(false);
        }
    }

    // Use this for initialization
    void Start()
    {
        leftTimeText.enabled = false;
        timer = new TimeCounter(timerLim);
        SoundPlayer.Find().StopBGM();
    }

    // Update is called once per frame
    void Update()
    {
        if (!onBattle) return;

        float leftSec = timerLim - timer.Now;
        leftTimeText.text = string.Format("{0:00} : {1:00.00}",
            Mathf.FloorToInt(leftSec / 60), leftSec - (int)(leftSec / 60) * 60);

        if (timer.OnLimit())
        {
            EndGame();
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            StopGame();
            LoadManager.Find().LoadScene(0);
        }
    }

    public void StartBattle()
    {
        timer.Start();
        onBattle = true;
        foreach (Player player in players)
        {
            if (player.gameObject.activeSelf)
            {
                player.playerController.isPlayable = true;
            }
        }
        leftTimeText.enabled = true;
        SoundPlayer.Find().PlayBGM(bgm);
    }

    public void CallResultScene()
    {
        LoadManager.Find().LoadScene(4, 1.5f);
    }

    public void PlayStartSE()
    {
        SoundPlayer.Find().PlaySE(startSE);
    }

    void StopGame()
    {
        timer.Pause();
        onBattle = false;
        foreach (Player player in players)
        {
            player.playerController.isPlayable = false;
        }
    }

    void EndGame()
    {
        StopGame();
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
                winnerIndex = players[i].PlayerID;
            }
        }
        UserData.instance.winnerIndex = winnerIndex;
        UserData.instance.winnerMoney = winnerMoney;
        Debug.Log(winnerMoney);
    }
}