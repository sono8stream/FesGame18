using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TutorialFacilitator : MonoBehaviour
{
    [SerializeField]
    GameObject[] players;
    [SerializeField]
    StandManager[] stands;
    [SerializeField]
    GameObject[] walls;
    [SerializeField]
    GameObject[] bombGenerators;
    [SerializeField]
    GameObject[] floors;

    int playerCnt;
    int stateNo;

    // Use this for initialization
    void Start()
    {
        playerCnt = players.Count(x => x.activeSelf);
        stateNo = 0;
    }

    // Update is called once per frame
    void Update()
    {
        switch (stateNo)
        {
            //屋台配置
            case 0:
                if (stands.Count(x => x.isStand) == playerCnt)
                {
                    stateNo = 1;
                    foreach(GameObject g in walls)
                    {
                        g.SetActive(false);
                    }
                    foreach(GameObject g in bombGenerators)
                    {
                        g.SetActive(true);
                    }
                }
                break;
                //爆弾使用
            case 1:
                if (stands.Count(x => !x.isStand) == 0)
                {
                    stateNo = 2;
                    foreach(GameObject g in floors)
                    {
                        g.SetActive(true);
                    }
                }
                break;
                //ゴールへ向かう
            case 2:
                break;
        }
    }
}