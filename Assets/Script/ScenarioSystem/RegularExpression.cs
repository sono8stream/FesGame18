using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RegularExpression : MonoBehaviour
{
    public Dictionary<string, int> regularExpression;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void InitializeRE()
    {
        regularExpression = new Dictionary<string, int>();
        regularExpression.Add("[r]", 0);
    }
}

