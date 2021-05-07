using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class IntUtils
{
    public static bool Iterate(ref int val, int max, int min = 0)
    {
        val++;
        if (val >= max)
        {
            val = min;
            return true;
        }
        return false;
    }
}
