using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameHelper
{
    public static T Random<T>(this List<T> list)
    {
        if (list == null || list.Count == 0)
            return default;

        return list[UnityEngine.Random.Range(0, list.Count)];
    }
}
