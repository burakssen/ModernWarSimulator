using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.Linq;
using System.Threading.Tasks;

public class MapGenerator : MonoBehaviour
{
    [SerializeField] private Serializables.UIInputs uiInputs;
    [SerializeField] private Serializables.MapArgs mapArguments;
    [SerializeField] private Noise.NormalizeMode normalizeMode;
    [SerializeField] private Image image;
    [SerializeField] private GameObject imageParent;
    [SerializeField] private bool isMapLoader = false;
    private Image[] map;
    private MapData[] currentMapData;
    private List<float[,]> fallOffMap;

    private void Awake()
    {
        if (isMapLoader)
            return;

        map = new Image[100];
        for (var i = 0; i < 10; i++)
        for (var j = 0; j < 10; j++)
        {
            var tImage = Instantiate(image, imageParent.transform);
            tImage.transform.localPosition = new Vector3(3 * i * tImage.rectTransform.rect.width,
                3 * j * tImage.rectTransform.rect.height - 400, 0f);
            map[i * 10 + j] = tImage;
        }

        fallOffMap = new List<float[,]>(100);
        SetFallOffMap();
    }

    public void DrawColorMap(MapData[] mapData, MapDisplay display)
    {
        var mapSize = (int)uiInputs.size.value * 200;
        for (var i = 0; i < mapData.Length; i++)
            display.DrawTexture(TextureGenerator.TextureFromColorMap(mapData[i].colorMap, mapSize / 10, mapSize / 10),
                map[i]);
    }

    public MapData[] GetMapData()
    {
        return currentMapData;
    }

    public void DrawFallOffMap(MapData[] mapDatas, MapDisplay display)
    {
        var mapSize = (int)uiInputs.size.value * 200;
        fallOffMap =
            FallOffGenerator.GenerateFallOff(mapSize, uiInputs.fallOffRate.value, Serializables.FallOffType.Island);

        for (var i = 0; i < mapDatas.Length; i++)
            display.DrawTexture(TextureGenerator.TextureFromFallOffMap(fallOffMap[i]), map[i]);
    }

    public void DrawMeshMap(MapData[] mapData, MapDisplay display, MapInfoSerializer mapInfoSerializer,
        Serializables.MapArgs mapArgs, GameObject[] planes)
    {
        var tasks = new List<Task<Tuple<int, MeshData>>>();

        for (var i = 0; i < 100; i++)
        {
            var index = i;
            var task = Task.Factory.StartNew(() => DrawMeshMapParallel(index, mapData[index], mapArgs));
            tasks.Add(task);
        }

        Task.WaitAll(tasks.ToArray());

        foreach (var task in tasks)
        {
            var mapSize = mapData[task.Result.Item1].mapSize;
            planes[task.Result.Item1].GetComponent<Tile>().averageHeight =
                mapData[task.Result.Item1].heightMap.Cast<float>().Sum() / (mapSize * mapSize);
            planes[task.Result.Item1].GetComponent<Tile>().centerHeight =
                mapData[task.Result.Item1].heightMap[(int)(mapSize / 2), (int)(mapSize / 2)];
            display.DrawMesh(
                task.Result.Item2,
                TextureGenerator.TextureFromColorMap(
                    mapData[task.Result.Item1].colorMap,
                    mapInfoSerializer.size / 10,
                    mapInfoSerializer.size / 10
                ),
                planes[task.Result.Item1]
            );
        }
    }

    public Tuple<int, MeshData> DrawMeshMapParallel(int index, MapData mapData, Serializables.MapArgs mapArgs)
    {
        return new Tuple<int, MeshData>(index,
            MeshGenerator.GenerateTerrainMesh(
                mapData,
                mapArgs.meshHeightMultiplier,
                mapArgs.meshHeightCurve,
                mapArgs.editorPreivewLevelOfDetail
            ));
    }

