using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

/// <summary>
/// インスタンスを生成すれば、Update()で呼び出してwait
/// </summary>
public class Waiter
{
    int lim, count;
    public int Limit { get { return lim; } }
    public int Count { get { return count; } }

    public Waiter(int lim, bool max = false)
    {
        Initialize(lim,max);
    }

    public bool Wait()
    {
        count = count < lim ? count + 1 : lim;
        return count == lim;
    }

    public void Initialize(int newLim = -1, bool max = false)
    {
        lim = newLim == -1 ? lim : newLim;
        count = max ? lim : 0;
    }
}
