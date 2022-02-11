using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class FallOffGenerator
{
    public static float[,] GenerateIsland(int size, float fallOffRate)
    {
        float[,] map = new float[size, size];
        for (int i = 0; i < size; i++)
        {
            for (int j = 0; j < size; j++)
            {
                float x = i / (float)size * 2 - 1;
                float y = j / (float)size * 2 - 1;
                float value = Mathf.Max(Mathf.Abs(x), Mathf.Abs(y)) * fallOffRate;
                map[i, j] = Evaluate(value);
            }
        }

        return map;
    }

    public static float[,] GenerateCoast(int size, float fallOffRate, Serializables.FallOffDirection fallOffDirection)
    {
        float[,] map = new float[size, size];
        for (int i = 0; i < size; i++)
        {
            for (int j = 0; j < size; j++)
            {
                float value = (fallOffDirection == Serializables.FallOffDirection.X) ? (i * fallOffRate) / (float)size : (j * fallOffRate) / (float)size;
                value = Mathf.Abs(value);
                map[i, j] = value * value;
            }
        }

        return map;
    }

    public static float[,] GenerateLake(int size, float fallOffRate)
    {
        float[,] map = new float[size, size];
        float centerX = size / 2;
        float centerY = size / 2;
        for (int i = 0; i < size; i++)
        {
            for (int j = 0; j < size; j++)
            {
                float x = Mathf.Abs((i - centerX) * fallOffRate / (float)size);
                float y = Mathf.Abs((j - centerY) * fallOffRate / (float)size);
                float value = Mathf.Sqrt(x * x + y * y);
                map[i, j] = 0.5f - value;
            }
        }

        return map;
    }


    static float Evaluate(float value)
    {
        float a = 3f;
        float b = 2.2f;

        return Mathf.Pow(value, a) / (Mathf.Pow(value, a) + Mathf.Pow((b - b * value), a));
    }
}
