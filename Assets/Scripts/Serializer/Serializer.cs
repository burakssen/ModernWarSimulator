using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

[Serializable]
public class MapSerilization
{
    public Guid guid;
    public String mapName;
    public float[,] mapData;
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

