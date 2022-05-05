using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MapDisplay : MonoBehaviour
{
    public void DrawTexture(Texture2D texture, Image image)
    {
        Sprite sprite = Sprite.Create(texture, new Rect(0.0f, 0.0f, texture.width, texture.height), new Vector2(0.5f, 0.5f), 100.0f);

        image.sprite = sprite;
    }

    public void DrawMesh(MeshData meshData, Texture2D texture, GameObject plane)
    {
        MeshFilter meshFilter = plane.GetComponent<MeshFilter>();
        MeshRenderer meshRenderer = plane.GetComponent<MeshRenderer>();
        meshFilter.mesh = meshData.CreateMesh();
        plane.GetComponent<MeshCollider>().sharedMesh = meshFilter.mesh;
        meshRenderer.material.mainTexture = texture;
        plane.GetComponent<Tile>().texture = texture;
    }
}