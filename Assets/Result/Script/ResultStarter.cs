using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResultStarter : MonoBehaviour
{
    [SerializeField]
    Text scoreText;
    [SerializeField]
    BGMinfo bgm;

    // Use this for initialization
    void Start()
    {
        scoreText.text = string.Format("￥ {0:#,0}", UserData.instance.winnerScore);
        SoundPlayer player =
        GameObject.Find(SoundPlayer.objectName).GetComponent<SoundPlayer>();
        player.PlayBGM(bgm,2);
    }

    // Update is called once per frame
    void Update()
    {
        /*if (Input.GetKeyDown(KeyCode.A))
        {
            GetComponent<Animator>().SetTrigger("Flow");
        }*/
    }
}
