using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlacerController : MonoBehaviour
{
    [SerializeField] private List<DefenceSelection> defenceSelections;
    [SerializeField] private GameObject selectionButton;
    [SerializeField] private GameObject buttonParent;
    [SerializeField] private TMP_Text budget;
    [SerializeField] private Button playButton;
    [SerializeField] private DefenceSelection baseSelection;
    [SerializeField] private GameObject canvas;
    [SerializeField] private GameObject button;

    private void Start()
    {
        var baseSel = Instantiate(selectionButton, buttonParent.transform);
        var btn = baseSel.transform.GetChild(0).GetComponent<Button>();
        var txt = baseSel.transform.GetChild(1).GetComponent<TMP_Text>();
        txt.text = baseSelection.cost.ToString();
        btn.GetComponentInChildren<TMP_Text>().SetText("      " + baseSelection.defenceName);
        btn.onClick.AddListener(delegate { SelectObject(baseSelection.defenceName, true); });

        foreach (var defence in defenceSelections)
        {
            var newButtonObject = Instantiate(selectionButton, buttonParent.transform);
            var button = newButtonObject.transform.GetChild(0).GetComponent<Button>();
            var text = newButtonObject.transform.GetChild(1).GetComponent<TMP_Text>();
            text.text = defence.cost.ToString();
            button.GetComponentInChildren<TMP_Text>().SetText("      " + defence.defenceName);
            button.onClick.AddListener(delegate { SelectObject(defence.defenceName); });
        }

        var playB = Instantiate(playButton, buttonParent.transform);
        playB.GetComponentInChildren<TMP_Text>().text = "Play";
        playB.onClick.AddListener(ActivatePlay);
    }

    private void Update()
    {
        budget.text = "Budget: " + Global.budget;
    }

    private void SelectObject(string name, bool isBase = false)
    {
        if (isBase)
        {
            Global.selectedDefence = baseSelection;
            Global.spawnObject = baseSelection.gameObject;
            FindObjectOfType<MapLoader>().SetSpawnObject();
            return;
        }

        var defenceSelection = defenceSelections.Find(selection =>
        {
            if (selection.defenceName == name) return true;

            return false;
        });
        Global.selectedDefence = defenceSelection;
        Global.spawnObject = defenceSelection.gameObject;
        FindObjectOfType<MapLoader>().SetSpawnObject();
    }

    private void ActivatePlay()
    {
        FindObjectOfType<MapLoader>().ClearAll();
        Global.gameState = Global.GameState.play;
        button.SetActive(true);
        FindObjectOfType<AttackerSpawner>().spawn = true;
        canvas.SetActive(false);
    }
}