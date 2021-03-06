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
    public Vector2 index;

    private void Start()
    {
        sceneCamera = FindObjectOfType<Camera>();
    }

    private void Update()
    {
        if (Global.budget <= 0)
            placeable = false;

        if (Global.gameState == Global.GameState.play)
            return;

        var pos = sceneCamera.ScreenToWorldPoint(Input.mousePosition);

        Debug.DrawRay(pos, Vector3.forward, Color.green);
        if (baseColors == null && texture)
        {
            baseColors = texture.GetPixels();
            if (averageHeight > 0.65 || averageHeight < 0.2)
            {
                placeable = false;
                AddXWithColor(Color.red);
            }
            else
            {
                gameObject.GetComponent<MeshRenderer>().material.mainTexture = texture;
            }
        }
    }

    private void OnMouseOver()
    {
        if (placeable && Global.selectedDefence && Global.budget >= Global.selectedDefence.cost)
            AddOutlineWithColor(Color.white);
    }

    private void OnMouseDown()
    {
        if (Global.basePlaced && Global.selectedDefence.name == "Base")
            return;

        if (Global.gameState == Global.GameState.play)
            return;

        if (!Global.selectedDefence || Global.budget < Global.selectedDefence.cost)
            return;

        if (objectToPlace == null)
            return;

        if (!placeable)
            return;

        if (occupied)
            return;

        if (Global.selectedDefence.name == "Base")
            Global.basePlaced = true;

        if (averageHeight > 0.65 || averageHeight < 0.2)
            return;

        Global.budget -= Global.selectedDefence.cost;

        var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            var gObject = Instantiate(objectToPlace, transform);
            gObject.GetComponent<SphereCollider>().enabled = false;
            gObject.transform.position =
                new Vector3(gameObject.transform.position.x, 0, gameObject.transform.position.z);
            gObject.transform.localPosition =
                new Vector3(gObject.transform.localPosition.x, hit.point.y + 20f, gObject.transform.localPosition.z);
            occupied = true;

            var val = 1;
            if (Global.selectedDefence.defenceName == "Base")
            {
                gObject.GetComponent<Base>().SetValues(Global.selectedDefence.health);
                val = 10;
            }
            else
            {
                gObject.GetComponent<BaseDefence>().SetValues(Global.selectedDefence.health);
            }

            if (index.x < 5)
                Global.leftCounter += val;
            else if (index.x >= 5)
                Global.rightCounter += val;
        }
    }

    private void OnMouseExit()
    {
        if (Global.gameState == Global.GameState.play)
            return;

        if (!Global.selectedDefence || Global.budget < Global.selectedDefence.cost)
            return;

        if (texture && placeable)
            Clear();
    }

    public void Clear()
    {
        gameObject.GetComponent<MeshRenderer>().material.mainTexture = texture;
        if (Global.gameState == Global.GameState.play)
            placeable = false;
    }

    private void AddOutlineWithColor(Color outlineColor)
    {
        var size = (int)Mathf.Sqrt(baseColors.Length);
        var colors = texture.GetPixels();
        for (var i = 0; i < size; i++)
        for (var j = 0; j < size; j++)
            if (i == 0 || j == 0 || i == size - 2 || j == size - 2)
                colors[i * size + j] = outlineColor;

        gameObject.GetComponent<MeshRenderer>().material.mainTexture =
            TextureGenerator.TextureFromColorMap(colors, size, size);
    } 

    private void AddXWithColor(Color xColor)
    {
        var size = (int)Mathf.Sqrt(baseColors.Length);
        var colors = texture.GetPixels();
        for (var i = 0; i < size; i++)
        for (var j = 0; j < size; j++)
        {
            colors[i * size + j] += new Color(xColor.r, xColor.b, xColor.g, 0.2f);
            if (i == 0 || j == 0 || i == size - 2 || j == size - 2)
                colors[i * size + j] = xColor;
        }

        gameObject.GetComponent<MeshRenderer>().material.mainTexture =
            TextureGenerator.TextureFromColorMap(colors, size, size);
    }
}