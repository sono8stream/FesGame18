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
        floorWidth = GetComponent<RectTransform>().sizeDelta.x;
        text = transform.Find("Text").GetComponent<Text>();
        text.text += "     ";
        textWidth = text.preferredWidth;
        do
        {
            text.text += text.text;
        } while (text.preferredWidth < floorWidth * 2);
        text.rectTransform.sizeDelta
            = new Vector2(text.preferredWidth, text.preferredHeight);
        text.rectTransform.localPosition = Vector3.left * floorWidth / 2;
        limitWaiter = new Counter(50);
    }

    // Update is called once per frame
    void Update()
    {
        if (onScroll)
        {
            text.transform.localPosition += Vector3.left * scrollSpeed;
            if (text.transform.localPosition.x + textWidth < -floorWidth / 2)
            {
                text.transform.localPosition
                    += Vector3.right * textWidth;
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