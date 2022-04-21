using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.Threading.Tasks;

public class MapGenerator : MonoBehaviour
{
    [SerializeField] Serializables.UIInputs uiInputs;
    [SerializeField] Serializables.MapArgs mapArguments;
    [SerializeField] Noise.NormalizeMode normalizeMode;
    [SerializeField] private List<float[,]> fallOffMap;
    [SerializeField] private Image image;
    [SerializeField] private GameObject imageParent;
    private Image[] map;
    private MapData[] currentMapData;
    void Awake()
    {
        fallOffMap = new List<float[,]>(100);
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
        int mapSize = (int) uiInputs.size.value * 200;
        for (int i = 0; i < mapData.Length; i++)
        {
            display.DrawTexture(TextureGenerator.TextureFromColorMap(mapData[i].colorMap, mapSize / 10, mapSize / 10), map[i]);
        }
    }

    public MapData[] GetMapData()
    {
        return currentMapData;
    }
    public void DrawFallOffMap(MapData[] mapDatas, MapDisplay display)
    {
        int mapSize = (int) uiInputs.size.value * 200;
        fallOffMap = FallOffGenerator.GenerateFallOff(mapSize, uiInputs.fallOffRate.value, Serializables.FallOffType.Island);

        for (int i = 0; i < mapDatas.Length; i++)
        {
            display.DrawTexture(TextureGenerator.TextureFromFallOffMap(fallOffMap[i]), map[i]);
        }
    }
    
    public void DrawMeshMap(MapData mapData, MapDisplay display)
    {
        int mapSize = (int) uiInputs.size.value * 200;
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
                mapSize, 
                mapSize
            )
        );
    }
    public void DrawMapInEditor()
    {
        MapData[] mapData = GenerateMapData(Vector2.zero);
        currentMapData = mapData;
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
            DrawFallOffMap(mapData, display);
        }
    }

    MapData[] GenerateMapData(Vector2 center){
        
        if (uiInputs.useFallOff.isOn)
        {
            SetFallOffMap();
        }
        
        int mapSize = (int) uiInputs.size.value * 200;
        
        Vector2 offset = new Vector2(float.Parse(uiInputs.offsetX.text), float.Parse(uiInputs.offsetY.text));

        Vector2[] indexOffsets = new Vector2[100];

        int index = 0;
        for (int i = -5; i < 5; i++)
        {
            for (int j = -5; j < 5; j++)
            {
                indexOffsets[index] = new Vector2((mapSize / 10) * i, (mapSize / 10) * j);
                index++;
            }
        }

        List<Task<Tuple<int, MapData>>> tasks = new List<Task<Tuple<int, MapData>>>(100);
        MapData[] mapDatas = new MapData[100];

        int[] mapIndexes = new int[100];
        index = 0;
        for (int i = 1; i <= 10; i++)
        {
            for (int j = 1; j <= 10; j++)
            {
                mapIndexes[index] = 10 * i - j;
                index++;
            }
        }
        
        for (int i = 0; i < 100; i += 1)
        {
            int i1 = i;
            Task<Tuple<int, MapData>> task0 = Task.Factory.StartNew(() =>  GenerateMapDataParallel(mapIndexes[i1], mapSize, center, offset, indexOffsets[99 - i1]));
            tasks.Add(task0);
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
                    //Debug.Log("X: "+x+", Y: "+y+", index: " + index);
                    noiseMap[x, y] = Mathf.Clamp01(noiseMap[x, y] - fallOffMap[index][x, y]);
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
        int mapSize = (int) uiInputs.size.value * 200;
        if (uiInputs.useFallOff){
            if (uiInputs.useFallOff.isOn)
            {
                fallOffMap = FallOffGenerator.GenerateFallOff(
                    mapSize, 
                    uiInputs.fallOffRate.value, 
                    (Serializables.FallOffType)uiInputs.fallOffType.value, 
                    (Serializables.FallOffDirection)uiInputs.fallOffDirection.value
                    );
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

