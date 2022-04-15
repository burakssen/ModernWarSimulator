using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MapDisplay : MonoBehaviour
{
    [SerializeField] Renderer textureRenderer;
    [SerializeField] MeshFilter meshFilter;
    [SerializeField] MeshRenderer meshRenderer;

    public void DrawTexture(Texture2D texture, Image image)
    {
        Sprite sprite = Sprite.Create(texture, new Rect(0.0f, 0.0f, texture.width, texture.height), new Vector2(0.5f, 0.5f), 100.0f);

        image.sprite = sprite;
        //textureRenderer.sharedMaterial.mainTexture = texture;
        //textureRenderer.transform.localScale = new Vector3(texture.width, 1, texture.height);
    }

    public void DrawMesh(MeshData meshData, Texture2D texture)
    {
        meshFilter.sharedMesh = meshData.CreateMesh();
        meshRenderer.sharedMaterial.mainTexture = texture;
    }
}