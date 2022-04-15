using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;


public class MapGenerator : MonoBehaviour
{
    [SerializeField] bool autoUpdate;
    [SerializeField] Serializables.UIInputs uiInputs;
    [SerializeField] Serializables.MapArgs mapArguments;
    [SerializeField] Image miniMap;
    [SerializeField] Noise.NormalizeMode normalizeMode;
    float[,] fallOffMap;
    float[,] heightMap;
    [SerializeField] private Image image;
    [SerializeField] private GameObject imageParent;
    private Image[] map;
    void Awake()
    {
        SetFallOffMap();
    }

    void Start()
    {
        map = new Image[100];
        for (int i = 0; i < 10; i++)
        {
            for (int j = 0; j < 10; j++)
            {
                Image tImage = Instantiate(image, imageParent.transform);
                tImage.transform.localPosition = new Vector3(3 * i * (tImage.rectTransform.rect.width),
                    3 * j * (tImage.rectTransform.rect.height) - 400, 0f);
                map[i * 10 + j] = tImage;
            }
        }
    }

    public void DrawColorMap(MapData[] mapData, MapDisplay display)
    {
        for (int i = 0; i < mapData.Length; i++)
        {
            display.DrawTexture(TextureGenerator.TextureFromColorMap(mapData[i].colorMap, Int32.Parse(uiInputs.size.text), Int32.Parse(uiInputs.size.text)), map[i]);
        }
    }

    public void DrawFallOffMap(MapData mapData, MapDisplay display)
    {
        //display.DrawTexture(TextureGenerator.TextureFromFallOffMap(FallOffGenerator.GenerateCoast(Int32.Parse(uiInputs.size.text), uiInputs.fallOffRate.value, (Serializables.FallOffDirection)uiInputs.fallOffDirection.value)));
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
        MapData[] mapData = GenerateMapData(Vector2.zero);
        MapDisplay display = FindObjectOfType<MapDisplay>();

        if (mapArguments.drawMode == Serializables.DrawMode.ColorMap)
        {
            DrawColorMap(mapData, display);
        }
        else if (mapArguments.drawMode == Serializables.DrawMode.Mesh)
        {
           // DrawMeshMap(mapData, display);
        }
        else if (mapArguments.drawMode == Serializables.DrawMode.FallOff)
        {
            //DrawFallOffMap(mapData, display);
        }
    }

    MapData[] GenerateMapData(Vector2 center){
        
        if (uiInputs.useFallOff.isOn)
        {
            SetFallOffMap();
        }

        int size = Int32.Parse(uiInputs.size.text);

        Vector2 offset = new Vector2(float.Parse(uiInputs.offsetX.text), float.Parse(uiInputs.offsetY.text));

        Vector2[] indexOffsets = new Vector2[100];

        int index = 0;
        for (int i = 0; i < size - size / 10; i += size / 10)
        {
            for (int j = 0; j < size - size / 10; j += size / 10)
            {
                indexOffsets[index] = new Vector2(size * i + size / 10, size * j + size / 10);
                index++;
            }
        }

        List<Task<Tuple<int, MapData>>> tasks = new List<Task<Tuple<int, MapData>>>(100);
        MapData[] mapDatas = new MapData[100];
        
        for (int i = 0; i < 10; i += 10)
        {
            int i1 = i;
            Task<Tuple<int, MapData>> task0 = Task.Factory.StartNew(() =>  GenerateMapDataParallel(i1 + 9, size, center, offset, indexOffsets[i1 + 9]));
            tasks.Add(task0);
            Task<Tuple<int, MapData>> task1 = Task.Factory.StartNew(() =>  GenerateMapDataParallel(i1 + 8, size, center, offset, indexOffsets[i1 + 8]));
            tasks.Add(task1);
            Task<Tuple<int, MapData>> task2 = Task.Factory.StartNew(() =>  GenerateMapDataParallel(i1 + 7, size, center, offset, indexOffsets[i1 + 7]));
            tasks.Add(task2);
            Task<Tuple<int, MapData>> task3 = Task.Factory.StartNew(() =>  GenerateMapDataParallel(i1 + 6, size, center, offset, indexOffsets[i1 + 6]));
            tasks.Add(task3);
            Task<Tuple<int, MapData>> task4 = Task.Factory.StartNew(() =>  GenerateMapDataParallel(i1 + 5, size, center, offset, indexOffsets[i1 + 5]));
            tasks.Add(task4);
            Task<Tuple<int, MapData>> task5 = Task.Factory.StartNew(() =>  GenerateMapDataParallel(i1 + 4, size, center, offset, indexOffsets[i1 + 4]));
            tasks.Add(task5);
            Task<Tuple<int, MapData>> task6 = Task.Factory.StartNew(() =>  GenerateMapDataParallel(i1 + 3, size, center, offset, indexOffsets[i1 + 3]));
            tasks.Add(task6);
            Task<Tuple<int, MapData>> task7 = Task.Factory.StartNew(() =>  GenerateMapDataParallel(i1 + 2, size, center, offset, indexOffsets[i1 + 2]));
            tasks.Add(task7);
            Task<Tuple<int, MapData>> task8 = Task.Factory.StartNew(() =>  GenerateMapDataParallel(i1 + 1, size, center, offset, indexOffsets[i1 + 1]));
            tasks.Add(task8);
            Task<Tuple<int, MapData>> task9 = Task.Factory.StartNew(() =>  GenerateMapDataParallel(i1 + 0, size, center, offset, indexOffsets[i1 + 0]));
            tasks.Add(task9);
        }

        Task.WaitAll(tasks.ToArray());

        foreach (var task in tasks)
        {
            mapDatas[task.Result.Item1] = task.Result.Item2;
        }
        
        return mapDatas;
    }
    public Tuple<int, MapData> GenerateMapDataParallel(int index, int size, Vector2 center, Vector2 offset, Vector2 indexOffset)
    {
    
        Color[] colorMap = new Color[(size / 10) * (size / 10)];

        float[,] noiseMap = Noise.GenerateNoiseMap(
            size / 10, 
            size / 10, 
            Int32.Parse(uiInputs.seed.text), 
            mapArguments.noiseScale, 
            mapArguments.octaves, 
            mapArguments.persistance, 
            mapArguments.lacunarity, 
            center + offset + indexOffset, 
            normalizeMode
        );

        for (int y = 0; y < size / 10; y++)
        {
            for (int x = 0; x < size / 10; x++)
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
                        colorMap[y * (size / 10) + x] = mapArguments.regions[i].color;
                    }
                    else
                    {
                        break;
                    }
                }
            }
        }

        return new Tuple<int, MapData>(index, new MapData(noiseMap, colorMap));
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


public struct MapDataDataClass
{
    public int index;
    public int size;
    public Vector2 center;
    public Vector2 offset;
    public Vector2 indexOffset;
    public List<MapData> imageMaps;

    public MapDataDataClass(int index, int size, Vector2 center, Vector2 offset, Vector2 indexOffset, ref List<MapData> imageMaps)
    {
        this.index = index;
        this.size = size;
        this.center = center;
        this.offset = offset;
        this.indexOffset = indexOffset;
        this.imageMaps = imageMaps;
    }
}

