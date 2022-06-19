using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;

public static class FallOffGenerator
{
    public static List<float[,]> GenerateFallOff(int size, float fallOffRate, Serializables.FallOffType fallOffType,
        Serializables.FallOffDirection fallOffDirection = Serializables.FallOffDirection.X)
    {
        var map = new List<float[,]>(100);

        var tasks = new List<Task<float[,]>>(100);

        var fallOffOffsets = new Vector2[100];

        var index = 0;

        for (var j = 0; j < 10; j++)
        for (var i = 0; i < 10; i++)
        {
            fallOffOffsets[index] = new Vector2(size / 10 * i, size / 10 * j);
            index++;
        }

        for (var i = 0; i < 100; i++)
        {
            var i1 = i;
            var task = Task.Factory.StartNew(() =>
            {
                if (fallOffType == Serializables.FallOffType.Island)
                    return GenerateIslandParallel(size, fallOffRate, fallOffOffsets[99 - i1]);

                if (fallOffType == Serializables.FallOffType.Coast)
                    return GenerateCoastParallel(size, fallOffRate, fallOffDirection, fallOffOffsets[99 - i1]);

                return GenerateLakeParallel(size, fallOffRate, fallOffOffsets[99 - i1]);
            });
            tasks.Add(task);
        }

        Task.WaitAll(tasks.ToArray());

        foreach (var task in tasks) map.Add(task.Result);

        return map;
    }

    public static float[,] GenerateIslandParallel(int size, float fallOffRate, Vector2 offset)
    {
        var map = new float[size / 10, size / 10];

        for (var k = 0; k < size / 10; k++)
        for (var l = 0; l < size / 10; l++)
        {
            var x = (l + offset.y) / size * 2 - 1;
            var y = (k + offset.x) / size * 2 - 1;
            var value = Mathf.Max(Mathf.Abs(x), Mathf.Abs(y)) * fallOffRate * 2;
            map[l, k] = Evaluate(value);
        }

        return map;
    }

    public static float[,] GenerateCoastParallel(int size, float fallOffRate,
        Serializables.FallOffDirection fallOffDirection, Vector2 offset)
    {
        var map = new float[size / 10, size / 10];
        for (var i = 0; i < size / 10; i++)
        for (var j = 0; j < size / 10; j++)
        {
            var value = fallOffDirection == Serializables.FallOffDirection.X
                ? (i + offset.x) * fallOffRate / (float)(size / 10)
                : (j + offset.y) * fallOffRate / (float)(size / 10);
            value = Mathf.Abs(value);
            map[j, i] = value * value;
        }

        return map;
    }

    public static float[,] GenerateLakeParallel(int size, float fallOffRate, Vector2 offset)
    {
        var map = new float[size / 10, size / 10];
        float centerX = size / 2;
        float centerY = size / 2;
        for (var i = 0; i < size / 10; i++)
        for (var j = 0; j < size / 10; j++)
        {
            var x = Mathf.Abs((j - centerX + offset.y) * fallOffRate / (float)size);
            var y = Mathf.Abs((i - centerY + offset.x) * fallOffRate / (float)size);
            var value = Mathf.Sqrt(x * x + y * y);
            map[j, i] = 0.5f - value;
        }

        return map;
    }


    private static float Evaluate(float value)
    {
        var a = 3f;
        var b = 2.2f;

        return Mathf.Pow(value, a) / (Mathf.Pow(value, a) + Mathf.Pow(b - b * value, a));
    }
}