using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class MathFuncs : MonoBehaviour
{
    public static int[] RandomDistictArray(int count, int lower, int upper)
    {
        if (count > upper - lower)
        {
            throw new ArgumentException("Requested array too large, or range too small.");
        }

        List<int> output = new List<int>(count);

        for (int i = 0; i < count; i++)
        {
            int num;
            do
            {
                num = Random.Range(lower,upper);
            }
            while (output.Contains(num));

            output.Add(num);
        }

        return output.ToArray();
    }
}