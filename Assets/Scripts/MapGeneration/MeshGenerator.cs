using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MeshGenerator
{
    public static MeshData GenerateTerrainMesh(MapData mapdata, float heightMultiplier, AnimationCurve _heightCurve, int levelOfDetail)
    {
        AnimationCurve heightCurve = new AnimationCurve(_heightCurve.keys);
        
        float topLeftX = (mapdata.mapSize - 1) / -2f;
        float topLeftZ = (mapdata.mapSize - 1) / 2f;
        MeshData meshData = new MeshData();

        for (int y = 0; y < mapdata.mapSize; y++)
        {
            for (int x = 0; x < mapdata.mapSize; x++)
            {

                meshData.vertices.Add(new Vector3(topLeftX + x, mapdata.heightMap[x, y] * heightCurve.Evaluate(mapdata.heightMap[x, y]) * heightMultiplier, topLeftZ - y));
                meshData.uvs.Add(new Vector2(x / (float)mapdata.mapSize, y / (float)mapdata.mapSize));
                if (y == 0 || x == 0) continue;
                
                meshData.AddTriangle(mapdata.mapSize * x + y, mapdata.mapSize * x + y - 1, mapdata.mapSize * (x - 1) + y - 1);
                meshData.AddTriangle(mapdata.mapSize * (x - 1) + y - 1, mapdata.mapSize * (x - 1) + y, mapdata.mapSize * x + y);
            }
        }
        return meshData;
    }
}

public class MeshData
{
    public List<Vector3> vertices;
    public List<int> triangles;
    public List<Vector2> uvs;
    
    public MeshData()
    {
        vertices = new List<Vector3>();
        triangles = new List<int>();
        uvs = new List<Vector2>();
    }
    public void AddTriangle(int a, int b, int c)
    {
        triangles.Add(a);
        triangles.Add(b);
        triangles.Add(c);
    }

    public Mesh CreateMesh()
    {
        Mesh mesh = new Mesh();
        mesh.vertices = vertices.ToArray();
        mesh.triangles = triangles.ToArray();
        mesh.uv = uvs.ToArray();
        mesh.RecalculateNormals();
        return mesh;
    }
}