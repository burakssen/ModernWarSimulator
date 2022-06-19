using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class TextureGenerator
{
    public static Texture2D TextureFromColorMap(Color[] colorMap, int width, int height)
    {
        var texture = new Texture2D(width, height);
        texture.filterMode = FilterMode.Point;
        texture.wrapMode = TextureWrapMode.Clamp;
        texture.SetPixels(colorMap);
        texture.Apply();
        return texture;
    }

    public static Texture2D TextureFromFallOffMap(float[,] heightMap)
    {
        var width = heightMap.GetLength(0);
        var height = heightMap.GetLength(1);

        var colorMap = new Color[width * height];

        for (var y = 0; y < height; y++)
        for (var x = 0; x < width; x++)
            colorMap[y * width + x] = Color.Lerp(Color.black, Color.white, heightMap[y, x]);

        return TextureFromColorMap(colorMap, width, height);
    }
}