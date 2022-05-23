using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using UnityEngine;
using Application = UnityEngine.Device.Application;

public class MapLoader : MonoBehaviour{
    [SerializeField] Serializables.MapArgs mapArguments;
    [SerializeField] GameObject planeParent;
    private GameObject[] _planes;
    [SerializeField] private GameObject spawnObject;
    [SerializeField] private bool clearAll = false;
    
    private void Start()
    {
        MapInfoSerializer mapInfoSerializer = GetMapData("Level2");
        MapGenerator mapGenerator = FindObjectOfType<MapGenerator>();
        MapData[] mapDatas = mapGenerator.GenerateMapData(Vector2.zero, mapInfoSerializer, mapArguments);
        
        _planes = new GameObject[100];
        for (int i = 0; i < 10; i++)
        {
            for (int j = 0; j < 10; j++)
            {
                GameObject plane = GameObject.CreatePrimitive(PrimitiveType.Plane); //Create GO and add necessary components
                plane.transform.SetParent(planeParent.transform);
                _planes[i * 10 + j] = plane;
                Tile tile = plane.AddComponent<Tile>();
            }
        }
        
        mapGenerator.DrawMeshMap(mapDatas, FindObjectOfType<MapDisplay>(), mapInfoSerializer, mapArguments, _planes);
        
        for (int i = 0; i < 10; i++)
        {
            for (int j = 0; j < 10; j++)
            {
               
                GameObject plane = _planes[i * 10 + j];
                Mesh planeMesh = plane.GetComponent<MeshFilter>().mesh;
                Bounds bounds = planeMesh.bounds;
                float width = plane.transform.localScale.x * bounds.size.x;
                float height = plane.transform.localScale.z * bounds.size.z;
                plane.transform.localPosition = new Vector3(100 -i * width, 0f, j * height);
                plane.GetComponent<Tile>().objectToPlace = spawnObject;
            }
        }
    }

    private void Update()
    {
        if (clearAll)
        {
            ClearAll();
            clearAll = false;
        }
    }

    private MapInfoSerializer GetMapData(string mapName)
    {
        string map = File.ReadAllText(Application.dataPath + $"/Maps/{mapName}.level");
        return Serializer.DeserializeFromString<MapInfoSerializer>(map);
    }

    private void ClearAll()
    {
        foreach (var plane in _planes)
        {
            plane.GetComponent<Tile>().Clear();
        }
    }
}
