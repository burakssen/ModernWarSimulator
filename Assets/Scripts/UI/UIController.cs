using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIController : MonoBehaviour
{
    [SerializeField] GameObject[] uiElements;
    [SerializeField] GameObject toggle;
    [SerializeField] GameObject dropDown;
    [SerializeField] Slider fallOffRate;
 
    public void EnableFallOff()
    {
        foreach (GameObject uiElement in uiElements)
        {
            uiElement.SetActive(toggle.GetComponent<Toggle>().isOn);
        }
        fallOffRate.value = 0;
    }

    public void EnableFallOffType(int value)
    {
        var drp = dropDown.GetComponentInChildren<TMP_Dropdown>();
        foreach (GameObject uiElement in uiElements)
        {
            uiElement.SetActive(drp.value == value);
        }
    }
}
