using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;


public class MapGenerator : MonoBehaviour
{
    [SerializeField] bool autoUpdate;
    [SerializeField] Serializables.UIInputs uiInputs;
    [SerializeField] Serializables.MapArgs mapArguments;
    [SerializeField] Image miniMap;
    [SerializeField] Noise.NormalizeMode normalizeMode;
    float[,] fallOffMap;
    float[,] heightMap;
    
    void Awake()
    {
        SetFallOffMap();
    }

    public void DrawColorMap(MapData mapData, MapDisplay display)
    {
        display.DrawTexture(TextureGenerator.TextureFromColorMap(mapData.colorMap, Int32.Parse(uiInputs.size.text), Int32.Parse(uiInputs.size.text)));
        miniMap.GetComponent<Image>().enabled = false;
        miniMap.GetComponent<Image>().enabled = true;
    }

    public void DrawFallOffMap(MapData mapData, MapDisplay display)
    {
        display.DrawTexture(TextureGenerator.TextureFromFallOffMap(FallOffGenerator.GenerateCoast(Int32.Parse(uiInputs.size.text), uiInputs.fallOffRate.value, (Serializables.FallOffDirection)uiInputs.fallOffDirection.value)));
    }

    public void DrawMeshMap(MapData mapData, MapDisplay display)
    {
        int rows = mapData.heightMap.GetLength(0);
        int cols = mapData.heightMap.GetLength(1);

        float[,] flippedHeightMap = new float[rows, cols];
        Color[] flippedColorMap = new Color[rows * cols];
        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < cols; j++)
            {
                flippedHeightMap[i, j] = mapData.heightMap[(rows - 1) - i, j];
                flippedColorMap[i * cols + j] = mapData.colorMap[i * cols + (cols - 1) - j];
            }
        }

        display.DrawMesh(
            MeshGenerator.GenerateTerrainMesh(
                flippedHeightMap, 
                mapArguments.meshHeightMultiplier, 
                mapArguments.meshHeightCurve, 
                mapArguments.editorPreivewLevelOfDetail
            ), 
            TextureGenerator.TextureFromColorMap(
                flippedColorMap, 
                Int32.Parse(uiInputs.size.text), 
                Int32.Parse(uiInputs.size.text)
            )
        );
    }

    public float [,] GetHeightMap(){
        return heightMap;
    }


    public void SetHeightMap(MapData mapData){
        heightMap = mapData.heightMap;
    }

    public void DrawMapInEditor()
    {
        MapData mapData = GenerateMapData(Vector2.zero);
        MapDisplay display = FindObjectOfType<MapDisplay>();

        if (mapArguments.drawMode == Serializables.DrawMode.ColorMap)
        {
            DrawColorMap(mapData, display);
        }
        else if (mapArguments.drawMode == Serializables.DrawMode.Mesh)
        {
            DrawMeshMap(mapData, display);
        }
        else if (mapArguments.drawMode == Serializables.DrawMode.FallOff)
        {
            DrawFallOffMap(mapData, display);
        }
    }

    MapData GenerateMapData(Vector2 center){
        
        if (uiInputs.useFallOff.isOn)
        {
            SetFallOffMap();
        }

        int size = Int32.Parse(uiInputs.size.text);

        Vector2 offset = new Vector2(float.Parse(uiInputs.offsetX.text), float.Parse(uiInputs.offsetY.text));
        
        Color[] colorMap = new Color[size * size];

        float[,] noiseMap = Noise.GenerateNoiseMap(
            size, 
            size, 
            Int32.Parse(uiInputs.seed.text), 
            mapArguments.noiseScale, 
            mapArguments.octaves, 
            mapArguments.persistance, 
            mapArguments.lacunarity, 
            center + offset, 
            normalizeMode
        );

        for (int y = 0; y < size; y++)
        {
            for (int x = 0; x < size; x++)
            {
                if (uiInputs.useFallOff.isOn)
                {
                    noiseMap[x, y] = Mathf.Clamp01(noiseMap[x, y] - fallOffMap[x, y]);
                }
                float currentHeight = noiseMap[x, y];
                for (int i = 0; i < mapArguments.regions.Length; i++)
                {
                    if (currentHeight >= mapArguments.regions[i].height)
                    {
                        colorMap[y * size + x] = mapArguments.regions[i].color;
                    }
                    else
                    {
                        break;
                    }
                }
            }
        }

        return new MapData(noiseMap, colorMap);
    }

    public void SetFallOffMap()
    {
        if (uiInputs.useFallOff){
            if (uiInputs.useFallOff.isOn)
            {
                if (uiInputs.fallOffType.value == 0)
                {
                    fallOffMap = FallOffGenerator.GenerateIsland(Int32.Parse(uiInputs.size.text), uiInputs.fallOffRate.value);
                }
                else if (uiInputs.fallOffType.value == 1)
                {
                    fallOffMap = FallOffGenerator.GenerateCoast(Int32.Parse(uiInputs.size.text), uiInputs.fallOffRate.value, (Serializables.FallOffDirection)uiInputs.fallOffDirection.value);
                }
                else if (uiInputs.fallOffType.value == 2)
                {
                    fallOffMap = FallOffGenerator.GenerateLake(Int32.Parse(uiInputs.size.text), uiInputs.fallOffRate.value);
                }
            }
        }
    }
}

public struct MapData
{
    public float[,] heightMap;
    public Color[] colorMap;

    public MapData(float[,] heightMap, Color[] colorMap)
    {
        this.heightMap = heightMap;
        this.colorMap = colorMap;
    }
}


