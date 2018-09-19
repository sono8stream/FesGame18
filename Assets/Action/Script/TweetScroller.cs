using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TweetScroller : MonoBehaviour
{
    [SerializeField]
    float scrollSpeed;

    Text text;
    float textWidth;
    float floorWidth;
    Counter limitWaiter;
    bool onScroll;

    // Use this for initialization
    void Start()
    {
        text = transform.Find("Text").GetComponent<Text>();
        text.rectTransform.sizeDelta
            = new Vector2(text.preferredWidth, text.preferredHeight);
        textWidth = text.preferredWidth;
        floorWidth = GetComponent<RectTransform>().sizeDelta.x;
        limitWaiter = new Counter(50);
    }

    // Update is called once per frame
    void Update()
    {
        if (onScroll)
        {
            text.transform.localPosition += Vector3.left * scrollSpeed;
            if (text.transform.localPosition.x == -floorWidth / 2)
            {
                onScroll = false;
            }
            if (text.transform.localPosition.x + textWidth < -floorWidth / 2)
            {
                text.transform.localPosition 
                    += Vector3.right * (textWidth + floorWidth);
            }
        }
        else if (limitWaiter.Count())
        {
            onScroll = true;
            limitWaiter.Initialize();
        }
    }

    public void UpdateText(string str)
    {
        text.text = str;
        text.rectTransform.sizeDelta
            = new Vector2(text.preferredWidth, text.preferredHeight);
        textWidth = text.preferredWidth;
    }
}