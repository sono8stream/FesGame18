using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowAnimation : MonoBehaviour
{
    [SerializeField]
    float defaultBoundScale;
    [SerializeField]
    int period;

    float boundScale;
    Counter boundCounter;

    // Use this for initialization
    void Start()
    {
        boundCounter = new Counter(period);
    }

    // Update is called once per frame
    void Update()
    {
        if (boundCounter.Count())
        {
            boundCounter.Initialize(period);
            boundScale = defaultBoundScale;
        }

        Bound();
    }

    void Bound()
    {
        float scale = 1
            + boundScale * Mathf.Sin(Mathf.PI * boundCounter.Now / boundCounter.Limit);
        transform.localScale = Vector3.one * scale;
    }

    public void BigBound(float newBoundScale,int newPeriod)
    {
        boundScale = newBoundScale;
        boundCounter.Initialize(newPeriod);
    }
}