    public void DrawMapInEditor()
    {
        //Start();
        var mapData = GenerateMapData(Vector2.zero);
        currentMapData = mapData;
        var display = FindObjectOfType<MapDisplay>();

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

    public MapData[] GenerateMapData(Vector2 center, MapInfoSerializer mapInfoSerializer = null,
        Serializables.MapArgs mArgs = default)
    {
        int mapSize;
        Vector2 offset;
        int seed;
        var useFallOff = false;
        Serializables.MapArgs mapArgs;

        if (mapInfoSerializer == null)
        {
            if (uiInputs.useFallOff.isOn)
            {
                SetFallOffMap();
                useFallOff = true;
            }

            mapSize = (int)uiInputs.size.value * 200;
            mapArgs = mapArguments;

            offset = new Vector2(float.Parse(uiInputs.offsetX.text), float.Parse(uiInputs.offsetY.text));
            seed = int.Parse(uiInputs.seed.text);
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

        var indexOffsets = new Vector2[100];

        var index = 0;
        for (var i = -5; i < 5; i++)
        for (var j = -5; j < 5; j++)
        {
            indexOffsets[index] = new Vector2(mapSize / 10 * i - i, mapSize / 10 * j - j);
            index++;
        }

        var tasks = new List<Task<Tuple<int, MapData>>>(100);
        var mapDatas = new MapData[100];

        var mapIndexes = new int[100];
        index = 0;
        for (var i = 1; i <= 10; i++)
        for (var j = 1; j <= 10; j++)
        {
            mapIndexes[index] = 10 * i - j;
            index++;
        }

        for (var i = 0; i < 100; i += 1)
        {
            var i1 = i;
            var task0 = Task.Factory.StartNew(() => GenerateMapDataParallel(mapIndexes[i1], mapSize, seed, center,
                offset, indexOffsets[99 - i1], useFallOff, mapArgs));
            tasks.Add(task0);
        }

        Task.WaitAll(tasks.ToArray());

        foreach (var task in tasks) mapDatas[task.Result.Item1] = task.Result.Item2;

        return mapDatas;
    }

    public Tuple<int, MapData> GenerateMapDataParallel(int index, int size, int seed, Vector2 center, Vector2 offset,
        Vector2 indexOffset, bool useFallOff, Serializables.MapArgs mapArgs)
    {
        var colorMap = new Color[size / 10 * (size / 10)];

        var noiseMap = Noise.GenerateNoiseMap(
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

        for (var y = 0; y < size / 10; y++)
        for (var x = 0; x < size / 10; x++)
        {
            if (useFallOff) noiseMap[x, y] = Mathf.Clamp01(noiseMap[x, y] - fallOffMap[index][x, y]);
            var currentHeight = noiseMap[x, y];
            for (var i = 0; i < mapArguments.regions.Length; i++)
                if (currentHeight >= mapArguments.regions[i].height)
                    colorMap[y * (size / 10) + x] = mapArguments.regions[i].color;
                else
                    break;
        }

        return new Tuple<int, MapData>(index, new MapData(noiseMap, colorMap, size / 10));
    }

    public void SetFallOffMap(MapInfoSerializer mapInfoSerializer = null)
    {
        int mapSize;
        float fallOffRate;
        Serializables.FallOffType fallOffType;
        Serializables.FallOffDirection fallOffDirection;
        var fallOffActive = false;


        if (mapInfoSerializer == null)
        {
            if (uiInputs.useFallOff)
                if (uiInputs.useFallOff.isOn)
                    fallOffActive = true;

            mapSize = (int)uiInputs.size.value * 200;
            fallOffRate = uiInputs.fallOffRate.value;
            fallOffType = (Serializables.FallOffType)uiInputs.fallOffType.value;
            fallOffDirection = (Serializables.FallOffDirection)uiInputs.fallOffDirection.value;
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
    public float[,] heightMap;
    public Color[] colorMap;
    public int mapSize;

    public MapData(float[,] heightMap, Color[] colorMap, int mapSize)
    {
        this.heightMap = heightMap;
        this.colorMap = colorMap;
        this.mapSize = mapSize;
    }
}