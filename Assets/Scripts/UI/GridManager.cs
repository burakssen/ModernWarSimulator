using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    [Range(1, 20)]
    [SerializeField] int tileSizeX;

    [Range(1, 20)]
    [SerializeField] int tileSizeY;

    [SerializeField] Tile tile;
    [SerializeField] GameObject target;
    private void Start()
    {
        GenerateGrid();        
    }
    void GenerateGrid()
    {

        for (float x = 0; x < tileSizeX; x++)
        {
            for (float y = 0; y < tileSizeY; y++)
            {
                float targetWidth = target.transform.GetComponent<RectTransform>().rect.width;
                float targetHeight = target.transform.GetComponent<RectTransform>().rect.height;
                float tileWidth = targetWidth / tileSizeX;

                float tileHeight = targetHeight / tileSizeY;
                tile.GetComponent<SpriteRenderer>().size = new Vector2(tileWidth, tileHeight);
                tile.GetComponent<BoxCollider2D>().size = new Vector2(tileWidth, tileHeight);
                tile.tileIndex = new Vector2(x, y);

                Tile spawnedTile = Instantiate(tile, new Vector3(x, y, -1), Quaternion.identity);
                spawnedTile.transform.SetParent(target.transform);
                spawnedTile.transform.localPosition = new Vector3((x * tileWidth) - targetWidth / 2 + tileWidth / 2, (y * tileHeight) - targetHeight / 2 + tileHeight / 2, -1);
                spawnedTile.transform.localScale = new Vector3(1f, 1f);
                spawnedTile.transform.GetComponent<RectTransform>().sizeDelta = new Vector2(tileWidth, tileHeight);

            }
        }
    }
}