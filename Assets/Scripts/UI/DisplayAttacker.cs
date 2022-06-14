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
    public GameObject nextButton;
    public GameObject waveLabel;
    private List<List<GameObject>> uIObjects;
    private List<AttackerSelectionSerializer> attackerSelectionSerializers;

    void Start()
    {
        uIObjects = new List<List<GameObject>>();
        attackerSelectionSerializers = new List<AttackerSelectionSerializer>();
        Populate();
    }

    public void Populate()
    {
        foreach (var attackerSelection in attackerSelections)
        {
            GameObject uiObj = Instantiate(uiObject, transform, false);
            AttackerSelectionSerializer attackerSelectionSerializer = new AttackerSelectionSerializer();
            attackerSelectionSerializer.damage = attackerSelection.damage;
            attackerSelectionSerializer.attackerName = attackerSelection.attackerName;
            attackerSelectionSerializer.attackerType = attackerSelection.attackerType;
            attackerSelectionSerializers.Add(attackerSelectionSerializer);
            Transform uiObjTransform = uiObj.transform;
            List<GameObject> uIs = new List<GameObject>();
            for (int i = 0; i < uiObjTransform.childCount; i++)
            {
                Transform child = uiObjTransform.GetChild(i);
                uIs.Add(child.gameObject);
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
            uIObjects.Add(uIs);
        }
        
        GameObject button = Instantiate(nextButton, transform, false);
        button.GetComponentInChildren<Button>().onClick.AddListener(FindObjectOfType<MapGeneratorController>().Serialize);
    }
    public List<AttackerSelectionSerializer> GetAttackerSelectionSerializers()
    {
        foreach (var attacker in attackerSelectionSerializers)
        {
            foreach (var uIObject in uIObjects)
            {
                if (attacker.attackerName == uIObject[1].GetComponent<Text>().text)
                    attacker.numberOfAttacker = Int32.Parse(uIObject[2].GetComponent<TMP_InputField>().text);
            }
           
        }
        return attackerSelectionSerializers;
    }
}
