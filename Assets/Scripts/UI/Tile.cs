using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class Tile : MonoBehaviour
{
    [SerializeField] Sprite unselected;
    [SerializeField] Sprite hovered;
    [SerializeField] Sprite selected;
    [SerializeField] GameObject textObject; 

    Vector2 size;
    public Vector2 tileIndex;
    private bool tileSelected = false;
    void Start()
    {
        size = GetComponent<SpriteRenderer>().size;
        GetComponent<SpriteRenderer>().sprite = unselected;
        GetComponent<SpriteRenderer>().size = size;
    }

    void Update()
    {
        if (Global.globalIndex != tileIndex && Global.tileUpdate)
        {
            GetComponent<SpriteRenderer>().sprite = unselected;
            GetComponent<SpriteRenderer>().size = size;
        }
    }

    void OnMouseOver()
    {
        if(Global.globalIndex != tileIndex)
        {
            GetComponent<SpriteRenderer>().sprite = hovered;
            GetComponent<SpriteRenderer>().size = size;
            Global.tileUpdate = false;
        }
    }

    private void OnMouseDown()
    {
        if (Global.globalIndex != tileIndex && tileSelected == false)
        {
            GetComponent<SpriteRenderer>().sprite = selected;
            GetComponent<SpriteRenderer>().size = size;
            tileSelected = true;
            Global.globalIndex = tileIndex;
            Global.tileUpdate = true;
            textObject.GetComponent<TextMesh>().text = "A";
        }
        else if (tileSelected)
        {
            GetComponent<SpriteRenderer>().sprite = unselected;
            tileSelected = false;
            Global.globalIndex = new Vector2(999,999);
            textObject.GetComponent<TextMesh>().text = "";
        }
    }

    void OnMouseExit()
    {
        if(Global.globalIndex != tileIndex)
        {
            GetComponent<SpriteRenderer>().sprite = unselected;
            GetComponent<SpriteRenderer>().size = size;
        }
    }
}