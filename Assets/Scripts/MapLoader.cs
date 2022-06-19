using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using UnityEngine;
using Application = UnityEngine.Device.Application;

public class MapLoader : MonoBehaviour
{
    [SerializeField] private Serializables.MapArgs mapArguments;
    [SerializeField] private GameObject planeParent;
    private GameObject[] _planes;
    private List<AttackerSelectionSerializer> attackerSelectionSerializers;

    [SerializeField] private bool clearAll = false;

    private void Start()
    {
        var mapInfoSerializer = GetMapData(PlayerPrefs.GetString("LevelName"));
        var mapGenerator = FindObjectOfType<MapGenerator>();
        attackerSelectionSerializers = new List<AttackerSelectionSerializer>();
        var mapDatas = mapGenerator.GenerateMapData(Vector2.zero, mapInfoSerializer, mapArguments);
        Global.budget = mapInfoSerializer.budget;
        Global.mapSize = (mapInfoSerializer.size - 1) / 200f;

        foreach (var attacker in mapInfoSerializer.attackerSelections)
        {
            var attackerSelectionSerializer = new AttackerSelectionSerializer();
            attackerSelectionSerializer.damage = attacker.damage;
            attackerSelectionSerializer.attackerName = attacker.attackerName;
            attackerSelectionSerializer.attackerType = attacker.attackerType;
            attackerSelectionSerializer.numberOfAttacker = attacker.numberOfAttacker;
            attackerSelectionSerializers.Add(attackerSelectionSerializer);
        }

        _planes = new GameObject[100];
        for (var i = 0; i < 10; i++)
        for (var j = 0; j < 10; j++)
        {
            var plane = GameObject.CreatePrimitive(PrimitiveType.Plane); //Create GO and add necessary components
            plane.transform.SetParent(planeParent.transform);
            _planes[i * 10 + j] = plane;
            var tile = plane.AddComponent<Tile>();
            tile.index = new Vector2(i, j);
            plane.tag = "Plane";
        }

        mapGenerator.DrawMeshMap(mapDatas, FindObjectOfType<MapDisplay>(), mapInfoSerializer, mapArguments, _planes);

        for (var i = 0; i < 10; i++)
        for (var j = 0; j < 10; j++)
        {
            var plane = _planes[i * 10 + j];
            var planeMesh = plane.GetComponent<MeshFilter>().mesh;
            var bounds = planeMesh.bounds;
            var width = plane.transform.localScale.x * bounds.size.x;
            var height = plane.transform.localScale.z * bounds.size.z;
            plane.transform.localPosition = new Vector3(100 - i * width, 0f, j * height);
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
        var map = File.ReadAllText(Application.dataPath + $"/Maps/{mapName}.level");
        return Serializer.DeserializeFromString<MapInfoSerializer>(map);
    }

    public void SetSpawnObject()
    {
        foreach (var plane in _planes) plane.GetComponent<Tile>().objectToPlace = Global.spawnObject;
    }

    public void ClearAll()
    {
        Global.gameState = Global.GameState.play;
        foreach (var plane in _planes) plane.GetComponent<Tile>().Clear();
    }

    public List<AttackerSelectionSerializer> GetAttackerSelectionSerializers()
    {
        return attackerSelectionSerializers;
    }
}