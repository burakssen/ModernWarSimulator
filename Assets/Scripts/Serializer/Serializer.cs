using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;


public class MapSerializer : Serializer
{
    public MapInfoSerializer mapInfoSerializer;

    public void Serialize(string path)
    {
        var data = SerializeToString(mapInfoSerializer);
        File.WriteAllText(path + $"/{mapInfoSerializer.mapName}.level", data, Encoding.UTF8);
    }
}

[DataContract]
public class MapInfoSerializer
{
    [DataMember] public string mapName;
    [DataMember] public int seed;
    [DataMember] public int size;
    [DataMember] public float offSetX;
    [DataMember] public float offSetY;
    [DataMember] public bool useFallOff;
    [DataMember] public Serializables.FallOffType fallOffType;
    [DataMember] public float fallOffRate;
    [DataMember] public Serializables.FallOffDirection fallOffDirection;
    [DataMember] public List<AttackerSelectionSerializer> attackerSelections;
    [DataMember] public int budget;
}

[DataContract]
public class AttackerSelectionSerializer
{
    [DataMember] public string attackerName;

    [DataMember] public int damage;

    [DataMember] public AttackerSelection.AttackerType attackerType;

    [DataMember] public int numberOfAttacker;
}

public class Serializer
{
    public static TSerializer DeserializeFromString<TSerializer>(string data) where TSerializer : class, new()
    {
        var deserializedObject = new TSerializer();
        var ms = new MemoryStream(Encoding.UTF8.GetBytes(data));
        var ser = new DataContractJsonSerializer(deserializedObject.GetType());
        deserializedObject = ser.ReadObject(ms) as TSerializer;
        ms.Close();
        return deserializedObject;
    }

    public static string SerializeToString<TSerializer>(TSerializer data)
    {
        var ms = new MemoryStream();
        var ser = new DataContractJsonSerializer(typeof(TSerializer));
        ser.WriteObject(ms, data);
        var json = ms.ToArray();
        ms.Close();
        return Encoding.UTF8.GetString(json, 0, json.Length);
    }
}