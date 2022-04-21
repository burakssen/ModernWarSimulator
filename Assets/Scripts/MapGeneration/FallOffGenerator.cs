using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public static class FallOffGenerator
{
    public static List<float[,]> GenerateIsland(int size, float fallOffRate)
    {
        List<float[,]> map = new List<float[,]>(100);

        List<Task<float[,]>> tasks = new List<Task<float[,]>>(100);

        Vector2[] fallOffOffsets = new Vector2[100];
        
        int index = 0;
        
        for (int j = 0; j < 10; j++)
        {
            for (int i = 0; i < 10; i++)
            {
                fallOffOffsets[index] = new Vector2((size / 10) * i, (size / 10) * j);
                index++;
            }
        }
        
        for (int i = 0; i < 100; i++)
        {
            int i1 = i;
            Task<float[,]> task = Task.Factory.StartNew(() => GenerateIslandParallel(size, fallOffRate, fallOffOffsets[99 - i1]));
            tasks.Add(task);
        }

        Task.WaitAll(tasks.ToArray());

        foreach (var task in tasks)
        {
            map.Add(task.Result);
        }
        
        return map;
    }

    public static float[,] GenerateIslandParallel(int size, float fallOffRate, Vector2 offset)
    {
        float[,] map = new float[(size / 10), (size / 10)];
        
        for (int k = 0; k < size / 10; k++)
        {
            for (int l = 0; l < size / 10; l++)
            {
                float x = (l + offset.y) / size * 2 - 1;
                float y = (k + offset.x) / size * 2 - 1;
                float value = Mathf.Max(Mathf.Abs(x), Mathf.Abs(y)) * fallOffRate * 2;
                map[l, k] = Evaluate(value);
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
