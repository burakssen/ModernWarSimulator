using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.IO;

public class MapGeneratorController : MonoBehaviour
{

    [SerializeField] Slider size;
    [SerializeField] TMP_Text sizeText;
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
        FindObjectOfType<MapGenerator>().DrawMapInEditor();
    }
    public void UpdateMap()
    {
        sizeText.text = (size.value * 200).ToString() ;
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
        fallOffRateText.text = (fallOffRate.value).ToString("F2");
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
            
            PlayerPrefs.SetInt("size", (int)size.value);
            PlayerPrefs.SetFloat("offsetX", float.Parse(offsetX.text));
            PlayerPrefs.SetFloat("offsetY", float.Parse(offsetY.text));
            PlayerPrefs.SetString("mapName", mapName.text);
            PlayerPrefs.SetFloat("fallOffRate", fallOffRate.value);
            PlayerPrefs.SetInt("seed", Int32.Parse(seed.text));
            
            ChangeUIType();
        }
        else
        {
            notification.text = "Unknown Error occured";
            Invoke("ClearNotification", 3);
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

    public void Serialize()
    {
        List<Wave> waves = FindObjectOfType<DisplayAttacker>().GetWaves();
        MapData[] mapDatas = FindObjectOfType<MapGenerator>().GetMapData();
        
        foreach (var wave in waves)
        {

            // FIX ME: burdasın
            //Serializer.SerializeToString();
            Debug.Log(wave.waveNumber);
            foreach (var w in wave.list)
            {
                Debug.Log(w.Item1.name + ", " + w.Item2.transform.GetChild(2).gameObject.GetComponent<TMP_InputField>().text);
            }
        }
    }
}
