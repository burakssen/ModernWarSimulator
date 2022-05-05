using System;
using UnityEngine;
public class Tile : MonoBehaviour
{
    public Texture2D texture;
    private Color[] baseColors;

    void Update()
    {
        if (baseColors == null && texture)
            baseColors = texture.GetPixels();
    }
    
    private void OnMouseOver()
    {
        AddOutlineWithColor(Color.red);
    }

    private void OnMouseExit()
    {
        if(texture)
            gameObject.GetComponent<MeshRenderer>().material.mainTexture = texture;
    }

    private void AddOutlineWithColor(Color outlineColor)
    {
        int size = (int)Mathf.Sqrt(baseColors.Length);
        Color[] colors = texture.GetPixels();
        for (int i = 0; i < size; i++)
        {
            for (int j = 0; j < size; j++)
            {
                if(i == 0 || j == 0 || i == size - 2 || j == size - 2)
                    colors[i * size + j] = outlineColor;
            }
        }

        gameObject.GetComponent<MeshRenderer>().material.mainTexture =
            TextureGenerator.TextureFromColorMap(colors, size, size);
    }
}