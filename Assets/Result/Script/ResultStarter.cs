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

	[SerializeField]
	Animator[] staggerAnimCharas;
	[SerializeField]
    Animator[] resultAnimCharas;


    // Use this for initialization
    void Start()
    {
        scoreText.text = string.Format("￥ {0:#,0}", UserData.instance.winnerMoney);
        SoundPlayer.Find().PlayBGM(bgm, 0.5f);
        //int winnerMatID = UserData.instance.winnerIndex - 1;
        int winnerMatID = 0;
        int looserMatID = winnerMatID == 0 ? 1 : 0;
        staggerAnimCharas[looserMatID].Play("stagger");
        Destroy(staggerAnimCharas[winnerMatID].gameObject);
        resultAnimCharas[winnerMatID].Play("result");
        Destroy(resultAnimCharas[looserMatID].gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            LoadManager.Find().LoadScene(5);
        }
    }
}