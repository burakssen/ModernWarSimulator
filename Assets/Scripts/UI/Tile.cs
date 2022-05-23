using System;
using UnityEngine;
public class Tile : MonoBehaviour
{
    public Texture2D texture;
    private Color[] baseColors;
    public GameObject objectToPlace;
    private bool occupied = false;
    public float averageHeight = 0;
    public float centerHeight = 0;
    private Camera sceneCamera;
    private bool placeable = true;

    void Start()
    {
        
        sceneCamera = FindObjectOfType<Camera>();
    }
    void Update()
    { 
        Vector3 pos = sceneCamera.ScreenToWorldPoint(Input.mousePosition);
        
        Debug.DrawRay(pos, Vector3.forward, Color.green);
        if (baseColors == null && texture)
        {
            baseColors = texture.GetPixels();
            if (averageHeight > 0.65 || averageHeight < 0.2)
            {
                placeable = false;
                AddXWithColor(Color.red);
            }
        }
    }
    
    private void OnMouseOver()
    {
        if(placeable)
            AddOutlineWithColor(Color.white);
    }

    private void OnMouseDown()
    {
        if (occupied)
            return;

        if (averageHeight > 0.65 || averageHeight < 0.2)
            return;
        
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit)) {
            GameObject gObject = Instantiate(objectToPlace, transform);
            gObject.transform.position = new Vector3(gameObject.transform.position.x, 0, gameObject.transform.position.z);
            gObject.transform.localPosition =
                new Vector3(gObject.transform.localPosition.x, hit.point.y + 20f, gObject.transform.localPosition.z);
            occupied = true;
        }
    }

    private void OnMouseExit()
    {
        if(texture && placeable)
            Clear();
    }

    public void Clear()
    {
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

    private void AddXWithColor(Color xColor)
    {
        int size = (int)Mathf.Sqrt(baseColors.Length);
        Color[] colors = texture.GetPixels();
        for (int i = 0; i < size; i++)
        {
            for (int j = 0; j < size; j++)
            {
                colors[i * size + j] += new Color(xColor.r, xColor.b, xColor.g, 0.2f);
                if(i == 0 || j == 0 || i == size - 2 || j == size - 2)
                    colors[i * size + j] = xColor;
            }
        }

        gameObject.GetComponent<MeshRenderer>().material.mainTexture =
            TextureGenerator.TextureFromColorMap(colors, size, size);
    }
}