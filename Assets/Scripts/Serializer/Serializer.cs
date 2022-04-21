using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

[DataContract]
public class MapSerializer : Serializer
{
    [DataMember] public string mapName;
    [DataMember] public List<MapData> mapDatas;
    [DataMember] public List<AttackerSerializer> attackers;

    public MapSerializer(string mapName, List<MapData> mapDatas, List<AttackerSerializer> attackers)
    {
        this.mapName = mapName;
        this.mapDatas = mapDatas;
        this.attackers = attackers;
    }

    public static void Serialize(string path)
    {
        
    }

    public static MapSerializer Deserialize(string path)
    {
        return null;
    }
}

[DataContract]
public class AttackerSerializer
{
    [DataMember] public Tuple<Vector2, AttackerSelection.AttackerType, string> wave;
}


public class Serializer
{
    public static TSerializer DeserializeFromString<TSerializer>(string data)
    {
        byte[] b = Convert.FromBase64String(data);
        using (var stream = new MemoryStream(b))
        {
            var formatter = new BinaryFormatter();
            stream.Seek(0, SeekOrigin.Begin);
            return (TSerializer)formatter.Deserialize(stream);
        }
    }

    public static string SerializeToString<TSerializer>(TSerializer data)
    {
        using (var stream = new MemoryStream())
        {
            var formatter = new BinaryFormatter();
            formatter.Serialize(stream, data);
            stream.Flush();
            stream.Position = 0;
            return Convert.ToBase64String(stream.ToArray());
        }
    }
}

