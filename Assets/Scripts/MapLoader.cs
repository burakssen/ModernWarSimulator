using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Application = UnityEngine.Device.Application;

public class MapLoader : MonoBehaviour{
    [SerializeField] Serializables.MapArgs mapArguments;
    [SerializeField] GameObject planeParent;
    private GameObject[] planes;
    
    private void Awake()
    {
        planes = new GameObject[100];
        
        for (int i = 0; i < 10; i++)
        {
            for (int j = 0; j < 10; j++)
            {
                GameObject tPlane = GameObject.CreatePrimitive(PrimitiveType.Plane);
                tPlane.transform.SetParent(planeParent.transform);
                Mesh planeMesh = tPlane.GetComponent<MeshFilter>().mesh;
                Bounds bounds = planeMesh.bounds;
                float width = tPlane.transform.localScale.x * bounds.size.x;
                float height = tPlane.transform.localScale.z * bounds.size.z;
                tPlane.transform.localPosition = new Vector3(i * width, 0f, j * height);
                planes[i * 10 + j] = tPlane;
            }
        }
    }

    private void Start()
    {
        MapInfoSerializer mapInfoSerializer = GetMapData("Level-1");
        MapGenerator mapGenerator = FindObjectOfType<MapGenerator>();
        MapData[] mapDatas = mapGenerator.GenerateMapData(Vector2.zero, mapInfoSerializer, mapArguments);
        MapDisplay display = FindObjectOfType<MapDisplay>();
        mapGenerator.DrawMeshMap(mapDatas, display, mapInfoSerializer, mapArguments, planes);
    }

    private MapInfoSerializer GetMapData(string mapName)
    {
        string map = File.ReadAllText(Application.dataPath + $"/Maps/{mapName}.level");
        return Serializer.DeserializeFromString<MapInfoSerializer>(map);
    }
}
