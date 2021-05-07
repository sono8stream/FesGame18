using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonScript : MonoBehaviour
{
    Waiter w;
    Image i;

    // Use this for initialization
    void Start()
    {
        w = new Waiter(100);
        i = GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        if (w.Wait()) { w.Initialize(); }
        i.color = Color.white
            * (0.95f + 0.05f * Mathf.Cos(2 * Mathf.PI * w.Count / w.Limit));
    }
}
