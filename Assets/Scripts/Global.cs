using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public static class Global
{
    public static Vector2 globalIndex = new Vector2(999,999);
    public static bool tileUpdate = false;
    public static int selectedTileNumber = 0;
    
    public static T[] To1DArray<T>(T[,] input)
    {
        int height = input.GetLength(0);
        int width = input.GetLength(1);
        T[] result = new T[height*width];
        
        for (int i = 0; i < height; i++)
        {
            for (int j = 0; j < width; j++)
            {
                result[i * width + j] = input[i, j];
            }
        }
        return result;
    }
    
    public static T[,] To2DArray<T>(T[] input, int height, int width)
    {
        T[,] output = new T[height, width];
        for (int i = 0; i < height; i++)
        {
            for (int j = 0; j < width; j++)
            {
                output[i, j] = input[i * width + j];
            }
        }
        return output;
    }
    
}