using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TweetUpdater : MonoBehaviour
{

    [SerializeField]
    Text[] tweetTexts;

    TweetScroller[] scrollers;
    RectTransform[] tweetRects;
    BoxCollider2D[] tweetColliders;

    TweetGetter getter;
    Counter tweetChangeCounter;
    Counter nowIndexCounter;

    // Use this for initialization
    void Start()
    {
        getter = new TweetGetter();
        StartCoroutine(getter.GetTweet(20));
        tweetChangeCounter = new Counter(200);
        nowIndexCounter = new Counter(tweetTexts.Length);

        scrollers = new TweetScroller[tweetTexts.Length];
        tweetRects = new RectTransform[tweetTexts.Length];
        tweetColliders = new BoxCollider2D[tweetTexts.Length];
        for (int i = 0; i < tweetTexts.Length; i++)
        {
            scrollers[i] = tweetTexts[i].GetComponent<TweetScroller>();
            tweetRects[i] = tweetTexts[i].GetComponent<RectTransform>();
            tweetColliders[i] = tweetTexts[i].GetComponent<BoxCollider2D>();
        }
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
        Text text = tweetTexts[index];
        text.text = newTweet.message.Substring(0, 15);
        if (newTweet.message.Length > 15)
        {
            text.text += "...";
        }
        Vector2 size = new Vector2(text.preferredWidth, text.preferredHeight);
        tweetRects[index].sizeDelta = size;
        tweetColliders[index].size = size;
    }
}