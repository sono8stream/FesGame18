using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class TweetGetter
{
    public Tweet[] Tweets { get; private set; }
    public bool DownloadEnded { get; private set; }

    public IEnumerator GetTweet()
    {
        DownloadEnded = false;
        UnityWebRequest request = UnityWebRequest.Get(
            "https://script.google.com/macros/s/AKfycbx14FEyVqlPhlLDOVbW-8FAN_xCEN6CcxJlE4FgvfOFdljhvbkY/exec");
        yield return request.SendWebRequest();

        DownloadEnded = true;
        if (request.isHttpError || request.isNetworkError)
        {
            Debug.Log(request.error);
        }
        else
        {
            if (request.responseCode != 200) yield break;

            Debug.Log(request.downloadHandler.text);
            string result = request.downloadHandler.text;
            string[] tweetTexts = result.Split('%');
            var tweets = new List<Tweet>();
            for (int i = 0; i < tweetTexts.Length; i++)
            {
                if (string.IsNullOrEmpty(tweetTexts[i])) continue;
                string[] tweet = tweetTexts[i].Split(':');
                string user = tweet[0].Substring(1);
                string message = tweet[1];
                tweets.Add(new Tweet(user, message));
                Debug.Log(string.Format("{0}:{1}", user, message));
            }
            Tweets = tweets.ToArray();
        }
    }
}