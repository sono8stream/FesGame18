using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Waiterと同じ機能で、名前変えただけ
/// </summary>
public class Counter
{
    public int Limit { get; private set; }
    public int Now { get; set; }

    public Counter(int Limit, bool max = false)
    {
        Initialize(Limit, max);
    }

    public bool Count(int increment = 1)
    {
        Now += increment;
        Now = Now > Limit ? Limit : Now;
        return OnLimit();
    }

    public bool OnLimit()
    {
        return Now == Limit;
    }

    public void Initialize(int newLimit = -1, bool max = false)
    {
        Limit = newLimit == -1 ? Limit : newLimit;
        Now = max ? Limit : 0;
    }
}
