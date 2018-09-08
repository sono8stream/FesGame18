using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TweetRenderer : MonoBehaviour
{
    [SerializeField]
    GameObject tweetObjOrigin;

    Queue<GameObject> renderingTweetObjs;
    TweetGetter getter;
    Counter tweetCounter;

    // Use this for initialization
    void Start()
    {
        renderingTweetObjs = new Queue<GameObject>();
        getter = new TweetGetter();
        StartCoroutine(getter.GetTweet());
        tweetCounter = new Counter(10);
    }

    // Update is called once per frame
    void Update()
    {
        if (!getter.DownloadEnded)
        {
            Debug.Log("notDownload");
            return;
        }

        MoveTweets();

        if (!Input.GetKeyDown(KeyCode.Space)) return;

        Debug.Log(getter.Tweets.Length);
        renderingTweetObjs.Enqueue(GenerateTweet(getter.Tweets[tweetCounter.Now]));
        tweetCounter.Count();
    }

    GameObject GenerateTweet(Tweet tweet)
    {
        Debug.Log(tweet.message);
        GameObject obj = Instantiate(tweetObjOrigin, transform);
        obj.GetComponent<Text>().text = tweet.message;
        obj.GetComponent<RectTransform>().localPosition
            = Vector3.up * Random.Range(-300, 300);
        return obj;
    }

    void MoveTweets()
    {
        float speed = 5f;
        foreach (GameObject objs in renderingTweetObjs)
        {
            objs.GetComponent<RectTransform>().localPosition += Vector3.left * speed;
        }
    }
}