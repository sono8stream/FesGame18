using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TweetUpdater : MonoBehaviour
{

    [SerializeField]
    TweetScroller[] scrollers;
    [SerializeField]
    int updateInterval;

    TweetGetter getter;
    Counter tweetChangeCounter;
    Counter nowIndexCounter;

    // Use this for initialization
    void Start()
    {
        getter = new TweetGetter();
        StartCoroutine(getter.GetTweet(20));
        tweetChangeCounter = new Counter(updateInterval);
        nowIndexCounter = new Counter(scrollers.Length);
    }

    // Update is called once per frame
    void Update()
    {
        if (tweetChangeCounter.Count())
        {
            tweetChangeCounter.Initialize();
            UpdateTweetText(nowIndexCounter.Now);
            if (nowIndexCounter.Count())
            {
                nowIndexCounter.Initialize();
            }
        }
    }

    void UpdateTweetText(int index)
    {
        Tweet newTweet = getter.Next();
        scrollers[index].UpdateText(newTweet.message);
    }
}