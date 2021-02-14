using System.Collections.Generic;
using Random = System.Random;
using UnityEngine;

public static class Utility 
{
    private static readonly Random rng = new Random();
    public static Vector3 halfVector = new Vector3(0.5f, 0.5f, 0f);
    public static void Shuffle<T>(this IList<T> list)
    {
        int n = list.Count;
        while (n > 1)
        {
            n--;
            int k = rng.Next(n + 1);
            T value = list[k];
            list[k] = list[n];
            list[n] = value;
        }
    }
}
