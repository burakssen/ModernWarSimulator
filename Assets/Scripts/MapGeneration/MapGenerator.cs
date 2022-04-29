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
    [SerializeField] private Image image;
    [SerializeField] private GameObject imageParent;
    [SerializeField] private bool isMapLoader = false;
    private Image[] map;
    private MapData[] currentMapData;
    private List<float[,]> fallOffMap;
    void Awake()
    {
        if (isMapLoader)
            return;
        
        fallOffMap = new List<float[,]>(100);
        SetFallOffMap();
    }

    void Start()
    {
        if (isMapLoader)
            return;
        
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
    
    public void DrawMeshMap(MapData[] mapData, MapDisplay display, MapInfoSerializer mapInfoSerializer, Serializables.MapArgs mapArgs, GameObject[] planes)
    {
        List<Task> tasks = new List<Task>();
        
        for (int i = 0; i < 100; i++)
        {
            int index = i;
            Task task = Task.Factory.StartNew(() => DrawMeshMapParallel(mapData[index], display, mapInfoSerializer.size / 10, mapArgs, planes[index]));
            tasks.Add(task);
        }

        Task.WaitAll(tasks.ToArray());
    }

    public void DrawMeshMapParallel(MapData mapData, MapDisplay display, int size, Serializables.MapArgs mapArgs, GameObject plane)
    {
        int mapSize = size;
        float[,] heightMap = Global.To2DArray(mapData.heightMap, size, size);
        int rows = heightMap.GetLength(0);
        int cols = heightMap.GetLength(1);

        float[,] flippedHeightMap = new float[rows, cols];
        Color[] flippedColorMap = new Color[rows * cols];
        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < cols; j++)
            {
                flippedHeightMap[i, j] = heightMap[rows - i - 1, j];
                flippedColorMap[i * cols + j] = mapData.colorMap[i * cols + (cols - 1) - j];
            }
        }
        // Fixme -> You are here
        display.DrawMesh(
            MeshGenerator.GenerateTerrainMesh(
                flippedHeightMap, 
                mapArgs.meshHeightMultiplier, 
                mapArgs.meshHeightCurve, 
                mapArgs.editorPreivewLevelOfDetail
            ), 
            TextureGenerator.TextureFromColorMap(
                flippedColorMap, 
                mapSize, 
                mapSize
            ),
            plane.GetComponent<MeshRenderer>(),
            plane.GetComponent<MeshFilter>()
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
    
    public MapData[] GenerateMapData(Vector2 center, MapInfoSerializer mapInfoSerializer = null, Serializables.MapArgs mArgs = default)
    {

        int mapSize;
        Vector2 offset;
        int seed;
        bool useFallOff = false;
        Serializables.MapArgs mapArgs;

        if (mapInfoSerializer == null)
        {
            if (uiInputs.useFallOff.isOn)
            {
                SetFallOffMap();
                useFallOff = true;
            }
            mapSize = (int) uiInputs.size.value * 200;
            mapArgs = mapArguments;
        
            offset = new Vector2(float.Parse(uiInputs.offsetX.text), float.Parse(uiInputs.offsetY.text));
            seed = Int32.Parse(uiInputs.seed.text);
        }
        else
        {
            if (mapInfoSerializer.useFallOff)
            {
                SetFallOffMap(mapInfoSerializer);
                useFallOff = true;
            }

            mapArgs = mArgs;
        
            mapSize = mapInfoSerializer.size;
        
            offset = new Vector2(mapInfoSerializer.offSetX, mapInfoSerializer.offSetY);
            seed = mapInfoSerializer.seed;
        }

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
            Task<Tuple<int, MapData>> task0 = Task.Factory.StartNew(() =>  GenerateMapDataParallel(mapIndexes[i1], mapSize, seed, center, offset, indexOffsets[99 - i1], useFallOff, mapArgs));
            tasks.Add(task0);
        }

        Task.WaitAll(tasks.ToArray());

        foreach (var task in tasks)
        {
            mapDatas[task.Result.Item1] = task.Result.Item2;
        }
        
        return mapDatas;
    }
    public Tuple<int, MapData> GenerateMapDataParallel(int index, int size, int seed, Vector2 center, Vector2 offset, Vector2 indexOffset, bool useFallOff, Serializables.MapArgs mapArgs)
    {
    
        Color[] colorMap = new Color[(size / 10) * (size / 10)];

        float[,] noiseMap = Noise.GenerateNoiseMap(
            size / 10, 
            size / 10, 
            seed, 
            mapArgs.noiseScale, 
            mapArgs.octaves, 
            mapArgs.persistance, 
            mapArgs.lacunarity, 
            center + offset + indexOffset, 
            normalizeMode
        );

        for (int y = 0; y < size / 10; y++)
        {
            for (int x = 0; x < size / 10; x++)
            {
                if (useFallOff)
                {
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

        return new Tuple<int, MapData>(index, new MapData(noiseMap, colorMap, size / 10));
    }

    public void SetFallOffMap(MapInfoSerializer mapInfoSerializer = null)
    {
        int mapSize;
        float fallOffRate;
        Serializables.FallOffType fallOffType;
        Serializables.FallOffDirection fallOffDirection;
        bool fallOffActive = false;
        
        
        if (mapInfoSerializer == null)
        {
            if (uiInputs.useFallOff){
                if (uiInputs.useFallOff.isOn)
                {
                    fallOffActive = true;
                }
            }
            
            mapSize = (int) uiInputs.size.value * 200;
            fallOffRate = uiInputs.fallOffRate.value;
            fallOffType = (Serializables.FallOffType)uiInputs.fallOffType.value;
            fallOffDirection = (Serializables.FallOffDirection) uiInputs.fallOffDirection.value;
        }
        else
        {
            fallOffActive = true;
            mapSize = mapInfoSerializer.size;
            fallOffRate = mapInfoSerializer.fallOffRate;
            fallOffType = mapInfoSerializer.fallOffType;
            fallOffDirection = mapInfoSerializer.fallOffDirection;
        }

        if (!fallOffActive)
            return;
        
        fallOffMap = FallOffGenerator.GenerateFallOff(
            mapSize, 
            fallOffRate, 
            fallOffType, 
            fallOffDirection
        );
    }

    public Serializables.UIInputs GetUIInputs()
    {
        return uiInputs;
    }
}

public struct MapData
{
    public float[] heightMap;
    public Color[] colorMap;
    public int mapSize;
    public MapData(float[,] heightMap, Color[] colorMap,int mapSize)
    {
        this.heightMap = Global.To1DArray(heightMap);
        this.colorMap = colorMap;
        this.mapSize = mapSize;
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

