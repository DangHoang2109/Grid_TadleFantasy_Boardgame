using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameHelper
{
    public static T Random<T>(this T[] array)
    {
        if (array == null || array.Length == 0)
            return default;

        if (array.Length == 1) return array[0];

        return array[UnityEngine.Random.Range(0, array.Length)];
    }
    public static T Random<T>(this List<T> list)
    {
        if (list == null || list.Count == 0)
            return default;

        return list[UnityEngine.Random.Range(0, list.Count)];
    }
}
