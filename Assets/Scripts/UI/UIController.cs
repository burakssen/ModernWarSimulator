using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIController : MonoBehaviour
{
    [SerializeField] private GameObject[] uiElements;
    [SerializeField] private GameObject toggle;
    [SerializeField] private GameObject dropDown;
    [SerializeField] private Slider fallOffRate;

    public void EnableFallOff()
    {
        foreach (var uiElement in uiElements) uiElement.SetActive(toggle.GetComponent<Toggle>().isOn);
        fallOffRate.value = 0;
    }

    public void EnableFallOffType(int value)
    {
        var drp = dropDown.GetComponentInChildren<TMP_Dropdown>();
        foreach (var uiElement in uiElements) uiElement.SetActive(drp.value == value);
    }
}