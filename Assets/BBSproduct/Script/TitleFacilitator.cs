using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleFacilitator : MonoBehaviour
{
    [SerializeField]
    BGMinfo bgm;
    [SerializeField]
    AudioClip enterSE;
    [SerializeField]
    float prologueInterval;
    [SerializeField]
    Transform startTextTransform;

    TimeCounter timer;
    bool onBattle;
    Counter transitionCounter;

    void Start()
    {
        SoundPlayer.Find().PlayBGM(bgm);
        timer = new TimeCounter(prologueInterval);
        timer.Start();
        onBattle = false;
        transitionCounter = new Counter(50);
    }

    // Update is called once per frame
    void Update()
    {
        if (onBattle)
        {
            transitionCounter.Count();
            if (transitionCounter.Now == transitionCounter.Limit / 2)
            {
                LoadManager.Find().LoadScene(3);
            }
            startTextTransform.localScale
                = Vector3.one * (transitionCounter.Now % 2);
            return;
        }

        if (timer.OnLimit())
        {
            LoadManager.Find().LoadScene(1);
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            onBattle = true;
            SoundPlayer.Find().PlaySE(enterSE);
        }
    }
}
