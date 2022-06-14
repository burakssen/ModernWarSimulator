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
    void Start()
    {
        foreach (var defence in defenceSelections)
        {
            GameObject newButtonObject = Instantiate(selectionButton, buttonParent.transform);
            Button button = newButtonObject.transform.GetChild(0).GetComponent<Button>();
            TMP_Text text = newButtonObject.transform.GetChild(1).GetComponent<TMP_Text>();
            text.text = defence.cost.ToString();
            button.GetComponentInChildren<TMP_Text>().SetText("      "+defence.name);
            button.onClick.AddListener(delegate { SelectObject(defence.name); });
        }

        Button playB = Instantiate(playButton, buttonParent.transform);
        playB.GetComponentInChildren<TMP_Text>().text = "Play";
        playB.onClick.AddListener(ActivatePlay);
    }

    private void Update()
    {
        budget.text = "Budget: " + Global.budget;
    }

    void SelectObject(string name)
    {
        DefenceSelection defenceSelection = defenceSelections.Find(selection =>
        {
            if (selection.name == name)
            {
                return true;
            }

            return false;
        });
        Global.selectedDefence = defenceSelection;
        Global.spawnObject = defenceSelection.gameObject;
        FindObjectOfType<MapLoader>().SetSpawnObject();
    }

    void ActivatePlay()
    {
        Global.gameState = Global.GameState.play;
    }
}
