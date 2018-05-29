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
	Animator staggerAnimChara;
	[SerializeField]
    Animator resultAnimChara;


    // Use this for initialization
    void Start()
    {
        scoreText.text = string.Format("￥ {0:#,0}", UserData.instance.winnerMoney);
        SoundPlayer.Find().PlayBGM(bgm, 0.5f);
		staggerAnimChara.Play("stagger");
		resultAnimChara.Play("result");
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