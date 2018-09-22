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

    // Use this for initialization
    void Start()
    {
        playerCnt = players.Count(x => x.gameObject.activeSelf);
        stateNo = 0;
        SoundPlayer.Find().PlayBGM(bgm);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            LoadManager.Find().LoadScene(8);
        }

        switch (stateNo)
        {
            //屋台配置
            case 0:
                if (stands.Count(x => x.isStand) == playerCnt)
                {
                    stateNo = 1;
                    standUpImageObj.SetActive(false);
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
                    bombImageObj.SetActive(true);
                }
                break;
                //爆弾使用
            case 2:
                if (stands.Count(x => x.isStand) < playerCnt)
                {
                    stateNo = 3;
                    floor.SetActive(true);
                    bombImageObj.SetActive(false);
                }
                break;
                //ゴールへ向かう
            case 3:
                break;
        }
    }
}