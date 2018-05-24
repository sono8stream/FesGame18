using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RectParamTester : MonoBehaviour
{
    [SerializeField]
    Rect rect;
    [SerializeField]
    float xMin;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        rect.xMin = xMin;
    }
}