using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TweetRenderer : MonoBehaviour
{
    [SerializeField]
    GameObject tweetObjOrigin;
    [SerializeField]
    int tweetCnt;
    [SerializeField]
    float tweetHeight;
    [SerializeField]
    int renderTweetLim;

    Queue<GameObject> renderingTweetObjs;
    Queue<int> showingIndexes;
    ObjectPool tweetObjPool;
    bool[] showingHeights;
    TweetGetter getter;
    Counter tweetCounter;
    float screenWidth;

    // Use this for initialization
    void Start()
    {
        renderingTweetObjs = new Queue<GameObject>();
        showingIndexes = new Queue<int>();
        tweetObjPool = new ObjectPool(renderTweetLim);
        showingHeights = new bool[renderTweetLim];
        getter = new TweetGetter();
        StartCoroutine(getter.GetTweet(tweetCnt));
        tweetCounter = new Counter(tweetCnt);
        screenWidth = GetComponent<RectTransform>().sizeDelta.x;
    }

    // Update is called once per frame
    void Update()
    {
        if (!getter.DownloadEnded) return;

        MoveTweets();

        if (!Input.GetKeyDown(KeyCode.Space)) return;

        GenerateTweet(getter.Tweets[tweetCounter.Now]);
        if (tweetCounter.Count()) tweetCounter.Initialize();
    }

    void GenerateTweet(Tweet tweet)
    {
        if (renderTweetLim == renderingTweetObjs.Count) return;

        Debug.Log(tweet.message);
        GameObject obj
            = tweetObjPool.Pop() ?? Instantiate(tweetObjOrigin, transform);
        Text text = obj.GetComponent<Text>();
        text.text = tweet.message;
        int index = Array.IndexOf(showingHeights, false);

        RectTransform rectT = obj.GetComponent<RectTransform>();
        rectT.sizeDelta = new Vector2(text.preferredWidth, rectT.sizeDelta.y);
        rectT.localPosition
            = Vector3.right * (text.preferredWidth + screenWidth) * 0.5f
            + Vector3.up * (renderTweetLim / 2 - index) * tweetHeight;

        renderingTweetObjs.Enqueue(obj);
        showingHeights[index] = true;
        showingIndexes.Enqueue(index);
    }

    void MoveTweets()
    {
        DeletePeekTweetObject();

        float speed = 10f;
        foreach (GameObject objs in renderingTweetObjs)
        {
            RectTransform rectT = objs.GetComponent<RectTransform>();
            rectT.localPosition += Vector3.left * speed;
        }
    }

    void DeletePeekTweetObject()
    {
        if (renderingTweetObjs.Count == 0) return;

        RectTransform rectT
            = renderingTweetObjs.Peek().GetComponent<RectTransform>();
        if (rectT.localPosition.x + rectT.sizeDelta.x * 0.5f < -screenWidth * 0.5f)
        {
            tweetObjPool.Push(renderingTweetObjs.Dequeue());
            showingHeights[showingIndexes.Dequeue()] = false;
        }
    }
}