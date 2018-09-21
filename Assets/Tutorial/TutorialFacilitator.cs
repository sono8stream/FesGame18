using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TutorialFacilitator : MonoBehaviour
{
    [SerializeField]
    GameObject[] players;

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
            case 0:
                break;
        }
    }
}