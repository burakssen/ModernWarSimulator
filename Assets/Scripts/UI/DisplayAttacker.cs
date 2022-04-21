using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DisplayAttacker : MonoBehaviour
{
    public AttackerSelection[] attackerSelections;
    public GameObject uiObject;
    public GameObject uiSeperator;
    public TMP_Dropdown waveCount;
    public GameObject nextButton;
    public GameObject waveLabel;
    private List<GameObject> uIObjects;
    private List<Wave> waves;

    void Start()
    {
        uIObjects = new List<GameObject>();
        waves = new List<Wave>();
        Populate();
    }

    public void Populate()
    {
        foreach (var obj in uIObjects)
        {
            Destroy(obj);
        }
        
        uIObjects.Clear();
        waves.Clear();

        for (int j = 0; j < waveCount.value + 1; j++)
        {
            waves.Add(new Wave());
            waves[j].waveNumber = j + 1;
            GameObject wLabel = Instantiate(waveLabel, transform, false);
            wLabel.GetComponentInChildren<Text>().text = $"{j + 1}. Wave";
            uIObjects.Add(wLabel);
            waves[j].list = new List<Tuple<AttackerSelection, GameObject>>(attackerSelections.Length);
            foreach (var attackerSelection in attackerSelections)
            {
                uIObjects.Add(Instantiate(uiSeperator, transform, false));
                GameObject uiObj = Instantiate(uiObject, transform, false);
                waves[j].list.Add(new Tuple<AttackerSelection, GameObject>(attackerSelection, uiObj));
                uIObjects.Add(uiObj);
                Transform uiObjTransform = uiObject.transform;
                for (int i = 0; i < uiObjTransform.childCount; i++)
                {
                    Transform child = uiObjTransform.GetChild(i);
                    if (child.tag == "Image")
                    {
                        child.gameObject.GetComponent<Image>().sprite = attackerSelection.image.sprite;
                    }

                    if (child.tag == "Label")
                    {
                        child.gameObject.GetComponent<Text>().text = attackerSelection.attackerName;
                    }

                    if (child.tag == "AttackerCount")
                    {
                        child.gameObject.GetComponent<TMP_InputField>().text = "0";
                    }
                }
            }
        }
        
        GameObject button = Instantiate(nextButton, transform, false);
        button.GetComponentInChildren<Button>().onClick.AddListener(FindObjectOfType<MapGeneratorController>().Serialize);
        uIObjects.Add(button);
    }
    public List<Wave> GetWaves()
    {
        return waves;
    }
}

public class Wave
{
    public int waveNumber;
    public List<Tuple<AttackerSelection, GameObject>> list;
}