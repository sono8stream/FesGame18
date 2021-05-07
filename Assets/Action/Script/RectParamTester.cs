using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RectParamTester : MonoBehaviour
{
    [SerializeField]
    Rect rect;
    [SerializeField]
    float xMin;
    [SerializeField]
    float xMax;
    [SerializeField]
    float x;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        rect.xMin = xMin;
        //rect.xMax = xMax;
        //rect.x = x;
    }
}