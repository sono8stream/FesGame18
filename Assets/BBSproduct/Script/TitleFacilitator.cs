using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleFacilitator : MonoBehaviour
{
    [SerializeField]
    BGMinfo bgm;
    [SerializeField]
    float prologueInterval;

    TimeCounter timer;

    void Start()
    {
        SoundPlayer.Find().PlayBGM(bgm);
        timer = new TimeCounter(prologueInterval);
        timer.Start();
    }

    // Update is called once per frame
    void Update()
    {
        if (timer.OnLimit())
        {
            LoadManager.Find().LoadScene(1);
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            LoadManager.Find().LoadScene(3);
        }
    }
}
