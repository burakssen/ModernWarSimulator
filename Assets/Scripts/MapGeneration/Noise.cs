using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System;
using UnityEngine;

public static class Noise
{
    public enum NormalizeMode
    {
        Local,
        Global
    };

    [DllImport("PerlinNoise", CallingConvention = CallingConvention.Cdecl)]
    private static extern void PerlinNoise(int mapWidth, int mapHeight, int seed, float scale, int octaves,
        float persistance, float lacunarity, Vector2 offset, Vector2[] octaveOffsets, float[,] noiseMap,
        NormalizeMode normalizeMode);

    public static float[,] GenerateNoiseMap(int mapWidth, int mapHeight, int seed, float scale, int octaves,
        float persistance, float lacunarity, Vector2 offset, NormalizeMode normalizeMode)
    {
        var noiseMap = new float[mapWidth, mapHeight];

        var octaveOffsets = new Vector2[octaves];

        PerlinNoise(mapWidth, mapHeight, seed, scale, octaves, persistance, lacunarity, offset, octaveOffsets, noiseMap,
            normalizeMode);

        return noiseMap;
    }
}