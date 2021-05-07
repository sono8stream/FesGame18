using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PopupMessage : MonoBehaviour
{
    [SerializeField]
    float amplitude = 10;

    Counter counter;

    // Use this for initialization
    void Start()
    {
        counter = new Counter(100);
    }

    // Update is called once per frame
    void Update()
    {
        if (counter.Count())
        {
            counter.Initialize();
        }
        float phase = 2 * Mathf.PI * counter.Now / counter.Limit;
        transform.localPosition += Vector3.up * amplitude * Mathf.Cos(phase);
    }
}