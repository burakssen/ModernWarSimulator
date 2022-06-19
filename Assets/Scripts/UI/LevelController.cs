using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.IO;
using System.Linq;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelController : MonoBehaviour
{
    [SerializeField] private GameObject levelUI;

    [SerializeField] private GameObject levelUIParent;
    // Start is called before the first frame update
    void Start()
    {
        var info = new DirectoryInfo(Application.dataPath + "/Maps/");
        var fileInfo = info.GetFiles("*.level");
        foreach (var file in fileInfo)
        {
            var map = File.ReadAllText(file.FullName); 
            MapInfoSerializer mapInfoSerializer = Serializer.DeserializeFromString<MapInfoSerializer>(map);
            GameObject levelui = Instantiate(levelUI, levelUIParent.transform);
            levelui.transform.Find("LevelName").GetComponent<TMP_Text>().text = mapInfoSerializer.mapName;
            levelui.transform.Find("Budget").GetComponent<TMP_Text>().text = "Budget: " + mapInfoSerializer.budget.ToString();
            levelui.transform.Find("Attackers-1").GetComponent<TMP_Text>().text =
                mapInfoSerializer.attackerSelections[0].attackerName + " #" +
                mapInfoSerializer.attackerSelections[0].numberOfAttacker;
            
            levelui.transform.Find("Attackers-2").GetComponent<TMP_Text>().text =
                mapInfoSerializer.attackerSelections[1].attackerName + " #" +
                mapInfoSerializer.attackerSelections[1].numberOfAttacker;
            
            levelui.transform.Find("Button").GetComponent<Button>().onClick.AddListener(() =>
            {
                PlayerPrefs.SetString("LevelName", mapInfoSerializer.mapName);
                SceneLoader.LoadScene("Game");
            });
        }
    }
}
