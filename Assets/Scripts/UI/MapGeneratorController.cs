using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.IO;

public class MapGeneratorController : MonoBehaviour
{

    [SerializeField] TMP_InputField size;
    [SerializeField] TMP_InputField offsetX;
    [SerializeField] TMP_InputField offsetY;
    [SerializeField] Slider fallOffRate;
    [SerializeField] TMP_Text fallOffRateText;
    [SerializeField] TMP_InputField seed;
    [SerializeField] TMP_InputField mapName;
    [SerializeField] TMP_Text notification;
    [SerializeField] GameObject[] AllUIElements;
    [SerializeField] GameObject[] EditorUI;
    void Start()
    {
        if (size.text != "")
        {
            FindObjectOfType<MapGenerator>().DrawMapInEditor();
        }
    }
    
    public void UpdateMap()
    {
        int iSize = Int32.Parse(size.text);
        if (iSize > 600)
        {
            size.text = "600";
        }
        else if (iSize < 50)
        {
            size.text = "50";
        }
        FindObjectOfType<MapGenerator>().DrawMapInEditor();
    }

    public void GeneratorX()
    {
        System.Random rnd = new System.Random();
        offsetX.text = (rnd.NextDouble() * 20000.0 - 10000.0).ToString("F3");
        FindObjectOfType<MapGenerator>().DrawMapInEditor();
    }

    public void GeneratorY()
    {
        System.Random rnd = new System.Random();
        offsetY.text = (rnd.NextDouble() * 20000.0 - 10000.0).ToString("F3");
        FindObjectOfType<MapGenerator>().DrawMapInEditor();
    }

    public void UpdateFallOff()
    {
        fallOffRateText.text = (fallOffRate.value * 1.5).ToString("F2");
        FindObjectOfType<MapGenerator>().DrawMapInEditor();
    }

    public void GeneratorSeed()
    {
        System.Random rnd = new System.Random();
        seed.text = (rnd.Next(-10000, 10000)).ToString();
        FindObjectOfType<MapGenerator>().DrawMapInEditor();
    }

    public void ClearNotification()
    {
        notification.text = "";
    }

    public void SaveUserPrefs(){
        if(mapName){
            if (mapName.text == "")
            {
                notification.text = "Please  provide  a  map  name!";
                Invoke(nameof(ClearNotification), 3);
                return;
            }     
            
            if (File.Exists("./Assets/Maps/" + mapName.text))
            {
                notification.text = "This  file  is  already  exist!";
                Invoke(nameof(ClearNotification), 3);
                return;
            }
            
            PlayerPrefs.SetInt("size", Int32.Parse(size.text));
            PlayerPrefs.SetFloat("offsetX", float.Parse(offsetX.text));
            PlayerPrefs.SetFloat("offsetY", float.Parse(offsetY.text));
            PlayerPrefs.SetString("mapName", mapName.text);
            PlayerPrefs.SetFloat("fallOffRate", fallOffRate.value);
            PlayerPrefs.SetInt("seed", Int32.Parse(seed.text));
            
            /*
            float [,] heightMap = FindObjectOfType<MapGenerator>().GetHeightMap();

            MapSerilization mapObject = new MapSerilization();

            mapObject.guid = Guid.NewGuid();
            mapObject.mapData = heightMap;
            mapObject.mapName = mapName.text;

            string binary = Serializer.SerializeToString(mapObject);

            File.WriteAllText("./Assets/Maps/" + mapName.text, binary);
            */
            
            ChangeUIType();
        }
        else
        {
            notification.text = "Unknown Error occured";
            Invoke("ClearNotification", 3);
            return;
        }

    }

    public void ChangeUIType()
    {
        foreach (GameObject UIElement in AllUIElements)
        {
            UIElement.SetActive(!UIElement.activeSelf);
            if (UIElement.activeSelf == true)
            {
                foreach (GameObject editorGameObject in EditorUI)
                {
                    editorGameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(800f, editorGameObject.GetComponent<RectTransform>().sizeDelta.y);
                }
            }
            else
            {
                foreach (GameObject editorGameObject in EditorUI)
                {
                    editorGameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(600f, editorGameObject.GetComponent<RectTransform>().sizeDelta.y);
                }
            }
        }
    }
}
