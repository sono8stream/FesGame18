using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goal : MonoBehaviour
{
    int playerCnt;
    int goalCnt;

    private void Start()
    {
        playerCnt = 0;
        foreach(GameObject g in GameObject.FindGameObjectsWithTag("Player"))
        {
            if (g.activeSelf) playerCnt++;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            goalCnt++;
            if (goalCnt == playerCnt)
            {
                LoadManager.Find().LoadScene(10);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            goalCnt--;
        }
    }
}