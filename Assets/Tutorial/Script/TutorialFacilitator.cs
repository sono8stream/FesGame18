using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TutorialFacilitator : MonoBehaviour
{
    [SerializeField]
    Player[] players;
    [SerializeField]
    StandManager[] stands;
    [SerializeField]
    GameObject standUpImageObj;
    [SerializeField]
    GameObject bombImageObj;
    [SerializeField]
    GameObject wall;
    [SerializeField]
    GameObject bombGenerator;
    [SerializeField]
    GameObject floor;
    [SerializeField]
    BGMinfo bgm;

    int playerCnt;
    int stateNo;
    [SerializeField]
    int bombCnt;
    [SerializeField]
    bool[] haveBomb;

    // Use this for initialization
    void Start()
    {
        playerCnt = players.Count(x => x.gameObject.activeSelf);
        stateNo = 0;
        SoundPlayer.Find().PlayBGM(bgm);
        haveBomb = new bool[playerCnt];
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            LoadManager.Find().LoadScene(6);
        }

        switch (stateNo)
        {
            //屋台配置
            case 0:
                if (stands.Count(x => x.isStand) == playerCnt)
                {
                    stateNo = 1;
                }
                break;
            //お金獲得
            case 1:
                if (players.Count(x =>
                {
                    return x.gameObject.activeSelf && x.Status.money > 0;
                }) == playerCnt)
                {
                    stateNo = 2;
                    wall.SetActive(false);
                    bombGenerator.SetActive(true);
                }
                break;
                //爆弾を使用
            case 2:
                for(int i = 0; i < playerCnt; i++)
                {
                    if (!players[i].gameObject.activeSelf) continue;

                    bool temp = players[i].havingItem;
                    if (haveBomb[i] && !temp)
                    {
                        bombCnt++;
                    }
                    haveBomb[i] = temp;
                }
                if (bombCnt >=4)
                {
                    floor.SetActive(true);
                    GetComponent<Animator>().enabled = true;
                    //GetComponent<Animator>().Play("Idle");
                    GetComponent<BattleFacilitator>().enabled = true;
                    transform.Find("Canvas").Find("TimerText").gameObject.SetActive(true);
                    foreach (Player player in players)
                    {
                        if (player.gameObject.activeSelf)
                        {
                            player.playerController.isPlayable = false;
                        }
                    }
                    this.enabled = false;
                }
                break;
        }
    }
}