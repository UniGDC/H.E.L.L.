using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Random = System.Random;

public class MathUtils : MonoBehaviour
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
                num = new Random().Next(lower, upper);
            }
            while (!output.Contains(num));

            output.Add(num);
        }

        return output.ToArray();
    }
}